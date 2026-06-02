using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Vision_;
using Socket_;

namespace FrameOfSystem3.Controller.Vision
{
    public class RequestDataOfProtecVision : VisionRequestData
    {
        public RequestDataOfProtecVision()
        {
            _grabMode = 0;
        }

        public RequestDataOfProtecVision(int grabMode, params object[] param)
        {
            _grabMode = grabMode;
            _additionalDatas = param;
        }
        public RequestDataOfProtecVision(int grabMode, bool bWaitResultEnd = true)
        {
            _grabMode = grabMode;
            _waitResultEnd = bWaitResultEnd;
        }

        public int GrabMode
        {
            get
            {
                return _grabMode;
            }
        }
        public object[] AdditionalDatas
        {
            get
            {
                return _additionalDatas;
            }
        }

        public bool WaitResultEnd
        {
            get
            {
                return _waitResultEnd;
            }
        }

        private int _grabMode;
        private object[] _additionalDatas = null;


        private bool _waitResultEnd = true;

        public void SetAdditionalDatas(int grabMode, params object[] param)
        {
            _grabMode = grabMode;
            _additionalDatas = param;
        }
        public void SetAdditionalDatas(params object[] param)
        {
            _additionalDatas = param;
        }
    }

    public class ProtecVisionController : VisionController
    {
        public ProtecVisionController(int socketIndex)
        {
            _cSocketIndex = socketIndex;
        }

        #region Message Format
        private const string _cProtocolTypeOfOperator = "O";
        private const string _cProtocolTypeOfVision = "V";

        private const string _cCommandVersion = "VER";
        private const string _cCommandSetCurrentRecipe = "SCR";
        private const string _cCommandGetCurrentRecipe = "GCR";
        private const string _cCommandSetCurrentInspectionMode = "SCI";

        private const string _cCommandResetInspectionMap = "RIM";

        private const string _cCommandSetGrabAndInspection = "SGI";
        private const string _cCommandSetGrabEnd = "SGE";
        
        //private const string m_strCommandSetEnableRejudge = "SER";  // 2021.05.11. 현재 사용안함

        private const string _cCommandSetShowMode = "SSM";

        //private const string m_strCommandAlarm = "ALM";  // 2021.05.11. 현재 사용안함
        // private const string m_strChangedResultJudge = "CRJ";  // 2021.05.11. 현재 사용안함

        private const string _cCommandSetTopWindow = "STW";

        // private const string m_strCommandFormSize = "SWS";  // 2021.05.11. 현재 사용안함
        // private const string m_strCommandGetImageSizeRequest = "GIS";  // 2021.05.11. 현재 사용안함

        private const string _cCommandStartMachineCalibration = "SMC";
        private const string _cCommandStartLive = "SLV";      // 2020.04.12. 현재 Vision 팀 구현안됨

        private const string _cCommandSetWindowVisible = "SWV";

        private const string _cCommandCopyRecipeAll = "CRA";

        // private const string _cCommandRequestLastShotImageSave = "SLI"; // 2021.05.11. 현재 사용안함
        private const string _cCommandSetTriggerMode = "STM";

        private const string _cCommandTurnOffVisionProgram = "TVP";

        //private const string _cCommandRequestSaveToFileCurrentAlgorithm = "SVR";   // 2021.05.11. 현재 사용안함
        //private const string m_strCommandRequestMotionMovement = "RMM";  // 2021.05.11. 현재 사용안함

        private const string _cCommandResetResult = "RSR";
        private const string _cCommandSetLayer = "SLR";

        // private const string m_strCommandSetScanPosRequest = "SSP";  // 2021.05.11. 현재 사용안함
        // private const string m_strCommandGetInspectionStateRequest = "GIT";  // 2021.05.11. 현재 사용안함
        // private const string m_strCommandGetScanLineTriggerCountRequest = "GTC";  // 2021.05.11. 현재 사용안함

        private const string _cCommandGetInspectionResult = "GIR";

        //private const string m_strCommandStopForcedInsepctionRequest = "STI";    // 2021.05.11. 현재 사용안함
        //private const string m_strCommandSetSingleShotRequest = "SSH";           // 2021.05.11. 현재 사용안함
        //private const string m_strCommandSetDeviceMapMarking = "SMM";            // 2021.05.11. 현재 사용안함
		private const string _cCommandSetIID = "SII";                  // 2021.05.11. 현재 사용안함	// 2022.02.21 by junho [MOD] 사용 함 

        //private const string m_strCommandReinsectionRequest = "RRI";             // 2021.05.11. 현재 사용안함
        //private const string m_strCommandSetResultModeRequest = "SRM";           // 2021.05.11. 현재 사용안함

        private const string _cCommandSetServerInput = "SSI";

        // SCC
        // SRC

        private const string _cCommandStartMultiCalibrationExcute = "SME";
        private const string _cCommandChangeCalibrationMode = "CCI";

        // GCI
        // GSM

        private const string _cCommandCheckDataExistInMap = "CDM";
        
        // SLD
        // SUL
        private const string _cCommandDeleteRecipe = "DLR";
        private const string _cCommandRecipeNamingRequest = "RNR";

        // SMF

		private const string _cCommandSetID = "SID";

        //private const string m_strCommandSetPID = "SPI"; // 2021.05.11. 현재 사용안함
		// 2022.02.21 by junho [ADD] Set PCB ID 추가
        private const string _cCommandSetPcbID = "SPI";

        private const string _cCommandShowSelectView = "SSV";
        private const string _cCommandSelectPCBResultData = "SPR";
        private const string _cCommandDeletePCBResultData = "DPR";

        // 2023.01.26 by junho [ADD] klarf file용 정보 전달
        private const string _cCommandKlarfInspectionData = "KID";

        private const string _cCommandType_Send = "001";
        private const string _cCommandType_Reply = "002";
        private const string _cCommandType_Receive = "003";

        // 2020.03.06. by shkim. [ADD] BP5000 Grab 후 파일로 결과 확인하는 모드
        private const string _cCommandType_SetSaveFileAfterInspection = "101";

		// 2021.08.20 by junho [ADD] BP5000WIR Wafer Map 초기화 용
		private const string _cCommandType_Send_ForWafer = "011";
		private const string _cCommandType_Reply_ForWafer = "012";

		// 2021.12.01 by junho [ADD] Recipe Save 한번에 하기용
		private const string _cCommandType_Send_ForRecipe = "101";
		private const string _cCommandType_Reply_ForRecipe = "102";

        #endregion

        // 통신관련
        private string _valueOfSTX;
        private string _valueOfETX;

        private Socket _instanceOfSocket = null;
        
        private readonly int _cSocketIndex = 0;

        private TickCounter_.TickCounter _timeoutCheckerForProgram = new TickCounter_.TickCounter();
        private ConcurrentDictionary<int, TickCounter_.TickCounter> _timeoutCheckersForCamera = new ConcurrentDictionary<int, TickCounter_.TickCounter>();
        private ConcurrentDictionary<int, TickCounter_.TickCounter> _timeoutCheckersForLayer = new ConcurrentDictionary<int,TickCounter_.TickCounter>();
        private ConcurrentDictionary<int, TickCounter_.TickCounter> _timeoutCheckersForMap = new ConcurrentDictionary<int, TickCounter_.TickCounter>();

        private ConcurrentDictionary<int, TickCounter_.TickCounter> _delayForMap = new ConcurrentDictionary<int, TickCounter_.TickCounter>();

		// 2023.02.11 by junho [MOD] vision time out 설정 가능하도록 parameter추가
		//private const uint _cReceiveTimeout = 5000;
		private uint _nReceiveTimeout = 5000;

        private ConcurrentQueue<string> _receivedMessages = new ConcurrentQueue<string>();
        private StringBuilder _splitMessageBuffer = new StringBuilder();

        // private bool m_bUseVision = true;

        private string _additinalMessageFromVision = string.Empty;

        private int _cameraCount = 0;
        //private int[] m_nArrResultType = null;

        private ConcurrentDictionary<string, string> _bufferMessagesForParsing = new ConcurrentDictionary<string, string>();
        
        private List<int> _camNumbersRequestedLoadRecipe = new List<int>();
        
        private VisionResultData[] _visionResults = null;
        private int[] _currentResultType;
        
        // public delegate VISION_RESULT DelParsingInspectionReturnData(string[] strData, ref VisionResultData resultData);
        private ConcurrentDictionary<int, ConcurrentDictionary<int, DelParsingInspectionReturnData>> _dicDelParsingResultFromInspectionData = null;

        private bool _visionSkip = false;
        
		// 2023.03.10 by junho [ADD] 마지막으로 load한 Calibration mode 관리기능 추가 (중복 요청 방지)
		private ConcurrentDictionary<int, int> _currentCalibrationMode = new ConcurrentDictionary<int, int>(); // key: cam index, value: mode No.

        #region <외부함수>
        // 2021.08.17. by shkim. [ADD] 비전 초기화 추가
        public override void ResetVision()
        {
            _receivedMessages = new ConcurrentQueue<string>();
            _bufferMessagesForParsing = new ConcurrentDictionary<string, string>();
            _visionResults = new VisionResultData[_cameraCount];
            for (int i = 0; i < _cameraCount; i++ )
            {
                _visionResults[i] = new VisionResultData();
            }
        }
        public override bool InitController(int cameraCount)
        {
            _cameraCount = cameraCount;

            if (_cameraCount < 1) return false;

            _valueOfSTX = System.Text.Encoding.Default.GetString(new byte[1] { 0x02 });
            _valueOfETX = System.Text.Encoding.Default.GetString(new byte[1] { 0x03 });

            _visionResults = new VisionResultData[_cameraCount];
            _currentResultType = new int[_cameraCount];
            // m_nArrResultType = new int[_cameraCount];

            _instanceOfSocket = Socket.GetInstance();

            for (int i = 0; i < _cameraCount; i++ )
            {
                _visionResults[i] = new VisionResultData();
                _timeoutCheckersForCamera.TryAdd(i, new TickCounter_.TickCounter());
            }

                // m_TickCounterForReceiveTimeout = new TickCounter_.TickCounter();    // [CHECKS] Socket Class Receive Timeout 문제 해결 후 제거할 것
                // m_TickCounterForMessage = new TickCounter_.TickCounter();
                //             m_TickCounterForCamera = new TickCounter_.TickCounter[_cameraCount];
                //             m_TickCounterForCamera[0] = new TickCounter_.TickCounter();

			// 2024.04.15 by junho [ADD] 최초 connect을 init에서 하도록 변경
			_instanceOfSocket.Connect(_cSocketIndex);
			return true;
        }
        public override void ExitController()
        {
            RequestProcessClose();
        }
        public override void Execute()
        {
            if (false == IsConnected())
            {
				// 2024.04.15 by junho [MOD] 최초 connect을 init에서 하도록 변경
				//if (_instanceOfSocket == null) return;
				//if (false == _initialize)
				//	_initialize = _instanceOfSocket.Connect(_cSocketIndex);
				//return;
				return;
            }
                
            string receivedMessageFromSocket = string.Empty;
            if(_instanceOfSocket.Receive(_cSocketIndex, ref receivedMessageFromSocket))
            {
                ParseMessageByToken(receivedMessageFromSocket);
            }
            string parsedMessageFromSocket = string.Empty;
            if(_receivedMessages.TryDequeue(out parsedMessageFromSocket))
            {
                // 1. STX, ETX 제거 + 프로토콜, 커맨드, 커맨드타입 확인
                parsedMessageFromSocket = parsedMessageFromSocket.Substring(1, parsedMessageFromSocket.Length - 2);

                string protocolType = parsedMessageFromSocket.Substring(0, 1);
                string command = parsedMessageFromSocket.Substring(1, 3);
                string commandType = parsedMessageFromSocket.Substring(4, 3);
                int dataLength = Convert.ToInt32(parsedMessageFromSocket.Substring(7, 4));

                // 2. 메세지 길이 확인
                if (dataLength + 11 != parsedMessageFromSocket.Length) return;

                string data = parsedMessageFromSocket.Substring(11, dataLength);

                string cameraIndex = string.Empty;

                // 카메라 번호 사용하는 타입인 경우들
                #region <카메라번호Parsing>
//                 if (command.Equals(_cCommandSetGrabAndInspection) && commandType.Equals(_cCommandType_Reply)    // Grab Ack
//                     || (command.Equals(_cCommandSetGrabEnd) && commandType.Equals(_cCommandType_Send))) // Grab End 수신
//                 {
//                     cameraIndex = parsedMessageFromSocket.Substring(11, 1);
//                 }
                if ((command.Equals(_cCommandSetGrabEnd) && commandType.Equals(_cCommandType_Send)) // Grab End 수신
                    || (command.Equals(_cCommandSetGrabAndInspection) && commandType.Equals(_cCommandType_Receive))) // 첫번째가 CamNo
                {
                    cameraIndex = parsedMessageFromSocket.Substring(11, 1);
                }
                else if (command.Equals(_cCommandSetGrabAndInspection)
                    || command.Equals(_cCommandSetGrabEnd)
                    || command.Equals(_cCommandSetCurrentInspectionMode)
                    || command.Equals(_cCommandStartMachineCalibration)
                    || command.Equals(_cCommandChangeCalibrationMode)
                    || command.Equals(_cCommandStartMultiCalibrationExcute)
                    || command.Equals(_cCommandResetInspectionMap)
                    || command.Equals(_cCommandSetServerInput)
                    || (command.Equals(_cCommandSetGrabAndInspection) && commandType.Equals(_cCommandType_Reply))    // Grab Ack (2021.06.22. 확인됨)
                    || (command.Equals(_cCommandSetCurrentRecipe) && false == commandType.Equals(_cCommandType_Reply_ForRecipe))  // 2021.06.22. by shkim. [ADD] 레시피 변경할 때 카메라 인덱스 확인했어야한다.
                    || command.Equals(_cCommandGetCurrentRecipe))   // 2021.07.13. by shkim. [ADD] 현재 레시피 확인 명령 추가
                {
                    cameraIndex = parsedMessageFromSocket.Substring(13, 1);
                }
                #endregion </카메라번호Parsing>

                if (commandType.Equals(_cCommandType_Send) || commandType.Equals(_cCommandType_Reply) || commandType.Equals(_cCommandType_Receive)
                     || commandType.Equals(_cCommandType_Reply_ForWafer) || commandType.Equals(_cCommandType_Reply_ForRecipe))
                {
                    _bufferMessagesForParsing.TryAdd(string.Format("{0}{1}{2}", command, commandType, cameraIndex), data);
                }
            }
        }

