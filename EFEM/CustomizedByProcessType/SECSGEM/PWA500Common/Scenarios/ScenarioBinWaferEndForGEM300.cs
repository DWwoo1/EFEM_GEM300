using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using FrameOfSystem3.SECSGEM.DefineSecsGem;
using FrameOfSystem3.SECSGEM.Scenario;

namespace EFEM.CustomizedByProcessType.PWA500Common
{
    public class ScenarioBinWaferEndForGEM300ParamValues : ScenarioParamValues
    {
        public ScenarioBinWaferEndForGEM300ParamValues(List<string> values, bool useEventHandling/*, List<string> sortingInfo*/, string sortingInfo) : base(values)
        {
            UseEventHandling = useEventHandling;
            SortingInfo = sortingInfo;
        }

        public readonly bool UseEventHandling;
        public readonly string SortingInfo;
    }

    public class ScenarioBinWaferEndForGEM300 : AutoScenarioBase
    {
        #region <Constructors>
        public ScenarioBinWaferEndForGEM300(string name, long eventId, List<long> variables,
            long vidSortingInfo,
            uint timeOut = 10000)
            : base(name, timeOut)
        {
            _receiveMessageFormat = new List<SemiObject>();
            MessageFormatToSend = new List<SemiObject>();

            _eventId = eventId;
            _variables = new List<long>(variables);

            VidSortingInfo = vidSortingInfo;
        }
        #endregion </Constructors>

        #region <Fields>
        private ScenarioBinWaferEndForGEM300ParamValues _paramValue = null;
        private List<SemiObject> _receiveMessageFormat = null;
        private readonly List<SemiObject> MessageFormatToSend = null;

        private readonly long VidSortingInfo;

        private long _eventId;
        private List<long> _variables;

        private readonly int _indexSortingInfoLotId = 0;
        private readonly int _indexSortingInfoWaferId = 1;
        private readonly int _indexSortingInfoSplittedQty = 2;
        #endregion </Fields>

        #region <Types>
        enum ScenarioSeq
        {
            INIT = 0,
            UPDATE_BINARY_VARIABLE = 400,
            SEND_EVENT = 500,

            FINISH = 1000,
        }
        #endregion </Types>

        #region <Properties>
        public long EventId
        {
            get
            {
                return _eventId;
            }
        }
        #endregion </Properties>

        #region <Methods>
        public override EN_SCENARIO_RESULT ExecuteScenario()
        {
            switch (_seqNum)
            {
                case (int)ScenarioSeq.INIT:
                    {
                        //Activate = true;
                        //InitFlags();
                        Receiving = true;

                        _seqNum = (int)ScenarioSeq.UPDATE_BINARY_VARIABLE;
                    }
                    break;

                case (int)ScenarioSeq.UPDATE_BINARY_VARIABLE:
                    {
                        string sortingInfo = _paramValue.SortingInfo;
                        int sortingInfoCount;

                        List<SemiObject> objectSortingInfo = new List<SemiObject>();
                        objectSortingInfo.Add(new SemiObjectList(2));
                        objectSortingInfo.Add(new SemiObjectAscii("SORTING_INFO", "SORTING_INFO"));
                        if (string.IsNullOrEmpty(sortingInfo))
                        {
                            objectSortingInfo.Add(new SemiObjectList(0));
                        }
                        //else
                        //{
                        //    if (sortingInfo.Contains(','))
                        //    {
                        //        string[] arraySortingInfo = sortingInfo.Split(',');
                        //        int arrayCount = arraySortingInfo.Length;
                        //        objectSortingInfo.Add(new SemiObjectList(arrayCount));

                        //        foreach (var item in arraySortingInfo)
                        //        {
                        //            objectSortingInfo.Add(new SemiObjectAscii("SORTING_INFO", item));
                        //        }
                        //    }
                        //    else
                        //    {
                        //        objectSortingInfo.Add(new SemiObjectList(1));
                        //        objectSortingInfo.Add(new SemiObjectAscii("SORTING_INFO", sortingInfo));
                        //    }
                        //}
                        else
                        {
                            string[] arraySortingInfo = sortingInfo.Split(',');
                            int arrayCount = arraySortingInfo.Length;
                            objectSortingInfo.Add(new SemiObjectList(arrayCount));

                            foreach (var item in arraySortingInfo)
                            {
                                string[] arraySortingInfoValue = item.Split(':');

                                objectSortingInfo.Add(new SemiObjectList(2));
                                objectSortingInfo.Add(new SemiObjectAscii("WAFER_ID", arraySortingInfoValue[_indexSortingInfoWaferId]));
                                objectSortingInfo.Add(new SemiObjectAscii("WAFER_ID", arraySortingInfoValue[_indexSortingInfoSplittedQty]));
                            }
                        }

                        _gemHandler.UpdateVariable(VidSortingInfo, objectSortingInfo);

                        SetTickCount(100);

                        _seqNum = (int)ScenarioSeq.SEND_EVENT;
                    }
                    break;

                case (int)ScenarioSeq.SEND_EVENT:
                    {
                        if (false == IsTickOver(false))
                            break;

                        List<long> variableIdsToUpdate = new List<long>();
                        List<string> variablesToUpdate = new List<string>();

                        // Sorting Info는 위에서 업데이트 했기 때문에 제외한다.
                        for (int i = 0; i < _variables.Count; ++i)
                        {
                            if (_variables[i] == VidSortingInfo)
                                continue;

                            variableIdsToUpdate.Add(_variables[i]);
                            variablesToUpdate.Add(_paramValue.VariableValues[i]);
                        }

                        _gemHandler.SendEvent(_eventId, variableIdsToUpdate.ToArray(), variablesToUpdate.ToArray());

                        SetTickCount(TimeOut);
                        ++_seqNum;
                    }
                    break;

                case (int)ScenarioSeq.SEND_EVENT + 1:
                    {
                        if (IsTickOver(true))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                        }

                        if (false == _gemHandler.IsSendingEventCompleted(_eventId))
                            break;

                        SetTickCount(TimeOut);
                        ++_seqNum;
                    }
                    break;

                case (int)ScenarioSeq.SEND_EVENT + 2:
                    {
                        if (IsTickOver(true))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                        }

                        return ReturnScenarioResult(EN_SCENARIO_RESULT.COMPLETED);
                    }
                    break;

                default:
                    return EN_SCENARIO_RESULT.ERROR;
            }

            return EN_SCENARIO_RESULT.PROCEED;
        }

        public override void UpdateParamValues(ScenarioParamValues paramValues)
        {
            _paramValue = paramValues as ScenarioBinWaferEndForGEM300ParamValues;
        }
        #endregion </Methods>
    }
}
