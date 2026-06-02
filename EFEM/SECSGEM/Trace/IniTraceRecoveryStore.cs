using System.Collections.Generic;
using System.IO;
using System.Linq;
using FrameOfSystem3.Functional;
using Define.DefineConstant;

namespace FrameOfSystem3.SECSGEM.Trace
{
    public sealed class IniTraceRecoveryStore : ITraceRecoveryStore
    {
        private const string SectionName = "VariableValues";

        private readonly string _traceInfoPath;
        private readonly string _traceDataRecoveryFilePath;

        public IniTraceRecoveryStore()
        {
            _traceInfoPath = $@"{FilePath.FILEPATH_EXE}\..\Recovery\TraceData\Info.ini";
            _traceDataRecoveryFilePath = $@"{FilePath.FILEPATH_EXE}\..\Recovery\TraceData\FdcValues.ini";

            string dir = Path.GetDirectoryName(_traceDataRecoveryFilePath);
            if (false == Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        public bool TryReadTraceInfo(
            out IDictionary<string, long> info,
            out IDictionary<string, string> processOnly,
            out IDictionary<string, string> initialTraceValues)
        {
            info = new Dictionary<string, long>();
            processOnly = new Dictionary<string, string>();
            initialTraceValues = new Dictionary<string, string>();

            if (false == File.Exists(_traceInfoPath))
                return false;

            IniControl ini = new IniControl(_traceInfoPath);

            List<long> ids = new List<long>();
            List<string> names = new List<string>();

            ini.sectionName = "ID";
            int count = ini.GetInt("COUNT", 0);
            for (int i = 0; i < count; ++i)
            {
                ids.Add(ini.GetLong($"Key_{i}", 0));
            }

            ini.sectionName = "NAME";
            count = ini.GetInt("COUNT", 0);
            for (int i = 0; i < count; ++i)
            {
                names.Add(ini.GetString($"Key_{i}", string.Empty));
            }

            for (int i = 0; i < count; ++i)
            {
                info[names[i]] = ids[i];
                initialTraceValues[names[i]] = string.Empty;
            }

            ini.sectionName = "PROCESS";
            count = ini.GetInt("COUNT", 0);
            for (int i = 0; i < count; ++i)
            {
                string value = ini.GetString($"Key_{i}", string.Empty);
                processOnly[value] = string.Empty;
            }

            return true;
        }

        public void WriteTraceInfo(
            IReadOnlyDictionary<string, long> info,
            IReadOnlyDictionary<string, string> processOnly)
        {
            if (File.Exists(_traceInfoPath))
                return;

            IniControl ini = new IniControl(_traceInfoPath);

            List<string> names = info.Keys.ToList();
            List<long> ids = info.Values.ToList();
            List<string> orderedProcessOnly = new List<string>();

            ini.sectionName = "ID";
            ini.WriteInt("COUNT", info.Count);
            for (int i = 0; i < ids.Count; ++i)
            {
                ini.WriteLong($"Key_{i}", ids[i]);
            }

            ini.sectionName = "NAME";
            ini.WriteInt("COUNT", info.Count);
            for (int i = 0; i < names.Count; ++i)
            {
                ini.WriteString($"Key_{i}", names[i]);

                if (processOnly.ContainsKey(names[i]))
                {
                    orderedProcessOnly.Add(names[i]);
                }
            }

            ini.sectionName = "PROCESS";
            ini.WriteInt("COUNT", orderedProcessOnly.Count);
            for (int i = 0; i < orderedProcessOnly.Count; ++i)
            {
                ini.WriteString($"Key_{i}", orderedProcessOnly[i]);
            }
        }

        public bool TryReadLastValues(ref Dictionary<long, string> values)
        {
            if (false == File.Exists(_traceDataRecoveryFilePath))
            {
                WriteLastValues(values);
                return false;
            }

            IniControl ini = new IniControl(_traceDataRecoveryFilePath);
            ini.sectionName = SectionName;

            List<long> ids = values.Keys.ToList();
            for (int i = 0; i < ids.Count; ++i)
            {
                long id = ids[i];
                values[id] = ini.GetString(id.ToString(), "0");
            }

            return true;
        }

        public void WriteLastValues(Dictionary<long, string> values)
        {
            if (File.Exists(_traceDataRecoveryFilePath))
                File.Delete(_traceDataRecoveryFilePath);

            IniControl ini = new IniControl(_traceDataRecoveryFilePath);
            ini.sectionName = SectionName;

            Dictionary<long, string> ordered = values
                .OrderBy(kvp => kvp.Key)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            foreach (KeyValuePair<long, string> item in ordered)
            {
                ini.WriteString(item.Key.ToString(), item.Value);
            }
        }
    }
}