        public override bool AddResultParsingDelegate(int cameraIndex, int sceneIndex, DelParsingInspectionReturnData del)
        {
            if(_dicDelParsingResultFromInspectionData == null)
            {
                _dicDelParsingResultFromInspectionData = new ConcurrentDictionary<int, ConcurrentDictionary<int, DelParsingInspectionReturnData>>();
            }
            ConcurrentDictionary<int, DelParsingInspectionReturnData> delParsingResultForCam;
            if (false == _dicDelParsingResultFromInspectionData.TryGetValue(cameraIndex, out delParsingResultForCam))
            {
                delParsingResultForCam = new ConcurrentDictionary<int, DelParsingInspectionReturnData>();
                _dicDelParsingResultFromInspectionData.TryAdd(cameraIndex, delParsingResultForCam);
            }
            DelParsingInspectionReturnData temp = null;
            if(true == delParsingResultForCam.TryGetValue(sceneIndex, out temp))
            {
                delParsingResultForCam.TryRemove(sceneIndex, out temp);
            }
            return delParsingResultForCam.TryAdd(sceneIndex, del);
        }

        // 확인완료 (2021.07.13)
        public override VISION_RESULT SetRunMode(ref int stepNumber, bool trigger, bool visionEnable = true)
        {
            switch(stepNumber)
            {
                case 0:
                    _visionSkip = !visionEnable;
                    if(_visionSkip)
                    {
                        return VISION_RESULT.COMPLETE;
                    }
                    RequestChangeShowMode(trigger ? 2 : 1);
                    ++stepNumber;
                    break;

                case 1:
                    VISION_RESULT result = IsShowModeChanged();
                    if(result != VISION_RESULT.IN_PROCESS)
                    {
                        stepNumber = 0;
                        return result;
                    }
                    break;
            }
            return VISION_RESULT.IN_PROCESS;
        }

        // 확인완료 (2021.07.13)
        public override VISION_RESULT SetShowHide(ref int stepNumber, bool trigger, bool visionEnable = true)
        {
            switch (stepNumber)
            {
                case 0:
                    _visionSkip = !visionEnable;
                    if (_visionSkip)
                    {
                        return VISION_RESULT.COMPLETE;
                    }
                    RequestChangeShowHide(trigger ? 1 : 0);
                    ++stepNumber;
                    break;

                case 1:
                    VISION_RESULT result = IsShowHideChanged();
                    if (result != VISION_RESULT.IN_PROCESS)
                    {
                        ++stepNumber;
                    }
                    break;
                case 2:
                    _visionSkip = !visionEnable;
                    if (_visionSkip)
                    {
                        return VISION_RESULT.COMPLETE;
                    }
                    RequestChangeTopMostMode(trigger ? 1 : 0);
                    ++stepNumber;
                    break;

                case 3:
                    result = IsTopMostModeChangeComplete();
                    if (result != VISION_RESULT.IN_PROCESS)
                    {
                        stepNumber = 0;
                        return result;
                    }
                    break;
            }
            return VISION_RESULT.IN_PROCESS;
        }

        #region <레시피>

        // 확인완료(2021.06.22)
        public override VISION_RESULT LoadRecipe(ref int stepNumber, ref string recipeName)
        {
            switch (stepNumber)
            {
                case 0:
                    if (_visionSkip)
                    {
                        return VISION_RESULT.COMPLETE;
                    }
                    _camNumbersRequestedLoadRecipe.Clear();
                    for (int i = 0; i < _cameraCount; i++)
                    {
						// 2024.04.03 by junho [DEL] 확장자 안붙어서 넘어옴. recipe name에 '.'이 포함이면 레시피 못불러오는 문제 개선
						//recipeName = System.IO.Path.GetFileNameWithoutExtension(recipeName);
                        RequestChangeRecipe(i, recipeName);
                        _camNumbersRequestedLoadRecipe.Add(i);
                        System.Threading.Thread.Sleep(100);
                    }
                    ++stepNumber;
                    break;

                case 1:
                    if (_camNumbersRequestedLoadRecipe.Count > 0)
                    {
                        foreach (var target in _camNumbersRequestedLoadRecipe.ToList())
                        {
                            VISION_RESULT result = IsRecipeChanged(target);
                            if (result == VISION_RESULT.IN_PROCESS) continue;
                            else if (result == VISION_RESULT.ERROR_VISION || result == VISION_RESULT.TIMEOUT_VISION)
                            {
                                stepNumber = 0;
                                _camNumbersRequestedLoadRecipe.Clear();
                                return result;
                            }
                            else if (result == VISION_RESULT.COMPLETE)
                            {   
                                _camNumbersRequestedLoadRecipe.Remove(target);
                                if (_camNumbersRequestedLoadRecipe.Count == 0)
                                {
                                    stepNumber = 0;
                                    return VISION_RESULT.COMPLETE;
                                }
                            }
                        }
                    }
                    else
                    {
                        return VISION_RESULT.ERROR_VISION;
                    }
                    break;
            }
            return VISION_RESULT.IN_PROCESS;
        }
		/// <summary>
		/// 2021.08.13 by junho [ADD] 개별 Cam index 용
		/// </summary>
		public override VISION_RESULT LoadRecipe(ref int stepNumber, ref string recipeName, int cameraIndex)
		{
			switch (stepNumber)
			{
				case 0:
					if (_visionSkip)
					{
						return VISION_RESULT.COMPLETE;
					}
					_camNumbersRequestedLoadRecipe.Clear();
					// 2024.04.03 by junho [DEL] 확장자 안붙어서 넘어옴. recipe name에 '.'이 포함이면 레시피 못불러오는 문제 개선
					//recipeName = System.IO.Path.GetFileNameWithoutExtension(recipeName);
					RequestChangeRecipe(cameraIndex, recipeName);
					_camNumbersRequestedLoadRecipe.Add(cameraIndex);
					++stepNumber;
					break;

				case 1:
					if (_camNumbersRequestedLoadRecipe.Count > 0)
					{
						foreach (var target in _camNumbersRequestedLoadRecipe.ToList())
						{
							VISION_RESULT result = IsRecipeChanged(target);
							if (result == VISION_RESULT.IN_PROCESS) continue;
							else if (result == VISION_RESULT.ERROR_VISION || result == VISION_RESULT.TIMEOUT_VISION)
							{
								stepNumber = 0;
								_camNumbersRequestedLoadRecipe.Clear();
								return result;
							}
							else if (result == VISION_RESULT.COMPLETE)
							{
								_camNumbersRequestedLoadRecipe.Remove(target);
								if (_camNumbersRequestedLoadRecipe.Count == 0)
								{
									stepNumber = 0;
									return VISION_RESULT.COMPLETE;
								}
							}
						}
					}
					break;
			}
			return VISION_RESULT.IN_PROCESS;
		}
		/// <summary>
		/// 2021.12.01 by junho [ADD] Recipe change 한번에 하기 추가
		/// </summary>
		public override VISION_RESULT LoadRecipeAll(ref int stepNumber, ref string recipeName)
		{
			switch (stepNumber)
			{
				case 0:
					if (_visionSkip)
					{
						return VISION_RESULT.COMPLETE;
					}
					// 2024.04.03 by junho [DEL] 확장자 안붙어서 넘어옴. recipe name에 '.'이 포함이면 레시피 못불러오는 문제 개선
					//recipeName = System.IO.Path.GetFileNameWithoutExtension(recipeName);
					RequestChangeRecipeAll(recipeName);
					++stepNumber;
					break;

				case 1:
					VISION_RESULT result = IsRecipeChangedAll();
					if (result != VISION_RESULT.IN_PROCESS)
					{
						stepNumber = 0;
						return result;
					}
					break;
			}
			return VISION_RESULT.IN_PROCESS;
		}

        /// <summary>
        /// 2021.06.22. by shkim. [CHECKS] 프로텍 비전 응답없어서 확인필요
        /// </summary>
        public override VISION_RESULT CopyRecipe(ref int stepNumber, string sourceName, string destinationName)
        {
            switch (stepNumber)
            {
                case 0:
                    if (_visionSkip)
                    {
                        return VISION_RESULT.COMPLETE;
                    }
                    RequestRecipeCopy(sourceName, destinationName);
                    ++stepNumber;
                    break;

                case 1:
                    VISION_RESULT result = IsRecipeCopyCompleted();
                    if (result != VISION_RESULT.IN_PROCESS)
                    {
                        stepNumber = 0;
                        return result;
                    }
                    break;
            }
            return VISION_RESULT.IN_PROCESS;
        }

        /// <summary>
        /// 2021.06.22. by shkim. [CHECKS] 프로텍 비전 응답없어서 확인필요
        /// </summary>
        public override VISION_RESULT GetCurrectRecipe(ref int stepNumber, int cameraNumber)
        {
            switch (stepNumber)
            {
                case 0:
                    if (_visionSkip)
                    {
                        return VISION_RESULT.COMPLETE;
                    }
                    RequestGetCurrentRecipe(cameraNumber);
                    ++stepNumber;
                    break;

                case 1:
                    VISION_RESULT result = IsCurrentRecipeReceived(cameraNumber);
                    if (result == VISION_RESULT.IN_PROCESS) break;
                    else
                    {
                        stepNumber = 0;
                        return result;
                    }
            }
            return VISION_RESULT.IN_PROCESS;
        }

