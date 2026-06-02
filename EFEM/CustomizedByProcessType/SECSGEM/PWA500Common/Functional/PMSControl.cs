using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EFEM.CustomizedByProcessType.PWA500Common
{
    public enum EN_PMSParameters
    {
        B_WD = 0,       // Bin Wafer ID
        B_WX = 1,       // Bonding Position X
        B_WY = 2,       // Bonding Position Y
        B_BN = 3,
        B_GD = 4,
        C_WD = 5,       // Core Wafer ID
        C_WX = 6,       // Picked Position X
        C_WY = 7,       // Picked Position Y
        C_BN = 8,
        ID = 9,
        HD = 10,         // Bonding한 Picker 1: Right, 2: Left 일듯
        ST = 11,
        US = 12,
    }

    public class PMSControl
    {
        #region <Constructor>
        #endregion </Constructor>

        #region <Fields>
        private const string _pmsWaferId = "WAFERID";
        private const string _pmsDataEndToken = "ENDOFFILE";
        #endregion </Fields>

        #region <Methods>
        //public string[] GetPMSFIleDatas(string pmsPath)
        //{
        //    string[] datas = File.ReadAllLines(pmsPath);

        //    return datas;
        //}
        public string GetBinWaferID(string[] pmsData)
        {
            string waferID = string.Empty;

            foreach (string data in pmsData)
            {
                if (data.Contains(";"))
                    data.Replace(";", "");

                if (data.Contains(_pmsWaferId))
                {
                    waferID = data.Trim().Replace(_pmsWaferId, "");
                }
            }
            return waferID;
        }
        public Dictionary<string, List<string[]>> GetTransferedData(string pmsData)
        {
            pmsData = pmsData.Replace("\\r\\n", "\r\n");
            string[] convertedData = pmsData.Split(new[] { "\r\n", "\\n" }, StringSplitOptions.None);
            return GetTransferedData(convertedData);

        }
        public Dictionary<string, List<string[]>> GetTransferedData(string[] pmsData)
        {
            Dictionary<string, List<string[]>> transferdChipsFromCoreWafer = new Dictionary<string, List<string[]>>();
            List<string[]> transferdChips = new List<string[]>();
            List<string> workedCoreWaferId = new List<string>();
            string waferID = string.Empty;
            string temp;
            foreach (string data in pmsData)
            {
                temp = data;
                if (data.Contains(";"))
                    temp = data.Replace(";", "");

                if (temp.Contains(_pmsWaferId))
                {
                    waferID = temp.Replace(_pmsWaferId, "").Trim();
                }
                else if (false == string.IsNullOrWhiteSpace(waferID) && temp.StartsWith(waferID))
                {
                    string[] bondingResults = temp.Split(new string[] { " " }, StringSplitOptions.None);

                    if (false == transferdChipsFromCoreWafer.ContainsKey(bondingResults[(int)EN_PMSParameters.C_WD]))
                        transferdChipsFromCoreWafer.Add(bondingResults[(int)EN_PMSParameters.C_WD], new List<string[]>());

                    transferdChipsFromCoreWafer[bondingResults[(int)EN_PMSParameters.C_WD]].Add(new string[] {
                        bondingResults[(int)EN_PMSParameters.C_WX],
                        bondingResults[(int)EN_PMSParameters.C_WY],
                        bondingResults[(int)EN_PMSParameters.B_WX],
                        bondingResults[(int)EN_PMSParameters.B_WY],
                        bondingResults[(int)EN_PMSParameters.HD]        // 2026.05.18 dwlim [ADD] Bin Map Upload 할 때, E142 Map 객체에 Bond Head정보 추가
                    });
                }
                Console.WriteLine(data);
                if (data.Contains(_pmsDataEndToken))
                {
                    return transferdChipsFromCoreWafer;
                }
            }
            return null;
        }
        #endregion </Methods>
    }

}
