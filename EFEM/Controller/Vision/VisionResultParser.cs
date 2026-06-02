using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision_;

using FrameOfSystem3.Functional;

namespace FrameOfSystem3.Controller.Vision
{
    public class VisionResultParser
    {
        public static VISION_RESULT BARCODE(string[] strData, ref VisionResultData resultData)
        {
            resultData.InitResult();
            if (strData.Length == 2)
            {
                resultData.SetVisionResult(true, strData[1]);
                return VISION_RESULT.COMPLETE;
            }
            else return VISION_RESULT.ERROR_VISION;
        }
        public static VISION_RESULT BP5000_REWORK(string[] strData, ref VisionResultData resultData)
        {
            Vision_.DPointXYT[] resultXYT = new Vision_.DPointXYT[2];
            resultData.InitResult();
            if(strData.Length == 3)
            {
                string[] results = strData[1].Split(',');
                bool[] isSuccess = new bool[5];
                string resultClassification = results[0];
                if (resultClassification.Equals("0") || resultClassification.Equals("1")) isSuccess[0] = true;

                isSuccess[1] = double.TryParse(results[1], out resultXYT[0].x);
                isSuccess[2] = double.TryParse(results[2], out resultXYT[0].y);
                isSuccess[3] = double.TryParse(results[3], out resultXYT[1].x);
                isSuccess[4] = double.TryParse(results[4], out resultXYT[1].y);

                // New
                //bool isNormalBall = (resultClassification.Equals("1"));
                // Old
                bool isNormalBall = (false == resultClassification.Equals("1"));
                
                // if(isSuccess.All(x=>x))
                if (isSuccess[0] && isSuccess[1] && isSuccess[2] && isSuccess[3] && isSuccess[4])
                {
                    resultData.SetVisionResult(true, isNormalBall, resultXYT[0], resultXYT[1]);
                    return VISION_RESULT.COMPLETE;
                }
            }
            return VISION_RESULT.ERROR_VISION;
        }
        public static VISION_RESULT ALIGN_MATCHING_DB(string[] strData, ref VisionResultData resultData)
        {
            Vision_.DPointXYT resultXYT = new Vision_.DPointXYT();
            resultData.InitResult();
            if(strData.Length == 5)
            {
                // 3번 인덱스가 검사 결과 값을 갖고 있다.
                // 데이터가 없을 경우 e로 들어오기 때문에(ex: Blob 의 경우 각도가 없기 때문에 0으로 들어옴) 0으로 바꿔 준다.
                strData[3] = strData[3].Replace("e", "0");       // jhchoo from : 2018.10.18 strTemp[] 의 값이 2개만 들어 있어서 메모리 참조 에러 발생
                string[] results = strData[3].Split(',');

                bool[] isSuccess = new bool[4];
                int nResult = 0;
                isSuccess[0] = int.TryParse(strData[1], out nResult);
                isSuccess[1] = double.TryParse(results[2], out resultXYT.x);
                isSuccess[2] = double.TryParse(results[3], out resultXYT.y);
                isSuccess[3] = double.TryParse(results[4], out resultXYT.t);

                // if(isSuccess.All(x=>x))
                if (isSuccess[0] && isSuccess[1] && isSuccess[2] && isSuccess[3])
                {
                    resultXYT.x /= 1000;
                    resultXYT.y /= 1000;
                    resultXYT.t = MathFormula.radianToDegree(resultXYT.t);
                    resultData.SetVisionResult(nResult == 0, resultXYT);
                    resultData.SetVisionResult(nResult == 0, "");
                    
                    return VISION_RESULT.COMPLETE;
                }
            }
            return VISION_RESULT.ERROR_VISION;
        }
        public static VISION_RESULT PROJECTION4(string[] strData, ref VisionResultData resultData)
        {
            Vision_.DPointXYT resultXYT = new Vision_.DPointXYT();
            resultData.InitResult();
            if (strData.Length == 13)
            {
                // 3번 인덱스가 검사 결과 값을 갖고 있다.
                // 데이터가 없을 경우 e로 들어오기 때문에(ex: Blob 의 경우 각도가 없기 때문에 0으로 들어옴) 0으로 바꿔 준다.
                strData[3] = strData[3].Replace("e", "0");       // jhchoo from : 2018.10.18 strTemp[] 의 값이 2개만 들어 있어서 메모리 참조 에러 발생
                string[] results = strData[3].Split(',');

                bool[] isSuccess = new bool[4];
                int nResult = 0;
                isSuccess[0] = int.TryParse(strData[1], out nResult);
                isSuccess[1] = double.TryParse(results[2], out resultXYT.x);
                isSuccess[2] = double.TryParse(results[3], out resultXYT.y);
                isSuccess[3] = double.TryParse(results[4], out resultXYT.t);

                // if(isSuccess.All(x=>x))
                if (isSuccess[0] && isSuccess[1] && isSuccess[2])
                {
                    resultXYT.x /= 1000;
                    resultXYT.y /= 1000;
                    resultXYT.t = MathFormula.radianToDegree(resultXYT.t);
                    resultData.SetVisionResult(nResult == 0, resultXYT);
                    resultData.SetVisionResult(nResult == 0, "");
                    return VISION_RESULT.COMPLETE;
                }
            }
            return VISION_RESULT.ERROR_VISION;
        }
    }
}