        public override VISION_RESULT DeleteRecipe(ref int stepNumber, string targetRecipeName)
        {
            switch (stepNumber)
            {
                case 0:
                    if (_visionSkip)
                    {
                        return VISION_RESULT.COMPLETE;
                    }
                    RequestDeleteRecipe(targetRecipeName);
                    ++stepNumber;
                    break;

                case 1:
                    VISION_RESULT result = IsRecipeDeleted();
                    if (result != VISION_RESULT.IN_PROCESS)
                    {
                        stepNumber = 0;
                        return result;
                    }
                    break;
            }
            return VISION_RESULT.IN_PROCESS;
        }
        public override VISION_RESULT ChangeRecipeName(ref int stepNumber, string sourceRecipeName, string destinationRecipeName)
        {
            switch (stepNumber)
            {
                case 0:
                    if (_visionSkip)
                    {
                        return VISION_RESULT.COMPLETE;
                    }
                    RequestChangeRecipeName(sourceRecipeName, destinationRecipeName);
                    ++stepNumber;
                    break;

                case 1:
                    VISION_RESULT result = IsRecipeNameChangeComplete();
                    if (result != VISION_RESULT.IN_PROCESS)
                    {
                        stepNumber = 0;
                        return result;
                    }
                    break;
            }
            return VISION_RESULT.IN_PROCESS;
        }
        #endregion </레시피>

        #region <검사>
        /// <summary>
        /// 2021.05.11. by shkim. [ADD] 검사 및 맵 초기화 Request (INS-900 사용중)
        /// </summary>
        /// <param name="nCountOfDeviceX">디바이스 유닛의 X 방향 개수(Insp Unit X 보다 크거나 같아야 함)</param>
        /// <param name="nCountOfDeviceY">디바이스 유닛의 Y 방향 개수(Insp Unit Y 보다 크거나 같아야 함)</param>
        /// <param name="nCountOfUnitX">검사 유닛의 X 방향 개수(1부터)</param>
        /// <param name="nCountOfUnitY">검사 유닛의 Y 방향 개수(1부터)</param>
        /// <param name="nMapDirection">0 : LT_TO_RT, 1 : LT_TO_RT_Z,  2 : LT_TO_LB, 3 : LT_TO_LB_Z</param>
        /// <returns></returns>
        public override VISION_RESULT ResetInspectionAndMap(ref int nStep, int nIndexOfCam, int nCountOfDeviceX, int nCountOfDeviceY, int nCountOfUnitX, int nCountOfUnitY, int nMapDirection)
        {
            switch (nStep)
            {
                case 0:
                    if (_visionSkip)
                    {
                        return VISION_RESULT.COMPLETE;
                    }
                    RequestResetInspectionAndMap(nIndexOfCam, nCountOfDeviceX, nCountOfDeviceY, nCountOfUnitX, nCountOfUnitY, nMapDirection);
                    ++nStep;
                    break;

                case 1:
                    VISION_RESULT result = IsResetInspectionAndMapCompleted(nIndexOfCam);
                    if (result != VISION_RESULT.IN_PROCESS)
                    {
                        nStep = 0;
                        return result;
                    }
                    break;
            }
            return VISION_RESULT.IN_PROCESS;
        }

        /// <summary>
        /// 검사 결과 초기화 (BP5000IR 사용중)
        /// </summary>
        public override VISION_RESULT ResetInspectionResult(ref int nStep, int nMapID)
        {
            switch (nStep)
            {
                case 0:
                    if (_visionSkip)
                    {
                        return VISION_RESULT.COMPLETE;
                    }
                    RequestResetInspectionResult(nMapID);
                    ++nStep;
                    break;

                case 1:
                    VISION_RESULT result = IsResetInspectionResult(nMapID);
                    if (result != VISION_RESULT.IN_PROCESS)
                    {
                        nStep = 0;
                        return result;
                    }
                    break;
            }
            return VISION_RESULT.IN_PROCESS;
        }

		/// <summary>
		/// WAFER 검사 결과 초기화 (BP5000WIR 사용중)
		/// </summary>
		public override VISION_RESULT ResetInspectionResultForWafer(ref int nStep, int nMapID, int groupX, int groupY, int unitX, int unitY, string waferMap)
		{
			switch (nStep)
			{
				case 0:
					if (_visionSkip)
					{
						return VISION_RESULT.COMPLETE;
					}
					RequestResetInspectionResultForWafer(nMapID, groupX, groupY, unitX, unitY, waferMap);
					++nStep;
					break;

				case 1:
					VISION_RESULT result = IsResetInspectionResultForWafer(nMapID);
					if (result != VISION_RESULT.IN_PROCESS)
					{
						nStep = 0;
						return result;
					}
					break;
			}
			return VISION_RESULT.IN_PROCESS;
		}

        // 확인완료(2021.06.22)
        public override VISION_RESULT ChangeScene(ref int stepNumber, int cameraNumber, int algorithmIndex, int inspectionMode = 0)
        {
            switch (stepNumber)
            {
                case 0:
                    if (_visionSkip)
                    {
                        return VISION_RESULT.COMPLETE;
                    }
                    RequestChangeAlgorithm(cameraNumber, algorithmIndex, inspectionMode);
                    ++stepNumber;
                    break;

                case 1:
                    VISION_RESULT result = IsAlgorithmChanged(cameraNumber);
                    if (result != VISION_RESULT.IN_PROCESS)
                    {
                        _currentResultType[cameraNumber] = algorithmIndex;
                        stepNumber = 0;
                        return result;
                    }
                    break;
            }
            return VISION_RESULT.IN_PROCESS;
        }
        ///// <summary>
        ///// Layer 설정 or 해당 Layer 검사 실행 (nTrgMode == 3, BP5000IR에서 사용)
        ///// </summary>
        ///// <param name="nStep"></param>
        ///// <param name="nLayerNo"></param>
        ///// <param name="nTrgMode"></param>
        ///// <returns></returns>
        public override VISION_RESULT SetLayer(ref int nStep, int camIndex, int nMapID, int nTrgMode, int nTargetShot)
        {
            VISION_RESULT result;
            switch (nStep)
            {
                case 0:
                    if (_visionSkip)
                    {
                        return VISION_RESULT.COMPLETE;
                    }
                    RequestSetLayer(camIndex, nMapID, nTrgMode, nTargetShot);
                    if (nTrgMode != 3)
                    {
                        nStep = 2;
                    }
                    else
                    {
                        nStep = 1;
                    }
                    break;

                case 1:
                    result = IsSetLayerCompleted(nMapID);
                    if (result == VISION_RESULT.IN_PROCESS)
                        break;

                    if (result == VISION_RESULT.COMPLETE)
                        nStep = 2;
                    else
                    {
                        nStep = 0;
                        return result;
                    }
                    break;

                case 2:
                    result = IsRequestGrabComplete(camIndex);
                    if (nTrgMode != 3)
                    {
                        if (result != VISION_RESULT.IN_PROCESS)
                        {
                            nStep = 0;
                            return result;
                        }
                    }
                    else
                    {
                        if (result == VISION_RESULT.IN_PROCESS)
                            break;

                        if (result == VISION_RESULT.COMPLETE)
                            nStep = 3;
                        else
                        {
                            nStep = 0;
                            return result;
                        }
                    }
                    break;
                case 3:
                    result = IsGrabEndReceived(camIndex);
					if (result != VISION_RESULT.IN_PROCESS)
					{
						nStep = 0;
						return result;
					}
                    break;
            }
            return VISION_RESULT.IN_PROCESS;
        }
		///// <summary>
		///// Layer 설정 or 해당 Layer 검사 실행 (nTrgMode == 3, BP5000IR에서 사용)
		///// 2021.12.01 by junho [ADD] Wafer용 SetLayer 추가
		///// </summary>
		///// <param name="nStep"></param>
		///// <param name="nLayerNo"></param>
		///// <param name="nTrgMode"></param>
		///// <returns></returns>
		public override VISION_RESULT SetLayerForWafer(ref int nStep, int camIndex, int nMapID, int nTrgMode, int nTargetShot)
		{
			VISION_RESULT result;
			switch (nStep)
			{
				case 0:
					if (_visionSkip)
					{
						return VISION_RESULT.COMPLETE;
					}
					RequestSetLayerForWafer(camIndex, nMapID, nTrgMode, nTargetShot);
					if (nTrgMode != 3)
					{
						nStep = 2;
					}
					else
					{
						nStep = 1;
					}
					break;

				case 1:
					result = IsSetLayerCompletedForWafer(nMapID);
					if (result == VISION_RESULT.IN_PROCESS)
						break;

					if (result == VISION_RESULT.COMPLETE)
						nStep = 2;
					else
					{
						nStep = 0;
						return result;
					}
					break;

				case 2:
					result = IsRequestGrabComplete(camIndex);
					if (nTrgMode != 3)
					{
						if (result != VISION_RESULT.IN_PROCESS)
						{
							nStep = 0;
							return result;
						}
					}
					else
					{
						if (result == VISION_RESULT.IN_PROCESS)
							break;

						if (result == VISION_RESULT.COMPLETE)
							nStep = 3;
						else
						{
							nStep = 0;
							return result;
						}
					}
					break;
				case 3:
					result = IsGrabEndReceived(camIndex);
					if (result != VISION_RESULT.IN_PROCESS)
					{
						nStep = 0;
						return result;
					}
					break;
			}
			return VISION_RESULT.IN_PROCESS;
		}


        // 확인완료(2021.06.22)
        public override VISION_RESULT GrabImage(ref int stepNumber, int cameraNumber, VisionRequestData requestData)
        {
            VISION_RESULT result;
            
            switch (stepNumber)
            {
                case 0:
                    if (_visionSkip)
                    {
                        return VISION_RESULT.COMPLETE;
                    }
                    RequestGrab(cameraNumber, ((RequestDataOfProtecVision)requestData).GrabMode, ((RequestDataOfProtecVision)requestData).AdditionalDatas);
                    ++stepNumber;
                    break;

                case 1:
                    result = IsRequestGrabComplete(cameraNumber);
                    switch(result)
                    {
                        case VISION_RESULT.COMPLETE:
                            if (((RequestDataOfProtecVision)requestData).WaitResultEnd)
                            {
                                ++stepNumber;
                                return VISION_RESULT.IN_PROCESS;
                            }
                            else
                            {
                                stepNumber = 0;
                                return result;
                            }

                        case VISION_RESULT.IN_PROCESS:
                            break;

                        default:
                            stepNumber = 0;
                            return result;
                    }
                    break;

                case 2:
                    result = IsGrabEndReceived(cameraNumber);
                    if (result != VISION_RESULT.IN_PROCESS)
                    {
                        stepNumber = 0;
                        return result;
                    }
                    break;
            }
            return VISION_RESULT.IN_PROCESS;
        }

