using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace FrameOfSystem3.SECSGEM.Scenario.Common
{
    public class SendingEventParamValuesEx : ScenarioParamValues
    {
        public SendingEventParamValuesEx(List<string> values, List<SemiObject> semiObjects)
        {
            if (values != null)
            {
                _variableValues = new string[values.Count];

                Array.Copy(values.ToArray(), _variableValues, values.Count);
            }

            if (semiObjects != null)
            {
                _variableSemiObjects = new List<SemiObject>();

                for (int i = 0; i < semiObjects.Count; ++i)
                {
                    _variableSemiObjects.Add(semiObjects[i]);
                }
            }
        }

        public List<SemiObject> VariableSemiObjects
        {
            get
            {
                return _variableSemiObjects;
            }
        }

        private List<SemiObject> _variableSemiObjects = null;
    }

    public class SendingEventScenarioEx : ScenarioBaseClass
    {
        #region <Constructor>
        public SendingEventScenarioEx(string name, long eventId,
            bool usePermission,
            List<long> variableIds,
            long variableIdTypeSemiObjects,
            uint timeOut = 5000)
            : base(name, timeOut)
        {
            _eventId = eventId;
            _usePermission = usePermission;
            if (variableIds != null)
            {
                _statusVariableIds = new long[variableIds.Count];

                for (int i = 0; i < variableIds.Count; ++i)
                {
                    _statusVariableIds[i] = variableIds[i];
                }
            }

            _statusVariableIdTypeSemiObjects = variableIdTypeSemiObjects;
        }
        #endregion </Constructor>

        #region <Fields>
        private long _eventId;
        private long[] _statusVariableIds = null;
        private long _statusVariableIdTypeSemiObjects;
        private bool _usePermission = false;
        SendingEventParamValuesEx _paramValues = null;
        #endregion </Fields>

        #region <Properties>
        // 2023.09.04. jhlim [DEL] 추후 정리 혹은 재개발
        //public RemoteCommandInfo RemoteCommandInfoNormal
        //{
        //    get
        //    {
        //        return _remoteCommdInfoNormal;
        //    }
        //}

        //public RemoteCommandInfo RemoteCommandInfoNg
        //{
        //    get
        //    {
        //        return _remoteCommdInfoNg;
        //    }
        //}
        #endregion </Properties>

        #region <Type>
        private enum EN_SCENARIO_SEQ
        {
            INIT = 0,
            UPDATE_SEMIOBJECT_VARIABLES = 100,
            SEND_MESSAGE = 200,
            WAIT_FOR_PERMISSION = 300,
            FINISH,
        }
        #endregion </Type>

        #region <Methods>
        public override void UpdateParamValues(ScenarioParamValues paramValues)
        {
            _paramValues = paramValues as SendingEventParamValuesEx;
        }

        public override EN_SCENARIO_RESULT ExecuteScenario()
        {
            switch (_seqNum)
            {
                case (int)EN_SCENARIO_SEQ.INIT:
                    Activate = true;
                    InitFlags();
                    _seqNum = (int)EN_SCENARIO_SEQ.UPDATE_SEMIOBJECT_VARIABLES;
                    break;
                
                case (int)EN_SCENARIO_SEQ.UPDATE_SEMIOBJECT_VARIABLES:
                    if (_paramValues != null && _paramValues.VariableSemiObjects != null)
                    {
                        _gemHandler.UpdateVariable(_statusVariableIdTypeSemiObjects, _paramValues.VariableSemiObjects);
                        SetTickCount(100);
                    }
                    ++_seqNum; break;
                case (int)EN_SCENARIO_SEQ.UPDATE_SEMIOBJECT_VARIABLES + 1:
                    if (false == IsTickOver(true))
                        break;
                    
                    _seqNum = (int)EN_SCENARIO_SEQ.SEND_MESSAGE;
                    break;

                case (int)EN_SCENARIO_SEQ.SEND_MESSAGE:
                    if (_paramValues != null && _paramValues.VariableValues != null)
                    {
                        _gemHandler.SendEvent(_eventId, _statusVariableIds, _paramValues.VariableValues);
                    }
                    else
                    {
                        _gemHandler.SendEvent(_eventId, null, null);
                    }
                    SetTickCount(TimeOut);
                    ++_seqNum; break;

                case (int)EN_SCENARIO_SEQ.SEND_MESSAGE + 1:
                    if (IsTickOver(true))
                    {
                        return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                    }

                    if (false == _gemHandler.IsSendingEventCompleted(_eventId))
                        break;

                    if (false == _usePermission)
                    {
                        return ReturnScenarioResult(EN_SCENARIO_RESULT.COMPLETED);
                    }
                    else
                    {
                        SetTickCount(TimeOut);
                        _seqNum = (int)EN_SCENARIO_SEQ.WAIT_FOR_PERMISSION;
                    }
                    break;

                case (int)EN_SCENARIO_SEQ.WAIT_FOR_PERMISSION:
                    if (IsTickOver(true))
                    {
                        return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                    }

                    switch (Permission)
                    {
                        case EN_SCENARIO_PERMISSION_RESULT.OK:
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.COMPLETED);
                        case EN_SCENARIO_PERMISSION_RESULT.ERROR:
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                        default:
                            break;
                    }
                    break;

                default:
                    return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
            }

            return EN_SCENARIO_RESULT.PROCEED;
        }
        #endregion </Methods>
    }
}
