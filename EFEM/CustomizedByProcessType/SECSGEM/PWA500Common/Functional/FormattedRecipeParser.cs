using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

using Define.DefineEnumProject.AppConfig;

namespace EFEM.CustomizedByProcessType.PWA500Common
{
    public enum EN_RECIPEGROUP
    {
        Global,
        SortingStage,
        SupplyOutShuttle,
        SupplyStage,
        SwingHead,
    }
    public class FormattedRecipeParser
    {
        #region <Constructor>
        public FormattedRecipeParser()
        {
            GroupRegex = new Regex(@"^\s*GROUP\s+(.+?)\s*$", RegexOptions.Compiled);
            ValueRegex = new Regex(@"^\s*VALUE\s*=\s*(.*)$", RegexOptions.Compiled);
            EndRegex = new Regex(@"^\s*END\s*$", RegexOptions.Compiled);

            // 키에서 인덱스([1] 같은) 부분을 분리하기 위한 정규식
            IndexSuffixRegex = new Regex(@"^(?<key>.+)\[(?<value>\d+)\]$", RegexOptions.Compiled);
        }
        #endregion </Constructor>

        #region <Singleton>
        public static FormattedRecipeParser Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new FormattedRecipeParser();
                }

                return _instance;
            }
        }
        #endregion </Singleton>

        #region <Fields>
        private static FormattedRecipeParser _instance = null;

        private readonly Regex GroupRegex;
        private readonly Regex ValueRegex;
        private readonly Regex EndRegex;

        // key와 value 분리하기 위한 정규식
        private readonly Regex IndexSuffixRegex;
        #endregion </Fields>

        #region <Methods>

        #region <External>
        /// <summary>
        /// Data를 받아 파싱합니다.
        /// </summary>
        public string ConvertForSendToPM(Dictionary<string, string> data, EN_PROCESS_TYPE processType)
        {
            return BuildRecipeText(data, processType);
        }

        /// <summary>
        /// 문자열 Data를 받아 파싱합니다.
        /// </summary>
        public Dictionary<string, string> ConvertStringToDictionary(string data, EN_PROCESS_TYPE processType)
        {
            if (string.IsNullOrEmpty(data))
                return new Dictionary<string, string>();

            // 다양한 개행 문자(\r\n, \n, \r)를 모두 안전하게 분리
            var lines = data.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

            return ParseRecipeLines(lines, processType);
        }
        /// <summary>
        /// 문자열 배열 Data를 받아 파싱합니다.
        /// </summary>
        public Dictionary<string, string> ConvertStringArrayToDictionary(string[] data, EN_PROCESS_TYPE processType)
        {
            return ParseRecipeLines(data, processType);
        }

        /// <summary>
        /// 파일 경로를 받아 파싱합니다.
        /// </summary>
        public Dictionary<string, string> ConvertFileToDictionary(string filePath, EN_PROCESS_TYPE processType)
        {
            var lines = File.ReadAllLines(filePath);
            return ParseRecipeLines(lines, processType);
        }
        #endregion </External>

        #region <Internal>


        /// <summary>
        /// 이미 읽어들인 라인 배열(or 문자열 컬렉션)을 파싱합니다.
        /// </summary>
        private Dictionary<string, string> ParseRecipeLines(IEnumerable<string> lines, EN_PROCESS_TYPE processType)
        {
            var result = new Dictionary<string, string>();
            var groupStack = new Stack<string>();

            var validGroups = new HashSet<string>(Enum.GetNames(typeof(EN_RECIPEGROUP)));
            string processPrefix = processType.ToString() + "."; // 예: "DIE_TRANSFER_300."

            foreach (var rawLine in lines)
            {
                if (string.IsNullOrWhiteSpace(rawLine))
                    continue;

                var line = rawLine.TrimEnd('\r', '\n');

                if (EndRegex.IsMatch(line))
                {
                    if (groupStack.Count > 0)
                        groupStack.Pop();
                    continue;
                }

                var groupMatch = GroupRegex.Match(line);
                if (groupMatch.Success)
                {
                    groupStack.Push(groupMatch.Groups[1].Value.Trim());
                    continue;
                }

                var valueMatch = ValueRegex.Match(line);
                if (valueMatch.Success)
                {
                    var value = valueMatch.Groups[1].Value.Trim();

                    var stackArr = groupStack.ToArray();
                    if (stackArr.Length >= 3)
                    {
                        string indexName = stackArr[0];
                        string paramName = stackArr[1];
                        string groupName = stackArr[2];

                        if (!validGroups.Contains(groupName))
                            continue;

                        string key = processPrefix + groupName + "." + paramName;

                        if (indexName != "0")
                            key += "[" + indexName + "]";

                        result[key] = value;
                    }
                }
            }

            return result;
        }
        //private Dictionary<string, string> ParseRecipeLines(IEnumerable<string> lines)
        //{
        //    var result = new Dictionary<string, string>();
        //    var groupStack = new Stack<string>(); // Push한 순서대로 쌓임 (Peek = 가장 안쪽 GROUP)

        //    var validGroups = new HashSet<string>(Enum.GetNames(typeof(EN_RECIPEGROUP)));

        //    foreach (var rawLine in lines)
        //    {
        //        if (string.IsNullOrWhiteSpace(rawLine))
        //            continue;

        //        var line = rawLine.TrimEnd('\r', '\n');

        //        // END => 스택 pop
        //        if (EndRegex.IsMatch(line))
        //        {
        //            if (groupStack.Count > 0)
        //                groupStack.Pop();
        //            continue;
        //        }

        //        // GROUP xxx => 스택 push
        //        var groupMatch = GroupRegex.Match(line);
        //        if (groupMatch.Success)
        //        {
        //            groupStack.Push(groupMatch.Groups[1].Value.Trim());
        //            continue;
        //        }

        //        // VALUE = xxx => 현재 스택 기준으로 키 생성
        //        var valueMatch = ValueRegex.Match(line);
        //        if (valueMatch.Success)
        //        {
        //            var value = valueMatch.Groups[1].Value.Trim();

        //            // 스택 순서(Peek 기준 위->아래): Index, ParamName, RecipeGroup, PROCESS, ...
        //            var stackArr = groupStack.ToArray();
        //            if (stackArr.Length >= 3)
        //            {
        //                string valueName = stackArr[0]; // "0" 등
        //                string keyName = stackArr[1]; // ex) AllowValidBincodeForAutoPickHeightDuringAutoRun
        //                string groupName = stackArr[2]; // ex) Global

        //                // EN_RECIPEGROUP에 정의된 그룹만 처리
        //                if (!validGroups.Contains(groupName))
        //                    continue;

        //                string key = groupName + "." + keyName;

        //                // Index가 0이 아닌 값이 여러 개 있을 수 있는 경우 대비
        //                if (valueName != "0")
        //                    key += "[" + valueName + "]";

        //                result[key] = value;
        //            }
        //        }
        //    }

        //    return result;
        //}

        /// <summary>
        /// Dictionary<string, string> (예: "Global.ChipSize_X" -> "10.2706")를
        /// 원본 GROUP PROCESS ... END 텍스트 형식으로 복원합니다.
        /// </summary>
        private string BuildRecipeText(Dictionary<string, string> data, EN_PROCESS_TYPE processType)
        {
            var sb = new StringBuilder();
            string processPrefix = processType.ToString() + "."; // 예: "DIE_TRANSFER_300."

            bool hasAnyMatchingData = data.Keys.Any(k => k.StartsWith(processPrefix, StringComparison.Ordinal));
            if (!hasAnyMatchingData)
                return null; // 또는 return null;

            AppendLine(sb, 0, ">> FileVersion 1.0.0.0");
            AppendLine(sb, 0, "GROUP PROCESS");

            foreach (EN_RECIPEGROUP groupEnum in Enum.GetValues(typeof(EN_RECIPEGROUP)))
            {
                string groupName = groupEnum.ToString();
                string fullPrefix = processPrefix + groupName + "."; // 예: "DIE_TRANSFER_300.Global."

                var entriesInGroup = data
                    .Where(kv => kv.Key.StartsWith(fullPrefix, StringComparison.Ordinal))
                    .ToList();

                if (entriesInGroup.Count == 0)
                    continue;

                AppendLine(sb, 1, $"GROUP {groupName}");

                foreach (var kv in entriesInGroup)
                {
                    // "DIE_TRANSFER_300.Global.ChipSize_X" -> "ChipSize_X"
                    string remainder = kv.Key.Substring(fullPrefix.Length);

                    string paramName = remainder;
                    string indexName = "0";

                    var m = IndexSuffixRegex.Match(remainder);
                    if (m.Success)
                    {
                        paramName = m.Groups["param"].Value;
                        indexName = m.Groups["index"].Value;
                    }

                    AppendLine(sb, 2, $"GROUP {paramName}");
                    AppendLine(sb, 3, $"GROUP {indexName}");
                    AppendLine(sb, 4, $"VALUE = {kv.Value}");
                    AppendLine(sb, 3, "END");
                    AppendLine(sb, 2, "END");
                }

                AppendLine(sb, 1, "END");
            }

            AppendLine(sb, 0, "END");

            return sb.ToString();
        }
        //private string BuildRecipeText(Dictionary<string, string> data)
        //{
        //    var sb = new StringBuilder();
        //    //sb.Append(">> FileVersion 1.0.0.0\r\n");
        //    AppendLine(sb, 0, ">> FileVersion 1.0.0.0");
        //    AppendLine(sb, 0, "GROUP PROCESS");

        //    // EN_RECIPEGROUP 열거형 선언 순서대로 그룹을 순회
        //    foreach (EN_RECIPEGROUP groupEnum in Enum.GetValues(typeof(EN_RECIPEGROUP)))
        //    {
        //        string groupName = groupEnum.ToString();
        //        string prefix = groupName + ".";

        //        // 이 그룹에 속하는 항목만 추출 (딕셔너리에 저장된 순서를 그대로 사용)
        //        var entriesInGroup = data
        //            .Where(kv => kv.Key.StartsWith(prefix, StringComparison.Ordinal))
        //            .ToList();

        //        if (entriesInGroup.Count == 0)
        //            continue; // 해당 그룹에 데이터가 없으면 건너뜀

        //        AppendLine(sb, 1, $"GROUP {groupName}");

        //        foreach (var kv in entriesInGroup)
        //        {
        //            // "Global.ChipSize_X" -> "ChipSize_X"
        //            string remainder = kv.Key.Substring(prefix.Length);

        //            // "SomeParam[1]" 형태면 파라미터명과 인덱스를 분리, 아니면 인덱스는 "0"
        //            string ccode = remainder;
        //            string pparm = "0";

        //            var m = IndexSuffixRegex.Match(remainder);
        //            if (m.Success)
        //            {
        //                ccode = m.Groups["key"].Value;
        //                pparm = m.Groups["value"].Value;
        //            }

        //            AppendLine(sb, 2, $"GROUP {ccode}");
        //            AppendLine(sb, 3, $"GROUP {pparm}");
        //            AppendLine(sb, 4, $"VALUE = {kv.Value}");
        //            AppendLine(sb, 3, "END");
        //            AppendLine(sb, 2, "END");
        //        }

        //        AppendLine(sb, 1, "END");
        //    }

        //    AppendLine(sb, 0, "END");

        //    return sb.ToString();
        //}

        // 들여쓰기(탭)를 붙여서 한 줄을 추가하는 헬퍼
        private void AppendLine(StringBuilder sb, int indentLevel, string content)
        {
            sb.Append('\t', indentLevel).Append(content).Append('\n');
        }
        #endregion </Internal>

        #endregion <Methods>
    }
}