        public override VISION_RESULT ExecuteInspection(ref int stepNumber, int cameraNumber, VisionRequestData requestData)
        {
            VISION_RESULT result;

            switch (stepNumber)
            {
                case 0:
                    if (_visionSkip)
                    {
                        return VISION_RESULT.COMPLETE;
                    }
                    RequestGrab(cameraNumber, ((RequestDataOfProtecVision)requestData).GrabMode, ((RequestDataOfProtecVision)requestData).AdditionalDatas);
                    ++stepNumber;
                    break;

                case 1:
                    result = IsRequestGrabComplete(cameraNumber);
                    switch (result)
                    {
                        case VISION_RESULT.COMPLETE:
                            ++stepNumber;
                            return VISION_RESULT.IN_PROCESS;

                        case VISION_RESULT.IN_PROCESS:
                            break;

                        default:
                            stepNumber = 0;
                            return result;
                    }
                    break;

                case 2:
                    result = IsGrabEndReceived(cameraNumber);
                    switch (result)
                    {
                        case VISION_RESULT.COMPLETE:
                            ++stepNumber;
                            return VISION_RESULT.IN_PROCESS;

                        case VISION_RESULT.IN_PROCESS:
                            break;

                        default:
                            stepNumber = 0;
                            return result;
                    }
                    break;

                case 3:
                    result = IsInspectionEnd(ref cameraNumber);
                    if (result != VISION_RESULT.IN_PROCESS)
                    {
                        stepNumber = 0;
                        return result;
                    }
                    break;
            }
            return VISION_RESULT.IN_PROCESS;
        }
        public override VISION_RESULT GetInspectionData(ref int nIndexOfCam, ref VisionResultData visionData)
        {
            if (_visionSkip)
            {
                // visionData.InspectResult = true;
                return VISION_RESULT.COMPLETE;
            }
            visionData = _visionResults[nIndexOfCam];

            return visionData.InspectResult ? VISION_RESULT.COMPLETE : VISION_RESULT.ERROR_VISION;
        }

        public override VISION_RESULT WriteResultDataToFile(ref int stepNumber, int nMapID)
        {
            VISION_RESULT result;
            switch(stepNumber)
            {
                case 0:
                    if (_visionSkip)
                    {
                        return VISION_RESULT.COMPLETE;
                    }
                    RequestResultDataWrite(nMapID);
                    ++stepNumber;
                    break;

                case 1:
                    result = IsResultDataWriteAckReceived(nMapID);
                    if (result != VISION_RESULT.IN_PROCESS)
                    {
                        stepNumber = 0;
                        return result;
                    }
                    break;
            }
            return VISION_RESULT.IN_PROCESS;
        }

        public override VISION_RESULT CheckInspectionFinishedAtMap(ref int stepNumber, int nMapID)
        {
            VISION_RESULT result;
            switch (stepNumber)
            {
                case 0:
                    if (_visionSkip)
                    {
                        return VISION_RESULT.COMPLETE;
                    }
                    if (_delayForMap.ContainsKey(nMapID) && false == _delayForMap[nMapID].IsTickOver(true))
                    {
                        break;
                    }
                    RequestCheckDataExistInMap(nMapID);
                    ++stepNumber;
                    break;

                case 1:
                    result = IsDataExistInMap(nMapID);
                    if (result != VISION_RESULT.IN_PROCESS && result != VISION_RESULT.NOT_COMPLETE)
                    {
                        stepNumber = 0;
                        TickCounter_.TickCounter delay = null;
                        _delayForMap.TryRemove(nMapID, out delay);
                        return result;
                    }
                    else if(result == VISION_RESULT.NOT_COMPLETE)
                    {
                        if (false == _delayForMap.ContainsKey(nMapID))
                        {
                            _delayForMap.TryAdd(nMapID, new TickCounter_.TickCounter());
                        }
                        _delayForMap[nMapID].SetTickCount(1000);
                        result = VISION_RESULT.IN_PROCESS;
                        --stepNumber;
                        break;
                    }
                    break;
            }
            return VISION_RESULT.IN_PROCESS;
        }

        public override VISION_RESULT SetMaterialID(ref int stepNumber, int nCamIndex, string pcbID)
        {
            VISION_RESULT result;
            switch(stepNumber)
            {
                case 0:
                    if (_visionSkip)
                    {
                        return VISION_RESULT.COMPLETE;
                    }
					RequestSetPCBID(nCamIndex, pcbID);
                    ++stepNumber;
                    break;

                case 1:
					result = IsSetPCBIdCompleted(nCamIndex);
                    if (result != VISION_RESULT.IN_PROCESS)
                    {
                        stepNumber = 0;
                        return result;
                    }
                    break;
            }
            return VISION_RESULT.IN_PROCESS;
        }
        public override VISION_RESULT SelectMaterialIDForResultViewer(ref int stepNumber, string pcbID)
        {
            VISION_RESULT result;
            switch (stepNumber)
            {
                case 0:
                    if (_visionSkip)
                    {
                        return VISION_RESULT.COMPLETE;
                    }
                    RequestSelectPCBResultData(pcbID);
                    ++stepNumber;
                    break;

                case 1:
                    result = IsRequestSelectPCBResultDataAckReceived();
                    if (result != VISION_RESULT.IN_PROCESS)
                    {
                        stepNumber = 0;
                        return result;
                    }
                    break;
            }
            return VISION_RESULT.IN_PROCESS;
        }

        public override VISION_RESULT ShowSelectViewer(ref int stepNumber, int viewerIndex)
        {
            VISION_RESULT result;
            switch(stepNumber)
            {
                case 0:
                    if (_visionSkip)
                    {
                        return VISION_RESULT.COMPLETE;
                    }
                    RequestShowSelectView(viewerIndex);
                    ++stepNumber;
                    break;

                case 1:
                    result = IsShowSelectViewAckReceived();
                    if (result != VISION_RESULT.IN_PROCESS)
                    {
                        stepNumber = 0;
                        return result;
                    }
                    break;
            }
            return VISION_RESULT.IN_PROCESS;
        }

        public override VISION_RESULT DeleteResultData(ref int stepNumber, string pcbID)
        {
            VISION_RESULT result;
            switch (stepNumber)
            {
                case 0:
                    if (_visionSkip)
                    {
                        return VISION_RESULT.COMPLETE;
                    }
                    RequestDeletePCBResultData(pcbID);
                    ++stepNumber;
                    break;

                case 1:
                    result = IsRequestDeletePCBResultDataAckReceived();
                    if (result != VISION_RESULT.IN_PROCESS)
                    {
                        stepNumber = 0;
                        return result;
                    }
                    break;
            }
            return VISION_RESULT.IN_PROCESS;
        }

		// 2021.08.30 by junho [ADD] 추가로 받아야 할 필요 있어서 추가 (BP5000WIR)
		public override VISION_RESULT IsInspectionEnd(ref int nIndexOfCam)
		{
			VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandSetGrabAndInspection, _cCommandType_Receive, nIndexOfCam.ToString(), true);
			if (VISION_RESULT.IN_PROCESS == result)
			{
				if (_timeoutCheckersForCamera[nIndexOfCam].IsTickOver(false))
				{
					return VISION_RESULT.TIMEOUT_VISION;
				}
			}
			return result;
		}
        public override VISION_RESULT SetIID(ref int stepNumber, int nCamIndex, double errorX, double errorY, int column, int row)
        {
            VISION_RESULT result;
            switch(stepNumber)
            {
                case 0:
                    if (_visionSkip)
                    {
                        return VISION_RESULT.COMPLETE;
                    }
					RequestSetIid(nCamIndex, errorX, errorY, column, row);
                    ++stepNumber;
                    break;

                case 1:
					result = IsSetIidCompleted(nCamIndex);
                    if (result != VISION_RESULT.IN_PROCESS)
                    {
                        stepNumber = 0;
                        return result;
                    }
                    break;
            }
            return VISION_RESULT.IN_PROCESS;
        }

		// 2023.01.26 by junho [ADD] klarf file용 정보 전달
		public override VISION_RESULT SetKlarfData(ref int stepNumber, int nCamIndex, double subjectDiameter, string lotID, Vision_.DPointXY unitSize, string subjectID, int slotNo, int inspectionCount, Vision_.DPointXY leftTopToCenterDistance)
		{
			VISION_RESULT result;
			switch (stepNumber)
			{
				case 0:
					if (_visionSkip)
					{
						return VISION_RESULT.COMPLETE;
					}
					RequestSetKlarfData(nCamIndex, subjectDiameter, lotID, unitSize, subjectID, slotNo, inspectionCount, leftTopToCenterDistance);
					++stepNumber;
					break;

				case 1:
					result = IsSetKlarfData(nCamIndex);
					if (result != VISION_RESULT.IN_PROCESS)
					{
						stepNumber = 0;
						return result;
					}
					break;
			}
			return VISION_RESULT.IN_PROCESS;
		}
		#endregion </검사>

		#region <캘리브레이션>
		public override VISION_RESULT ExcuteResolutionCalibration(ref int stepNumber, int nIndexOfCam, double dDistanceX, double dDistanceY)
        {
            VISION_RESULT result;
            switch(stepNumber)
            {
                case 0:
                    if (_visionSkip)
                    {
                        return VISION_RESULT.COMPLETE;
                    }
                    RequestRunResolutionCalibration(nIndexOfCam, dDistanceX, dDistanceY);
                    ++stepNumber;
                    break;

                case 1:
                    result = IsResolutionCalibrationRun(nIndexOfCam);
                    if (result != VISION_RESULT.IN_PROCESS)
                    {
                        stepNumber = 0;
                        return result;
                    }
                    break;
            }
            return VISION_RESULT.IN_PROCESS;
        }

		public override void ResetCurrentCalibrationMode()
		{
			_currentCalibrationMode.Clear();
		}
        public override VISION_RESULT ChangeCalibrationMode(ref int stepNumber, int nIndexOfCam, int calibrationScene)
        {
            VISION_RESULT result;

            switch (stepNumber)
            {
                case 0:
                    if (_visionSkip)
                    {
                        return VISION_RESULT.COMPLETE;
                    }

					// 2023.03.10 by junho [ADD] 마지막으로 load한 Calibration mode 관리기능 추가 (중복 요청 방지)
					if (_currentCalibrationMode.ContainsKey(nIndexOfCam)
						&& _currentCalibrationMode[nIndexOfCam] == calibrationScene)
						return VISION_RESULT.COMPLETE;

					++stepNumber;
					break;

				case 1:
					RequestChangeCalibrationMode(nIndexOfCam, calibrationScene);
                    ++stepNumber;
                    break;

                case 2:
                    result = IsCalibrationModeChanged(nIndexOfCam);
                    if (result != VISION_RESULT.IN_PROCESS)
                    {
                        stepNumber = 0;

						// 2023.03.10 by junho [ADD] 마지막으로 load한 Calibration mode 관리기능 추가 (중복 요청 방지)
						if (result == VISION_RESULT.COMPLETE)
						{
							_currentCalibrationMode.AddOrUpdate(nIndexOfCam, calibrationScene, (key, currentValue) => { return calibrationScene; });
						}

                        return result;
                    }
                    break;
            }
            return VISION_RESULT.IN_PROCESS;
        }
        public override VISION_RESULT ExcuteMultiCalibration(ref int stepNumber, int nIndexOfCam, int calibrationScene, int calibrationMode, double dDistanceX, double dDistanceY)
        {
            VISION_RESULT result;
            switch (stepNumber)
            {
                case 0:
                    if (_visionSkip)
                    {
                        return VISION_RESULT.COMPLETE;
                    }
					RequestExcuteMultiCalibration(nIndexOfCam, calibrationScene, calibrationMode, dDistanceX, dDistanceY);
                    ++stepNumber;
                    break;

                case 1:
                    result = IsRequestExcuteMultiCalibrationAckReceived(nIndexOfCam);
                    if (result != VISION_RESULT.IN_PROCESS)
                    {
                        stepNumber = 0;
                        return result;
                    }
                    break;
            }
            return VISION_RESULT.IN_PROCESS;
        }
        #endregion </캘리브레이션>

		// 2023.02.11 by junho [MOD] vision time out 설정 가능하도록 parameter추가
		public override bool SetVisionTimeout(int time)
		{
			if (time < 1) return false;
			_nReceiveTimeout = (uint)time;
			return true;
		}

        #endregion </외부함수>

        #region <설정관련>
        private bool RequestGetVersion()
        {
            return SendMessage(_cProtocolTypeOfOperator,
                _cCommandVersion,
                _cCommandType_Send,
                "000",
                _timeoutCheckerForProgram);
        }
        private VISION_RESULT IsVersionReceived()
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandVersion, _cCommandType_Reply);
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if (_timeoutCheckerForProgram.IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
        }

        private bool RequestProcessClose()
        {
            return SendMessage(_cProtocolTypeOfOperator,
                _cCommandTurnOffVisionProgram,
                _cCommandType_Send,
                "1",
                _timeoutCheckerForProgram);
        }
        private VISION_RESULT IsProcessCloseAckReceived()
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandTurnOffVisionProgram, _cCommandType_Reply);
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if (_timeoutCheckerForProgram.IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
        }

        
        /// <summary>
        /// SHOW MODE 전환 (RUN<->SETUP)
        /// </summary>
        /// <param name="nShowMode">1 : SETUP, 2 : RUN</param>
        /// <returns></returns>
        private bool RequestChangeShowMode(int nShowMode)
        {
            return SendMessage(_cProtocolTypeOfOperator,
                _cCommandSetShowMode,
                _cCommandType_Send,
                nShowMode.ToString(),
                _timeoutCheckerForProgram);
        }
        private VISION_RESULT IsShowModeChanged()
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandSetShowMode, _cCommandType_Reply);
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if (_timeoutCheckerForProgram.IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
        }


        /// <summary>
        /// TOP MOST 설정
        /// </summary>
        /// <param name="nTopMost">0 : NONE TOP MOST, 1 : TOP MOST</param>
        /// <returns></returns>
        private bool RequestChangeTopMostMode(int nTopMost)
        {
            return SendMessage(_cProtocolTypeOfOperator,
                _cCommandSetTopWindow,
                _cCommandType_Send,
                nTopMost.ToString(),
                _timeoutCheckerForProgram);
        }
        private VISION_RESULT IsTopMostModeChangeComplete()
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandSetTopWindow, _cCommandType_Reply);
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if (_timeoutCheckerForProgram.IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
        }

        /// <summary>
        /// SHOW, HIDE 변경
        /// </summary>
        /// <param name="nMode">0 : HIDE, 1 : SHOW</param>
        /// <returns></returns>
        private bool RequestChangeShowHide(int nMode)
        {
            return SendMessage(_cProtocolTypeOfOperator,
                _cCommandSetWindowVisible,
                _cCommandType_Send,
                nMode.ToString(),
                _timeoutCheckerForProgram);
        }
        private VISION_RESULT IsShowHideChanged()
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandSetWindowVisible, _cCommandType_Reply);
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if (_timeoutCheckerForProgram.IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
        }


        /// <summary>
        /// 라이브 모드 설정
        /// </summary>
        /// <param name="nMode">0 : STOP, 1 : START</param>
        /// <returns></returns>
        private bool RequestChangeLiveMode(int nIndexOfCam, int nMode)
        {
            if(false == _timeoutCheckersForCamera.ContainsKey(nIndexOfCam))
            {
                _timeoutCheckersForCamera.TryAdd(nIndexOfCam, new TickCounter_.TickCounter());
            }
            string strData = string.Format("{0};{1}", nIndexOfCam, nMode);
            return SendMessage(_cProtocolTypeOfOperator,
                _cCommandStartLive,
                _cCommandType_Send,
                strData,
                _timeoutCheckersForCamera[nIndexOfCam]);
            
        }
        private VISION_RESULT IsLiveModeChanged(int nIndexOfCam)
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandStartLive, _cCommandType_Reply, nIndexOfCam.ToString());
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if (_timeoutCheckersForCamera[nIndexOfCam].IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
        }
        
        #endregion </설정관련>

        #region <캘리브레이션>
        private bool RequestRunResolutionCalibration(int nIndexOfCam, double dDistanceX, double dDistanceY)
        {
            if (false == _timeoutCheckersForCamera.ContainsKey(nIndexOfCam))
            {
                _timeoutCheckersForCamera.TryAdd(nIndexOfCam, new TickCounter_.TickCounter());
            }
            string strData = string.Format("{0};{1};{2}", nIndexOfCam, dDistanceX, dDistanceY);
            return SendMessage(_cProtocolTypeOfOperator,
                _cCommandStartMachineCalibration,
                _cCommandType_Send,
                strData,
                _timeoutCheckersForCamera[nIndexOfCam]);
            
        }
        private VISION_RESULT IsResolutionCalibrationRun(int nIndexOfCam)
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandStartMachineCalibration, _cCommandType_Reply, nIndexOfCam.ToString());
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if (_timeoutCheckersForCamera[nIndexOfCam].IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
        }

        private bool RequestChangeCalibrationMode(int nIndexOfCam, int nIndexOfCalibrationScene)
        {
            if (false == _timeoutCheckersForCamera.ContainsKey(nIndexOfCam))
            {
                _timeoutCheckersForCamera.TryAdd(nIndexOfCam, new TickCounter_.TickCounter());
            }
			string strData = string.Format("{0};{1}", nIndexOfCam, nIndexOfCalibrationScene);
            return SendMessage(_cProtocolTypeOfOperator,
                _cCommandChangeCalibrationMode,
                _cCommandType_Send,
                strData,
                _timeoutCheckersForCamera[nIndexOfCam]);   
        }
        private VISION_RESULT IsCalibrationModeChanged(int nIndexOfCam)
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandChangeCalibrationMode, _cCommandType_Reply, nIndexOfCam.ToString());
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if (_timeoutCheckersForCamera[nIndexOfCam].IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
        }

        private bool RequestExcuteMultiCalibration(int nIndexOfCam, int nScene, int nMode, double dDistanceX, double dDistanceY)
        {
            if (false == _timeoutCheckersForCamera.ContainsKey(nIndexOfCam))
            {
                _timeoutCheckersForCamera.TryAdd(nIndexOfCam, new TickCounter_.TickCounter());
            }

            // 2021.08.14 by junho [MOD] Calibration Scene 추가되어야 함
            //string strData = string.Format("{0};{1};{2};{3}", nIndexOfCam, nMode, dDistanceX, dDistanceY);
			string strData = string.Format("{0};{1};{2};{3};{4}", nIndexOfCam, nScene, nMode, dDistanceX, dDistanceY);
            return SendMessage(_cProtocolTypeOfOperator,
                _cCommandStartMultiCalibrationExcute,
                _cCommandType_Send,
                strData,
                _timeoutCheckersForCamera[nIndexOfCam]);
        }
        private VISION_RESULT IsRequestExcuteMultiCalibrationAckReceived(int nIndexOfCam)
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandStartMultiCalibrationExcute, _cCommandType_Reply, nIndexOfCam.ToString());
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if (_timeoutCheckersForCamera[nIndexOfCam].IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
        }
        #endregion </캘리브레이션>

        #region <레시피>

        private bool RequestChangeRecipe(int nIndexOfCam, string strRecipeName)
        {
            string strData = string.Format("{0};{1}", nIndexOfCam, strRecipeName);

            return SendMessage(_cProtocolTypeOfOperator,
                _cCommandSetCurrentRecipe,
                _cCommandType_Send,
                strData,
                _timeoutCheckersForCamera[nIndexOfCam]);
        }
        private VISION_RESULT IsRecipeChanged(int nIndexOfCam)
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandSetCurrentRecipe, _cCommandType_Reply, nIndexOfCam.ToString());
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if(_timeoutCheckersForCamera[nIndexOfCam].IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
        }

		/// <summary>
		/// 2021.12.01 by junho [ADD] Recipe change 한번에 하는 프로토콜 추가
		/// </summary>
		private bool RequestChangeRecipeAll(string strRecipeName)
		{
			string strData = string.Format("{0}", strRecipeName);

			return SendMessage(_cProtocolTypeOfOperator,
				_cCommandSetCurrentRecipe,
				_cCommandType_Send_ForRecipe,
				strData,
				_timeoutCheckerForProgram);
		}
		private VISION_RESULT IsRecipeChangedAll()
		{
			VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandSetCurrentRecipe, _cCommandType_Reply_ForRecipe);
			if (VISION_RESULT.IN_PROCESS == result)
			{
				if (_timeoutCheckerForProgram.IsTickOver(false))
				{
					return VISION_RESULT.TIMEOUT_VISION;
				}
			}
			return result;
		}

        private bool RequestRecipeCopy(string strSourceName, string strDestName)
        {
            string strData = string.Format("{0};{1}", strSourceName, strDestName);

            return SendMessage(_cProtocolTypeOfOperator,
                _cCommandCopyRecipeAll,
                _cCommandType_Send,
                strData,
                _timeoutCheckerForProgram);
            
        }
        private VISION_RESULT IsRecipeCopyCompleted()
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandCopyRecipeAll, _cCommandType_Reply);
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if (_timeoutCheckerForProgram.IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
        }

        private bool RequestGetCurrentRecipe(int nIndexOfCam)
        {
            return SendMessage(_cProtocolTypeOfOperator,
                _cCommandGetCurrentRecipe,
                _cCommandType_Send,
                nIndexOfCam.ToString(),
                _timeoutCheckersForCamera[nIndexOfCam]);
            
        }
        private VISION_RESULT IsCurrentRecipeReceived(int nIndexOfCam)
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandGetCurrentRecipe, _cCommandType_Reply, nIndexOfCam.ToString());
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if (_timeoutCheckersForCamera[nIndexOfCam].IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
        }

        private bool RequestDeleteRecipe(string strRecipeName)
        {
            return SendMessage(_cProtocolTypeOfOperator,
                _cCommandDeleteRecipe,
                _cCommandType_Send,
                strRecipeName,
                _timeoutCheckerForProgram);
        }
        private VISION_RESULT IsRecipeDeleted()
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandDeleteRecipe, _cCommandType_Reply);
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if (_timeoutCheckerForProgram.IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
        }

        private bool RequestChangeRecipeName(string strSourceName, string strDestName)
        {
            string strData = string.Format("{0};{1}", strSourceName, strDestName);
            return SendMessage(_cProtocolTypeOfOperator,
                _cCommandRecipeNamingRequest, 
                _cCommandType_Send,
                strData,
                _timeoutCheckerForProgram);
        }
        private VISION_RESULT IsRecipeNameChangeComplete()
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandRecipeNamingRequest, _cCommandType_Reply);
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if (_timeoutCheckerForProgram.IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
        }
        #endregion </레시피>

        #region <검사결과관련설정>
        /// <summary>
        /// 2021.05.11. by shkim. [ADD] 검사 및 맵 초기화 Request
        /// </summary>
        /// <param name="nIndexOfCam"></param>
        /// <param name="nCountOfDeviceX">디바이스 유닛의 X 방향 개수(Insp Unit X 보다 크거나 같아야 함)</param>
        /// <param name="nCountOfDeviceY">디바이스 유닛의 Y 방향 개수(Insp Unit Y 보다 크거나 같아야 함)</param>
        /// <param name="nCountOfUnitX">검사 유닛의 X 방향 개수(1부터)</param>
        /// <param name="nCountOfUnitY">검사 유닛의 Y 방향 개수(1부터)</param>
        /// <param name="nMapDirection">0 : LT_TO_RT, 1 : LT_TO_RT_Z,  2 : LT_TO_LB, 3 : LT_TO_LB_Z</param>
        /// <returns></returns>
        private bool RequestResetInspectionAndMap(int nIndexOfCam, int nCountOfDeviceX, int nCountOfDeviceY, int nCountOfUnitX, int nCountOfUnitY, int nMapDirection)
        {
            string strData = string.Format("{0};{1};{2};{3};{4};{5}", nIndexOfCam, nCountOfDeviceX, nCountOfDeviceY, nCountOfUnitX, nCountOfUnitY, nMapDirection);

            return SendMessage(_cProtocolTypeOfOperator,
                _cCommandResetInspectionMap,
                _cCommandType_Send,
                strData,
                _timeoutCheckersForCamera[nIndexOfCam]);
        }
        private VISION_RESULT IsResetInspectionAndMapCompleted(int nIndexOfCam)
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandResetInspectionMap, _cCommandType_Reply, nIndexOfCam.ToString());
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if (_timeoutCheckersForCamera[nIndexOfCam].IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
        }


        private bool RequestResetInspectionResult(int nMapID)
        {
            if (false == _timeoutCheckersForMap.ContainsKey(nMapID))
            {
                _timeoutCheckersForMap.TryAdd(nMapID, new TickCounter_.TickCounter());
            }
            return SendMessage(_cProtocolTypeOfOperator,
                _cCommandResetResult,
                _cCommandType_Send,
                nMapID.ToString(),
                _timeoutCheckersForMap[nMapID]);
        }
        private VISION_RESULT IsResetInspectionResult(int nMapID)
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandResetResult, _cCommandType_Reply);
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if (_timeoutCheckersForMap[nMapID].IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
        }

		private bool RequestResetInspectionResultForWafer(int nMapID, int groupX, int groupY, int unitX, int unitY, string waferMap)
		{
			if (false == _timeoutCheckersForMap.ContainsKey(nMapID))
			{
				_timeoutCheckersForMap.TryAdd(nMapID, new TickCounter_.TickCounter());
			}

			string strData = string.Format("{0};{1};{2};{3};{4};{5}", nMapID, groupX, groupY, unitX, unitY, waferMap);

			return SendMessage(_cProtocolTypeOfOperator,
				_cCommandResetResult,
				_cCommandType_Send_ForWafer,
				strData,
				_timeoutCheckersForMap[nMapID]);
		}
		private VISION_RESULT IsResetInspectionResultForWafer(int nMapID)
		{
			VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandResetResult, _cCommandType_Reply_ForWafer);
			if (VISION_RESULT.IN_PROCESS == result)
			{
				if (_timeoutCheckersForMap[nMapID].IsTickOver(false))
				{
					return VISION_RESULT.TIMEOUT_VISION;
				}
			}
			return result;
		}


        /// <summary>
        /// Set Layer or InspChange and Grab and Inspection
        /// </summary>
        /// <param name="nLayerNo"></param>
        /// <param name="nTriggerMode"></param>
        /// <param name="nShotNo">1: Soft Trg, 2: HW Trg, 3 : InspChange and Grab and Inspection</param>
        /// <returns></returns>
        private bool RequestSetLayer(int cameraIndex, int nMapID, int nTriggerMode, int nShotNo = -1)
        {
            if (false == _timeoutCheckersForLayer.ContainsKey(nMapID))
            {
                _timeoutCheckersForLayer.TryAdd(nMapID, new TickCounter_.TickCounter());
            }

            string strData = string.Empty;
            if(nTriggerMode == 3)
            {
                strData = string.Format("{0};{1};{2}", nMapID, nTriggerMode, nShotNo);
                return SendMessage(_cProtocolTypeOfOperator,
               _cCommandSetLayer,
               _cCommandType_Send,
               strData,
               _timeoutCheckersForLayer[nMapID]);  
            }
            else
            {
                strData = string.Format("{0};{1}", nMapID, nTriggerMode);
                return SendMessage(_cProtocolTypeOfOperator,
                _cCommandSetLayer,
                _cCommandType_Send,
                strData,
                _timeoutCheckersForCamera[cameraIndex]);   
            }
            
        }
		/// <summary>
		/// Set Layer or InspChange and Grab and Inspection
		/// 2021.12.01 by junho [ADD] Apply new protocal for wafer
		/// </summary>
		/// <param name="nLayerNo"></param>
		/// <param name="nTriggerMode"></param>
		/// <param name="nShotNo">1: Soft Trg, 2: HW Trg, 3 : InspChange and Grab and Inspection</param>
		/// <returns></returns>
		private bool RequestSetLayerForWafer(int cameraIndex, int nMapID, int nTriggerMode, int nShotNo = -1)
		{
			if (false == _timeoutCheckersForLayer.ContainsKey(nMapID))
			{
				_timeoutCheckersForLayer.TryAdd(nMapID, new TickCounter_.TickCounter());
			}
			
			string strData = string.Empty;
			if (nTriggerMode == 3)
			{
				// 2022.04.13 by junho [ADD] Set layer만 계속 할 경우 IsGrabMessage에서 timeout 발생. TimeCheckerCamera의 reset이 안되기 때문. 버그 개선
				ResetTimeCheckerByManual(_timeoutCheckersForCamera[cameraIndex]);

				strData = string.Format("{0};{1};{2}", nMapID, nTriggerMode, nShotNo);
				return SendMessage(_cProtocolTypeOfOperator,
			   _cCommandSetLayer,
			   _cCommandType_Send_ForWafer,
			   strData,
			   _timeoutCheckersForLayer[nMapID]);
			}
			else
			{
				strData = string.Format("{0};{1}", nMapID, nTriggerMode);
				return SendMessage(_cProtocolTypeOfOperator,
				_cCommandSetLayer,
				_cCommandType_Send_ForWafer,
				strData,
				_timeoutCheckersForCamera[cameraIndex]);
			}

		}
        private VISION_RESULT IsSetLayerCompleted(int nLayerNo)
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandSetLayer, _cCommandType_Reply);
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if (_timeoutCheckersForLayer[nLayerNo].IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
            // return CheckCmdCompletedInBuffer(_cCommandSetLayer, _cCommandType_Reply);
        }
		private VISION_RESULT IsSetLayerCompletedForWafer(int nLayerNo)
		{
			VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandSetLayer, _cCommandType_Reply_ForWafer);
			if (VISION_RESULT.IN_PROCESS == result)
			{
				if (_timeoutCheckersForLayer[nLayerNo].IsTickOver(false))
				{
					return VISION_RESULT.TIMEOUT_VISION;
				}
			}
			return result;
		}

		// 2022.02.21 by junho [MOD] Pcb ID와 그냥 Set ID 분리
		//private bool RequestSetPCBID(int nRailID, string strPCBID)
        private bool RequestSetId(int nRailID, string strID)
        {
            if (false == _timeoutCheckersForLayer.ContainsKey(nRailID))
            {
                _timeoutCheckersForLayer.TryAdd(nRailID, new TickCounter_.TickCounter());
            }
			string strData = string.Format("{0};{1}", nRailID, strID);
            return SendMessage(_cProtocolTypeOfOperator,
                _cCommandSetID,
                _cCommandType_Send,
                strData,
                _timeoutCheckersForLayer[nRailID]);
        }
		// 2022.02.21 by junho [MOD] Pcb ID와 그냥 Set ID 분리
		//private VISION_RESULT IsSetPCBIdCompleted(int nRailID)
        private VISION_RESULT IsSetIdCompleted(int nRailID)
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandSetID, _cCommandType_Reply, string.Empty, false, nRailID);
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if (_timeoutCheckersForLayer[nRailID].IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
            // return CheckCmdCompletedInBuffer(_cCommandSetID, _cCommandType_Reply, string.Empty, false, nRailID);
        }

        private bool RequestSetServerInputData(int nIndexOfCam, params object [] param)
        {
            if (false == _timeoutCheckersForCamera.ContainsKey(nIndexOfCam))
            {
                _timeoutCheckersForCamera.TryAdd(nIndexOfCam, new TickCounter_.TickCounter());
            }
            string strData = string.Empty; // string.Format("{0};{1};{2}", nIndexOfCam, param.Length, )
            string strInputData = string.Empty;
            int nParamCount = param.Length;
            for (int i = 0; i < nParamCount; i++)
            {
                strInputData += param[i];
                if (i < param.Length - 1) strInputData += ";";
            }
            strData = string.Format("{0};{1};{2}", nIndexOfCam, nParamCount, strInputData);
    
            return SendMessage(_cProtocolTypeOfOperator,
                _cCommandSetServerInput,
                _cCommandType_Send,
                strData,
                _timeoutCheckersForCamera[nIndexOfCam]);
        }
        private VISION_RESULT IsSetServerInputDateCompleted(int nIndexOfCam)
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandSetServerInput, _cCommandType_Reply, nIndexOfCam.ToString());
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if (_timeoutCheckersForCamera[nIndexOfCam].IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
        }

        private bool RequestCheckDataExistInMap(int nMapID)
        {
            if (false == _timeoutCheckersForLayer.ContainsKey(nMapID))
            {
                _timeoutCheckersForLayer.TryAdd(nMapID, new TickCounter_.TickCounter());
            }
            return SendMessage(_cProtocolTypeOfOperator,
                _cCommandCheckDataExistInMap,
                _cCommandType_Send,
                nMapID.ToString(),
                _timeoutCheckersForLayer[nMapID]);
        }
        private VISION_RESULT IsDataExistInMap(int nMapID)
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandCheckDataExistInMap, _cCommandType_Reply, string.Empty, false, nMapID);
            if (VISION_RESULT.IN_PROCESS == result || VISION_RESULT.NOT_COMPLETE == result)
            {
                if (_timeoutCheckersForLayer[nMapID].IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
        }

        private bool RequestResultDataWrite(int nMapID)
        {
            if (false == _timeoutCheckersForLayer.ContainsKey(nMapID))
            {
                _timeoutCheckersForLayer.TryAdd(nMapID, new TickCounter_.TickCounter());
            }
            return SendMessage(_cProtocolTypeOfOperator,
                _cCommandGetInspectionResult,
                _cCommandType_Send,
                nMapID.ToString(),
                _timeoutCheckersForLayer[nMapID]);
            
        }
        private VISION_RESULT IsResultDataWriteAckReceived(int nMapID)
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandGetInspectionResult, _cCommandType_Reply);
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if (_timeoutCheckersForLayer[nMapID].IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
        }

        private bool RequestShowSelectView(int nViewID)
        {
            return SendMessage(_cProtocolTypeOfOperator,
                _cCommandShowSelectView,
                _cCommandType_Send,
                nViewID.ToString(),
                _timeoutCheckerForProgram);
        }
        private VISION_RESULT IsShowSelectViewAckReceived()
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandShowSelectView, _cCommandType_Reply);
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if (_timeoutCheckerForProgram.IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
        }

        private bool RequestSelectPCBResultData(string strPCBID)
        {
            return SendMessage(_cProtocolTypeOfOperator,
                _cCommandSelectPCBResultData,
                _cCommandType_Send,
                strPCBID,
                _timeoutCheckerForProgram);
            
        }
        private VISION_RESULT IsRequestSelectPCBResultDataAckReceived()
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandSelectPCBResultData, _cCommandType_Reply);
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if (_timeoutCheckerForProgram.IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
        }

        private bool RequestDeletePCBResultData(string strPCBID)
        {
            return SendMessage(_cProtocolTypeOfOperator,
                _cCommandDeletePCBResultData,
                _cCommandType_Send,
                strPCBID,
                _timeoutCheckerForProgram);
        }
        private VISION_RESULT IsRequestDeletePCBResultDataAckReceived()
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandDeletePCBResultData, _cCommandType_Receive);
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if (_timeoutCheckerForProgram.IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
        }

		private bool RequestSetPCBID(int nIndexOfCam, string strPCBID)
		{
			if (false == _timeoutCheckersForCamera.ContainsKey(nIndexOfCam))
			{
				_timeoutCheckersForCamera.TryAdd(nIndexOfCam, new TickCounter_.TickCounter());
			}
			string strData = string.Format("{0};{1}", nIndexOfCam, strPCBID);
			return SendMessage(_cProtocolTypeOfOperator,
                _cCommandSetPcbID,
				_cCommandType_Send,
				strData,
				_timeoutCheckersForCamera[nIndexOfCam]);
		}
		private VISION_RESULT IsSetPCBIdCompleted(int nIndexOfCam)
		{
			VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandSetPcbID, _cCommandType_Reply, string.Empty, false, nIndexOfCam.ToString());
			if (VISION_RESULT.IN_PROCESS == result)
			{
				if (_timeoutCheckersForCamera[nIndexOfCam].IsTickOver(false))
				{
					return VISION_RESULT.TIMEOUT_VISION;
				}
			}
			return result;
		}

        private bool RequestSetIid(int nIndexOfCam, double errorX, double errorY, int column, int row)
		{
			if (false == _timeoutCheckersForCamera.ContainsKey(nIndexOfCam))
			{
				_timeoutCheckersForCamera.TryAdd(nIndexOfCam, new TickCounter_.TickCounter());
			}
			string strData = string.Format("{0};{1:0.00},{2:0.00},{3},{4}", nIndexOfCam, errorX, errorY, column, row);
			return SendMessage(_cProtocolTypeOfOperator,
				_cCommandSetIID,
				_cCommandType_Send,
				strData,
				_timeoutCheckersForCamera[nIndexOfCam]);
		}
		private VISION_RESULT IsSetIidCompleted(int nIndexOfCam)
		{
			VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandSetIID, _cCommandType_Reply, string.Empty, false, nIndexOfCam.ToString());
			if (VISION_RESULT.IN_PROCESS == result)
			{
				if (_timeoutCheckersForCamera[nIndexOfCam].IsTickOver(false))
				{
					return VISION_RESULT.TIMEOUT_VISION;
				}
			}
			return result;
		}

		// 2023.01.26 by junho [ADD] klarf file용 정보 전달
		private bool RequestSetKlarfData(int nIndexOfCam, double subjectDiameter, string lotID, Vision_.DPointXY unitSize, string subjectID, int slotNo, int inspectionCount, Vision_.DPointXY leftTopToCenterDistance)
		{
            if(false == _timeoutCheckersForCamera.ContainsKey(nIndexOfCam))
            {
                _timeoutCheckersForCamera.TryAdd(nIndexOfCam, new TickCounter_.TickCounter());
			}
			string sendData = string.Format("{0};{1};{2},{3};{4};{5};{6};{7},{8}"
				, subjectDiameter												// 0
				, lotID															// 1
				, unitSize.x, unitSize.y										// 2, 3
				, subjectID													// 4
				, slotNo														// 5
				, inspectionCount												// 6
				, leftTopToCenterDistance.x, leftTopToCenterDistance.y		// 7, 8
				);
			return SendMessage(_cProtocolTypeOfOperator
				, _cCommandKlarfInspectionData
				, _cCommandType_Send
				, sendData
				, _timeoutCheckersForCamera[nIndexOfCam]);
		}
		private VISION_RESULT IsSetKlarfData(int nIndexOfCam)
		{
			VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandKlarfInspectionData, _cCommandType_Reply, string.Empty, false, nIndexOfCam.ToString());
			if(VISION_RESULT.IN_PROCESS == result)
			{
				if(_timeoutCheckersForCamera[nIndexOfCam].IsTickOver(false))
				{
					return VISION_RESULT.TIMEOUT_VISION;
				}
			}
			return result;
		}
        #endregion </검사결과관련설정>

        #region <검사>
        /// <summary> 트리거 모드 변경 요청 </summary>
        /// <param name="nIndexOfCam"></param>
        /// <param name="nMode">
        /// 0 : SOFTWARETRIGGER
        /// 1 : SOFTWAREGRAB 
        /// 2 : HARDWARETRIGGER
        /// 3 : SOFTWAREHARDWARETRIGGER 
        /// </param>
        /// <returns></returns>
        private bool RequestChangeTriggerMode(int nIndexOfCam, int nMode)
        {
            if (false == _timeoutCheckersForCamera.ContainsKey(nIndexOfCam))
            {
                _timeoutCheckersForCamera.TryAdd(nIndexOfCam, new TickCounter_.TickCounter());
            }
            string strData = string.Format("{0};{1}", nIndexOfCam, nMode);
            return SendMessage(_cProtocolTypeOfOperator,
                _cCommandSetTriggerMode,
                _cCommandType_Send,
                strData,
                _timeoutCheckersForCamera[nIndexOfCam]);
        }
        private VISION_RESULT IsTriggerModeChanged(int nIndexOfCam)
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandSetTriggerMode, _cCommandType_Reply, nIndexOfCam.ToString());
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if (_timeoutCheckersForCamera[nIndexOfCam].IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
        }

        private bool RequestChangeAlgorithm(int nIndexOfCam, int nIndexOfAlgorithm, int nIndexOfInspMode = 0)
        {
            if (false == _timeoutCheckersForCamera.ContainsKey(nIndexOfCam))
            {
                _timeoutCheckersForCamera.TryAdd(nIndexOfCam, new TickCounter_.TickCounter());
            }
            string strData = string.Format("{0};{1};{2}", nIndexOfCam, nIndexOfAlgorithm, nIndexOfInspMode);

            return SendMessage(_cProtocolTypeOfOperator,
                _cCommandSetCurrentInspectionMode,
                _cCommandType_Send,
                strData,
                _timeoutCheckersForCamera[nIndexOfCam]);
        }
        private VISION_RESULT IsAlgorithmChanged(int nIndexOfCam)
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandSetCurrentInspectionMode, _cCommandType_Reply, nIndexOfCam.ToString());
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if (_timeoutCheckersForCamera[nIndexOfCam].IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
        }
        private bool RequestGrab(int nIndexOfCam, int nShotMode = 0, params object[] param)
        {
            if (false == _timeoutCheckersForCamera.ContainsKey(nIndexOfCam))
            {
                _timeoutCheckersForCamera.TryAdd(nIndexOfCam, new TickCounter_.TickCounter());
            }
            string strData = string.Empty; //string.Format("{0};{1}", nIndexOfCam, nShotMode);

            if(param == null || param.Length == 0)
            {
                strData = string.Format("{0};{1};0", nIndexOfCam, nShotMode);
            }
            else
            {
                switch (nShotMode)
                {
                    case 0:
                    case 1:
                        strData = string.Format("{0};{1};{2}", nIndexOfCam, nShotMode, param[0]);
                        break;
                    case 2:
                    case 3:
                        foreach(var data in param)
                        {
                            strData += string.Format(";{0}", data);
                        }
                        break;
                }
            }
            return SendMessage(_cProtocolTypeOfOperator,
                    _cCommandSetGrabAndInspection,
                    _cCommandType_Send,
                    strData,
                    _timeoutCheckersForCamera[nIndexOfCam]);
        }
        /// <summary>
        /// 2021.05.11. by shkim. [ADD] 그랩 요청에 대한 ACK
        /// </summary>
        /// <param name="nIndexOfCam"></param>
        /// <returns></returns>
        private VISION_RESULT IsRequestGrabComplete(int nIndexOfCam)
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandSetGrabAndInspection, _cCommandType_Reply, nIndexOfCam.ToString());
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if (_timeoutCheckersForCamera[nIndexOfCam].IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
        }
        /// <summary>
        /// 2021.05.11. by shkim. [ADD] 그랩 완료에 대한 신호
        /// </summary>
        /// <param name="nIndexOfCam"></param>
        /// <returns></returns>
        private VISION_RESULT IsGrabEndReceived(int nIndexOfCam)
        {
            VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandSetGrabEnd, _cCommandType_Send, nIndexOfCam.ToString());
            if (VISION_RESULT.IN_PROCESS == result)
            {
                if (_timeoutCheckersForCamera[nIndexOfCam].IsTickOver(false))
                {
                    return VISION_RESULT.TIMEOUT_VISION;
                }
            }
            return result;
        }

		// 2021.08.30 by junho [DEL] 별도로 외부에서 받아야 할 필요 있어서 외부함수로 이동 (BP5000WIR)
		//private VISION_RESULT IsInspectionEnd(int nIndexOfCam)
		//{
		//	VISION_RESULT result = CheckCmdCompletedInBuffer(_cCommandSetGrabAndInspection, _cCommandType_Receive, nIndexOfCam.ToString(), true);
		//	if (VISION_RESULT.IN_PROCESS == result)
		//	{
		//		if (_timeoutCheckersForCamera[nIndexOfCam].IsTickOver(false))
		//		{
		//			return VISION_RESULT.TIMEOUT_VISION;
		//		}
		//	}
		//	return result;
		//}
        #endregion </검사>

        #region <통신>
        private VISION_RESULT CheckCmdCompletedInBuffer(string strCmd, string strCmdType, string strCamIndex = "", bool bIsInspectionEnd = false, params object[] compareValues)
        {
            string strTargetCmd = string.Format("{0}{1}{2}", strCmd, strCmdType, strCamIndex);
            
            string strReturnMsg = string.Empty;

            if (_bufferMessagesForParsing.TryRemove(strTargetCmd, out strReturnMsg))
            {
                return ParseReturnMsg(strCmd, strReturnMsg, bIsInspectionEnd, compareValues);
            }
//             else if(m_TickCounterForReceiveTimeout.IsTickOver(false))
//             {
//                 return VISION_RESULT.TIMEOUT_VISION;
//             }
            return VISION_RESULT.IN_PROCESS;
        }
        private VISION_RESULT ParseReturnMsg(string strCmd, string strReturnMsg, bool bIsInspectionEnd = false, params object[] compareValues)
        {
            string[] strArrSplitData = strReturnMsg.Split(';');

            if (bIsInspectionEnd)
            {
                return ParseInspectionResult(strArrSplitData);
            }
            int cameraNumber = 0;
            switch(strCmd)
            {
                case _cCommandVersion:    // 버전 요청
                    if (strArrSplitData.Length == 2)
                    {
                        _additinalMessageFromVision = strArrSplitData[1];
                        return VISION_RESULT.COMPLETE;
                    }
                    else return VISION_RESULT.ERROR_VISION;
                    
                case _cCommandTurnOffVisionProgram:           // 프로세스 종료 요청
                case _cCommandSetShowMode:         // SHOW 모드 변경 요청 (RUN OR SETUP)
                case _cCommandSetTopWindow:      // TOP MOST 모드 변경 요청
                case _cCommandSetWindowVisible:         // SHOW/HIDE 변경 요청

                case _cCommandResetResult:  // 검사결과 초기화 요청
                case _cCommandSetLayer:               // 검사 LAYER 설정 요청
                case _cCommandGetInspectionResult:        // 검사 결과 파일 쓰기 요청
                case _cCommandDeleteRecipe:           // 레시피 삭제 요청
                case _cCommandRecipeNamingRequest:       // 레시피 이름 변경 요청
                case _cCommandShowSelectView:         // 비전 결과 확인 VIEW 선택 요청
                case _cCommandSelectPCBResultData:    // 선택된 PCB ID의 결과 DATA 요청
                case _cCommandDeletePCBResultData:    // 선택된 PCB ID 결과 DATA 삭제 요청
				case _cCommandKlarfInspectionData:		// Klarf file data 전달 ack
                    if (strArrSplitData.Length == 1)
                    {
                        if (strArrSplitData[0].Equals("1")) return VISION_RESULT.COMPLETE;
                        else return VISION_RESULT.ERROR_VISION;
                    }
                    return VISION_RESULT.ERROR_VISION;

                case _cCommandSetTriggerMode:          // 트리거 모드 변경 요청
                case _cCommandStartLive:             // 라이브 모드 변경 요청
                case _cCommandStartMachineCalibration:   // RESOLUTION CALIBRATION 실행 요청
                case _cCommandCopyRecipeAll:            // 레시피 복사 요청
                case _cCommandSetCurrentInspectionMode:            // 알고리즘 변경 요청
                case _cCommandResetInspectionMap:           // 검사, 맵 초기화 요청
                case _cCommandSetGrabAndInspection:                       // 그랩 요청
                case _cCommandSetServerInput:             // 서버 데이터 설정 요청
                case _cCommandChangeCalibrationMode:      // 멀티캘리브레이션 모드 변경 요청
                case _cCommandStartMultiCalibrationExcute:     // 멀티캘리브레이션 모드 실행 요청
                    if (strArrSplitData.Length == 2)
                    {
                        if (strArrSplitData[0].Equals("1")) return VISION_RESULT.COMPLETE;
                        else return VISION_RESULT.ERROR_VISION;
                    }
                    return VISION_RESULT.ERROR_VISION;

                case _cCommandSetGrabEnd:           // 그랩 완료 신호
                    return VISION_RESULT.COMPLETE;

                case _cCommandCheckDataExistInMap:               // 맵 데이터 완료 확인 요청
                    if (strArrSplitData.Length == 3)
                    {
                        //if (strArrSplitData[0].Equals("1") && strArrSplitData[1].Equals(compareValues[0]))
                        if (strArrSplitData[0].Equals("1") && strArrSplitData[1].Equals(compareValues[0].ToString()))
                        {
                            if (strArrSplitData[2].Equals("1")) return VISION_RESULT.COMPLETE;
                            else return VISION_RESULT.NOT_COMPLETE;
                            // else return VISION_RESULT.IN_PROCESS;
                        }
                        else return VISION_RESULT.ERROR_VISION;
                    }
                    return VISION_RESULT.ERROR_VISION;

				case _cCommandSetID:   // ID 설정 요청
				case _cCommandSetPcbID:   // PCB ID 설정 요청
				case _cCommandSetIID:   // IID 설정 요청
                    if (strArrSplitData.Length == 2)
                    {
                        if (strArrSplitData[0].Equals("1") && strArrSplitData[1].Equals(compareValues[0].ToString()))  
                        {
                            return VISION_RESULT.COMPLETE;
                        }
                        else return VISION_RESULT.ERROR_VISION;
                    }
                    else return VISION_RESULT.ERROR_VISION;

                case _cCommandGetCurrentRecipe:  // 현재 레시피 정보 요청
                    if (strArrSplitData.Length == 3)
                    {
                        if (strArrSplitData[0].Equals("1"))
                        {
                            _additinalMessageFromVision = strArrSplitData[2];
                            if (false == int.TryParse(strArrSplitData[1], out cameraNumber))
                            {
                                // 카메라 번호 확인이 안되면 에러 발생
                                return VISION_RESULT.ERROR_VISION;
                            }
                            return VISION_RESULT.COMPLETE;
                        }
                        else return VISION_RESULT.ERROR_VISION;
                    }
                    return VISION_RESULT.ERROR_VISION;

				// 2021.12.02 by junho [MOD] Recipe 전체 변경 대응
				case _cCommandSetCurrentRecipe:               // 레시피 변경 요청
					if (strArrSplitData.Length == 2 || strArrSplitData.Length == 1)
                    {
                        if (strArrSplitData[0].Equals("1")) return VISION_RESULT.COMPLETE;
                        else return VISION_RESULT.ERROR_VISION;
                    }
                    return VISION_RESULT.ERROR_VISION;

                default:
                    return VISION_RESULT.ERROR_COMMAND;
            }
        }

        /// <summary>
        /// 검사 결과 타입 별로 파싱한다.
        /// </summary>
        /// <returns></returns>
        private VISION_RESULT ParseInspectionResult(string[] splitedDatas)
        {
            int cameraNumber = 0;

			// 2023.01.05 by junho [ADD] splitedDatas가 안맞는 경우의 예외처리 개선
			if (splitedDatas == null || splitedDatas.Length < 1 || false == int.TryParse(splitedDatas[0], out cameraNumber))
            {
                // 카메라 번호 확인이 안되면 에러 발생
                return VISION_RESULT.ERROR_VISION;
            }
            if (_dicDelParsingResultFromInspectionData == null) return VISION_RESULT.ERROR_VISION;

            ConcurrentDictionary<int, DelParsingInspectionReturnData> delParsingResultForCam;
            if (_dicDelParsingResultFromInspectionData.TryGetValue(cameraNumber, out delParsingResultForCam))
            {
                DelParsingInspectionReturnData parsingInspectionRetrunData;
                if(delParsingResultForCam.TryGetValue(_currentResultType[cameraNumber], out parsingInspectionRetrunData))
                {
                    return parsingInspectionRetrunData(splitedDatas, ref _visionResults[cameraNumber]);
                }
            }
            return VISION_RESULT.ERROR_VISION;
        }

        private bool IsConnected()
        {
            if (_instanceOfSocket == null) return false;

            SOCKET_ITEM_STATE state = _instanceOfSocket.GetState(_cSocketIndex);

            bool bReturn = true;
            bReturn &= state != SOCKET_ITEM_STATE.DISCONNECTED;
            bReturn &= state != SOCKET_ITEM_STATE.READY;
            bReturn &= state != SOCKET_ITEM_STATE.DISABLED;
            bReturn &= state != SOCKET_ITEM_STATE.WAITING_CONNECTION;

            /*return state == SOCKET_ITEM_STATE.CONNECTED;*/
            // return (m_TickCounterForReceiveTimeout.IsTickOver(false));
            return bReturn;
        }
        private bool SendMessage(string strProtocalType, string strCmd, string strCmdType, string strData, TickCounter_.TickCounter _timeoutChecker)
        {
			_timeoutChecker.SetTickCount(_nReceiveTimeout);

            string message = string.Format("{0}{1}{2}{3}{4:0000}{5}{6}",
                _valueOfSTX,
                strProtocalType,
                strCmd,
                strCmdType,
                strData.Length,
                strData,
                _valueOfETX);

            return _instanceOfSocket.Send(_cSocketIndex, message);
        }

        /// <summary>
        /// 2018.07.25 by yjlee [ADD] 메시지를 토큰으로 파싱한다.
        /// 2020.06.12. by shkim. [MOD] 메시지 파싱 안되는 경우 수정
        /// </summary>
        private void ParseMessageByToken(string strMessage)
        {
            string strStartingToken = null;
            string strEndingToken = null;

            strStartingToken = _valueOfSTX;//chStartingToken.ToString();
            strEndingToken = _valueOfETX;//chEndingToken.ToString();

            int nStartingIndex = strMessage.IndexOf(strStartingToken);
            int nEndingIndex = strMessage.IndexOf(strEndingToken);


            int nReadByte;
            string strFullMessage;

            // 2020.06.15. by shkim [CHECKS] 통신문제 파악중
            // LogWriter.GetInstance().MakeLog("Communication", strMessage);

            // 시작 토큰 인덱스와 종료 토큰 인덱스가 존재하고 같을 경우
            if (nStartingIndex != -1
                && nStartingIndex == nEndingIndex)
            {
                // strFullMessage = m_dicOfTempMessage[nIndex].ToString();
                strFullMessage = _splitMessageBuffer.ToString();

                // 메시지 버퍼가 비어있을 경우 ( 시작 메시지가 없을 경우 )
                if (String.IsNullOrEmpty(strFullMessage))
                {
                    nEndingIndex = -1;
                }
                // 메시지 버퍼에 메시지가 있을 경우
                else
                {
                    nStartingIndex = -1;
                }
            }
            // 시작 토큰이 종료 토큰보다 뒤에 있을 경우
            else if (nEndingIndex != -1
                && nStartingIndex > nEndingIndex)
            {
                //nReadByte = sItem.bHexadecimal ? 2 : 1;
                nReadByte = 1;


                strFullMessage = _splitMessageBuffer.ToString();

                // 메시지 버퍼가 비어있지 않을 경우(시작 메시지가 존재할 경우)
                if (false == String.IsNullOrEmpty(strFullMessage))
                {
                    _splitMessageBuffer.Append(strMessage.Substring(0, nEndingIndex + nReadByte));

                    strFullMessage = _splitMessageBuffer.ToString();

                    //Byte[] arFullMessage = null;
                    //arFullMessage = Encoding.UTF8.GetBytes(strFullMessage);

                    _splitMessageBuffer.Clear();

                    _receivedMessages.Enqueue(strFullMessage);

                    // AddReceiveMessage(nIndex, strSenderIP, arFullMessage, arFullMessage.Length, sItem.bWriteLog);
                }
            }

            // 토큰이 둘 다 없을 경우
            if (nStartingIndex == -1 && nEndingIndex == -1)
            {
                strFullMessage = _splitMessageBuffer.ToString();

                // 메시지 버퍼가 비어있지 않을 경우 ( 시작 메시지가 존재할 경우 )
                if (false == String.IsNullOrEmpty(strFullMessage))
                {
                    _splitMessageBuffer.Append(strMessage);
                }
            }
            // 시작 토큰만 없을 경우
            else if (nStartingIndex == -1)
            {
                //nReadByte = sItem.bHexadecimal ? 2 : 1;
                nReadByte = 1;

                strFullMessage = _splitMessageBuffer.ToString();

                // 메시지 버퍼가 비어있지 않을 경우(시작 메시지가 존재할 경우)
                if (false == String.IsNullOrEmpty(strFullMessage))
                {
                    _splitMessageBuffer.Append(strMessage.Substring(0, nEndingIndex + nReadByte));

                    strFullMessage = _splitMessageBuffer.ToString();

                    //                     Byte[] arFullMessage = null;
                    // 
                    //                     arFullMessage = Encoding.UTF8.GetBytes(strFullMessage);

                    _splitMessageBuffer.Clear();

                    _receivedMessages.Enqueue(strFullMessage);

                    // AddReceiveMessage(nIndex, strSenderIP, arFullMessage, arFullMessage.Length, sItem.bWriteLog);
                }
            }
            // 종료 토큰만 없을 경우
            else if (nEndingIndex == -1)
            {
                _splitMessageBuffer.Append(strMessage.Substring(nStartingIndex));
            }
            // 토큰이 둘 다 있을 경우
            else
            {
                // nReadByte = sItem.bHexadecimal ? 2 : 1;
                nReadByte = 1;

                // string strFullMessage   = strMessage.Substring(nStartingIndex, nEndingIndex - nStartingIndex + nReadByte);
                strFullMessage = strMessage;
                while (nStartingIndex != -1)
                {
                    string strTempMsg = strFullMessage.Substring(nStartingIndex);
                    string strTempFullMessage = String.Empty;
                    if (false == strTempMsg.Contains(strEndingToken))
                    {
                        _splitMessageBuffer.Append(strMessage.Substring(nStartingIndex));
                        break;
                    }
                    else
                    {
                        nEndingIndex = strFullMessage.IndexOf(strEndingToken, nStartingIndex);

                        strTempFullMessage = strFullMessage.Substring(nStartingIndex, nEndingIndex - nStartingIndex + nReadByte);

                        Byte[] arFullMessage = null;

                        arFullMessage = Encoding.UTF8.GetBytes(strTempFullMessage);

                        // 2021.08.13 wdw [fix] strFullMessage -> strTempFullMessage 변경
                        _receivedMessages.Enqueue(strTempFullMessage);

                        // AddReceiveMessage(nIndex, strSenderIP, arFullMessage, arFullMessage.Length, sItem.bWriteLog);

                        nStartingIndex = strFullMessage.IndexOf(strStartingToken, nEndingIndex + 1);
                    }
                }
            }
        }
		private void ResetTimeCheckerByManual(TickCounter_.TickCounter _timeoutChecker)
		{
			_timeoutChecker.SetTickCount(_nReceiveTimeout);
		}
        #endregion </통신>
    }
}
