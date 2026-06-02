using System;
using System.Collections.Generic;

using FrameOfSystem3.SECSGEM.DefineSecsGem;
using FrameOfSystem3.SECSGEM.SecsGemSDK.Gem300;

using EFEM.Defines.Job;

using XGEM300PRO.Library;

namespace FrameOfSystem3.SECSGEM
{
    internal sealed class XGemControlJobDriver : IControlJobDriver
    {
        private readonly XGem300ProW _driver;

        public XGemControlJobDriver(XGem300ProW driver)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
            SubscribeDriverEvents();
        }

        private sealed class ControlJobAttributeInfo
        {
            public ControlJobState State { get; set; }

            public ControlJobStartMode StartMode { get; set; }

            public string[] CurrentProcessJobIds { get; set; }

            public string DataCollectionPlan { get; set; }

            public string[] CarrierInputIds { get; set; }

            public ControlJobMaterialOutputSpec[] MaterialOutputSpecifications { get; set; }

            public ControlJobMaterialOutputByStatus[] MaterialOutputByStatus { get; set; }

            public uint[] PauseEventIds { get; set; }

            public ControlJobProcessJobStatusInfo[] ProcessJobStatus { get; set; }

            public ControlJobProcessingControlSpec[] ProcessingControlSpecifications { get; set; }

            public MaterialOrderMode ProcessOrderManagement { get; set; }
        }

        public event EventHandler<ControlJobCreatedEventArgs> ControlJobCreated;
        public event EventHandler<ControlJobStateChangedEventArgs> ControlJobStateChanged;
        public event EventHandler<ControlJobDeletedEventArgs> ControlJobDeleted;
        public event EventHandler<ControlJobVerifyRequestedEventArgs> ControlJobVerifyRequestedByHost;
        public event EventHandler<ControlJobCommandRequestedEventArgs> ControlJobCommandRequestedByHost;
        public event EventHandler<ControlJobManualStartEventArgs> ControlJobManualStartRequired;
        public event EventHandler<ControlJobHoqChangedEventArgs> ControlJobHeadOfQueueChanged;

        public long Create(string controlJobId, ControlJobStartMode startMode, string[] processJobIds)
        {
            string[] safeIds = processJobIds ?? new string[0];
            return _driver.CJReqCreate(controlJobId ?? string.Empty, (long)startMode, safeIds.Length, safeIds);
        }

        public long RequestJob(string controlJobId)
        {
            return _driver.CJReqGetJob(controlJobId ?? string.Empty);
        }

        public long RequestAllJobIds()
        {
            return _driver.CJReqGetAllJobID();
        }

        public long RequestSelect(string controlJobId)
        {
            return _driver.CJReqSelect(controlJobId ?? string.Empty);
        }

        public long RequestHeadOfQueue(string controlJobId)
        {
            return _driver.CJReqHOQJob(controlJobId ?? string.Empty);
        }

        public long RequestHeadOfQueueInfo()
        {
            return _driver.CJGetHOQJob();
        }

        public long RequestCommand(string controlJobId, ControlJobCommand command, string commandParameterName, string commandParameterValue)
        {
            return _driver.CJReqCommand(controlJobId ?? string.Empty, (long)command, commandParameterName ?? string.Empty, commandParameterValue ?? string.Empty);
        }

        public long AcknowledgeVerify(long messageId, string controlJobId, long result, long[] errorCodes, string[] errorTexts)
        {
            long[] safeCodes = errorCodes ?? new long[0];
            string[] safeTexts = errorTexts ?? new string[0];

            EnsureEqualLength(safeCodes.Length, safeTexts.Length, nameof(errorCodes), nameof(errorTexts));

            return _driver.CJRspVerify(messageId, controlJobId ?? string.Empty, result, safeCodes.Length, safeCodes, safeTexts);
        }

        public long AcknowledgeCommand(long messageId, string controlJobId, ControlJobCommand command, long result, long[] errorCodes, string[] errorTexts)
        {
            long firstErrorCode = errorCodes != null && errorCodes.Length > 0 ? errorCodes[0] : 0L;
            string firstErrorText = errorTexts != null && errorTexts.Length > 0 ? errorTexts[0] : string.Empty;

            return _driver.CJRspCommand(messageId, controlJobId ?? string.Empty, (long)command, result, firstErrorCode, firstErrorText);
        }

        public long SetJobInfo(string controlJobId, ControlJobState state, ControlJobStartMode startMode, string[] processJobIds)
        {
            string[] safeIds = processJobIds ?? new string[0];
            return _driver.CJSetJobInfo(controlJobId ?? string.Empty, (long)state, (long)startMode, safeIds.Length, safeIds);
        }

        public long Remove(string controlJobId)
        {
            return _driver.CJDelJobInfo(controlJobId ?? string.Empty);
        }

        public long RemoveAll()
        {
            return _driver.CJDelAllJobInfo();
        }

        private void SubscribeDriverEvents()
        {
            _driver.OnCJCreated += HandleCreated;
            _driver.OnCJStateChanged += HandleStateChanged;
            _driver.OnCJDeleted += HandleDeleted;
            _driver.OnCJReqVerify += HandleVerifyRequested;
            _driver.OnCJReqCommand += HandleCommandRequested;
            _driver.OnCJManualStartDisplay += HandleManualStartRequired;
            _driver.OnCJHOQJobChanged += HandleHeadOfQueueChanged;
        }

        private void HandleCreated(
            string controlJobId,
            long startMode,
            long processJobCount,
            string[] processJobIds)
        {
            var callbackInfo = new ControlJobInfo(
                controlJobId,
                (ControlJobStartMode)startMode,
                Slice(processJobIds, 0, (int)processJobCount));

            var attributeInfo = ReadControlJobAttributesOrDefault(controlJobId);

            ControlJobInfo eventInfo;

            if (attributeInfo == null)
            {
                eventInfo = callbackInfo;
            }
            else
            {
                eventInfo = new ControlJobInfo(
                    controlJobId,
                    attributeInfo.State,
                    attributeInfo.StartMode,
                    callbackInfo.ProcessJobIds,
                    attributeInfo.CurrentProcessJobIds,
                    attributeInfo.DataCollectionPlan,
                    attributeInfo.CarrierInputIds,
                    attributeInfo.MaterialOutputSpecifications,
                    attributeInfo.MaterialOutputByStatus,
                    attributeInfo.PauseEventIds,
                    attributeInfo.ProcessJobStatus,
                    attributeInfo.ProcessingControlSpecifications,
                    attributeInfo.ProcessOrderManagement);
            }

            // 여기서 생성됨
            // 1

            ControlJobCreated?.Invoke(
                this,
                new ControlJobCreatedEventArgs(eventInfo));
        }
        private void HandleStateChanged(string controlJobId, long state)
        {
            ControlJobStateChanged?.Invoke(this, new ControlJobStateChangedEventArgs(controlJobId, (ControlJobState)state));

            // 2

            // 셀렉트 이후 1), 3)
        }

        private void HandleDeleted(string controlJobId)
        {
            ControlJobDeleted?.Invoke(this, new ControlJobDeletedEventArgs(controlJobId));
        }

        private void HandleVerifyRequested(
            long messageId,
            string controlJobId,
            long carrierCount,
            string[] carrierIds,
            long processJobCount,
            string[] processJobIds,
            long processOrderManagement,
            long startMethod)
        {
            ControlJobVerifyRequestedByHost?.Invoke(
                this,
                new ControlJobVerifyRequestedEventArgs(
                    messageId,
                    controlJobId,
                    Slice(carrierIds, 0, (int)carrierCount),
                    Slice(processJobIds, 0, (int)processJobCount),
                    (MaterialOrderMode)processOrderManagement,
                    (ControlJobStartMode)startMethod));
        }

        private void HandleCommandRequested(long messageId, string controlJobId, long command, string sCPName, string sCPVal)
        {
            ControlJobCommandRequestedByHost?.Invoke(this,
                new ControlJobCommandRequestedEventArgs(messageId, controlJobId, (ControlJobCommand)command));
        }

        private void HandleManualStartRequired(string controlJobId)
        {
            ControlJobManualStartRequired?.Invoke(this, new ControlJobManualStartEventArgs(controlJobId));
        }

        private void HandleHeadOfQueueChanged(string controlJobId)
        {
            ControlJobHeadOfQueueChanged?.Invoke(this, new ControlJobHoqChangedEventArgs(controlJobId));

            // 3

            // 셀렉트 이후 2)
        }

        private ControlJobAttributeInfo ReadControlJobAttributesOrDefault(string controlJobId)
        {
            if (string.IsNullOrWhiteSpace(controlJobId))
                return null;

            long result = 0;
            long messageId = 0;

            result = _driver.OpenGEMObject(
                ref messageId,
                "ControlJob",
                controlJobId);

            if (result < 0)
                return null;

            try
            {
                var info = new ControlJobAttributeInfo();

                info.CurrentProcessJobIds = ReadCurrentProcessJobIds(messageId);
                info.DataCollectionPlan = ReadDataCollectionPlan(messageId);
                info.CarrierInputIds = ReadCarrierInputIds(messageId);
                info.MaterialOutputSpecifications = ReadMaterialOutputSpecifications(messageId);
                info.MaterialOutputByStatus = ReadMaterialOutputByStatus(messageId);
                info.PauseEventIds = ReadPauseEventIds(messageId);
                info.ProcessJobStatus = ReadProcessJobStatus(messageId);
                info.ProcessingControlSpecifications = ReadProcessingControlSpecifications(messageId);
                info.ProcessOrderManagement = (MaterialOrderMode)ReadProcessOrderManagement(messageId);
                info.StartMode = ReadControlJobStartMode(messageId);
                info.State = ReadControlJobState(messageId);

                return info;
            }
            finally
            {
                _driver.CloseGEMObject(messageId);
            }
        }
        private string[] ReadCurrentProcessJobIds(long messageId)
        {
            long attrId = 0;
            long count = 0;

            _driver.GetAttrData(ref attrId, messageId, "CurrentPRJob");
            _driver.GetListItem(attrId, ref count);

            if (count <= 0)
                return new string[0];

            var result = new List<string>();

            for (int i = 0; i < count; ++i)
            {
                string processJobId = string.Empty;

                _driver.GetStringItem(attrId, ref processJobId);

                result.Add(processJobId ?? string.Empty);
            }

            return result.ToArray();
        }
        private string ReadDataCollectionPlan(long messageId)
        {
            long attrId = 0;
            string value = string.Empty;

            _driver.GetAttrData(ref attrId, messageId, "DataCollectionPlan");
            _driver.GetStringItem(attrId, ref value);

            return value ?? string.Empty;
        }
        private string[] ReadCarrierInputIds(long messageId)
        {
            long attrId = 0;
            long count = 0;

            _driver.GetAttrData(ref attrId, messageId, "CarrierInputSpec");
            _driver.GetListItem(attrId, ref count);

            if (count <= 0)
                return new string[0];

            var result = new List<string>();

            for (int i = 0; i < count; ++i)
            {
                string carrierId = string.Empty;

                _driver.GetStringItem(attrId, ref carrierId);

                result.Add(carrierId ?? string.Empty);
            }

            return result.ToArray();
        }
        private ControlJobMaterialOutputSpec[] ReadMaterialOutputSpecifications(long messageId)
        {
            long attrId = 0;
            long count = 0;

            _driver.GetAttrData(ref attrId, messageId, "MtrlOutSpec");
            _driver.GetListItem(attrId, ref count);

            if (count <= 0)
                return new ControlJobMaterialOutputSpec[0];

            var result = new List<ControlJobMaterialOutputSpec>();

            for (int i = 0; i < count; ++i)
            {
                long itemCount = 0;
                long listCount = 0;

                string attributeId = string.Empty;
                string value = string.Empty;

                // Outer item list.
                _driver.GetListItem(attrId, ref itemCount);

                // Source spec list.
                _driver.GetListItem(attrId, ref listCount);

                // AttrId : ASCII
                _driver.GetStringItem(attrId, ref attributeId);

                // Src SlotNo List n
                long sourceSlotCount = 0;
                _driver.GetListItem(attrId, ref sourceSlotCount);

                byte[] sourceSlotNumbers = ReadUint1ListItems(attrId, sourceSlotCount);

                // Destination/value spec list.
                _driver.GetListItem(attrId, ref listCount);

                // Value : ASCII
                _driver.GetStringItem(attrId, ref value);

                // Dest SlotNo List n
                long destinationSlotCount = 0;
                _driver.GetListItem(attrId, ref destinationSlotCount);

                byte[] destinationSlotNumbers = ReadUint1ListItems(attrId, destinationSlotCount);

                result.Add(
                    new ControlJobMaterialOutputSpec(
                        attributeId,
                        sourceSlotNumbers,
                        value,
                        destinationSlotNumbers));
            }

            return result.ToArray();
        }
        private ControlJobMaterialOutputByStatus[] ReadMaterialOutputByStatus(long messageId)
        {
            long attrId = 0;
            long count = 0;

            _driver.GetAttrData(ref attrId, messageId, "MtrlOutByStatus");
            _driver.GetListItem(attrId, ref count);

            if (count <= 0)
                return new ControlJobMaterialOutputByStatus[0];

            var result = new List<ControlJobMaterialOutputByStatus>();

            for (int i = 0; i < count; ++i)
            {
                long itemCount = 0;
                long slotCount = 0;

                // List 3
                _driver.GetListItem(attrId, ref itemCount);

                // Material Status : U1
                byte materialStatus = ReadUint1Item(attrId);

                // Value : ASCII
                string value = string.Empty;
                _driver.GetStringItem(attrId, ref value);

                // SlotNo List n
                _driver.GetListItem(attrId, ref slotCount);

                byte[] slotNumbers = ReadUint1ListItems(attrId, slotCount);

                result.Add(
                    new ControlJobMaterialOutputByStatus(
                        materialStatus,
                        value,
                        slotNumbers));
            }

            return result.ToArray();
        }
        private uint[] ReadPauseEventIds(long messageId)
        {
            long attrId = 0;
            long count = 0;

            _driver.GetAttrData(ref attrId, messageId, "PauseEvent");
            _driver.GetListItem(attrId, ref count);

            if (count <= 0)
                return new uint[0];

            int itemCount = checked((int)count);
            var result = new uint[itemCount];

            for (int i = 0; i < itemCount; ++i)
            {
                result[i] = ReadUint4Item(attrId);
            }

            return result;
        }
        private ControlJobProcessJobStatusInfo[] ReadProcessJobStatus(long messageId)
        {
            long attrId = 0;
            long count = 0;

            _driver.GetAttrData(ref attrId, messageId, "PRJobStatusList");
            _driver.GetListItem(attrId, ref count);

            if (count <= 0)
                return new ControlJobProcessJobStatusInfo[0];

            var result = new List<ControlJobProcessJobStatusInfo>();

            for (int i = 0; i < count; ++i)
            {
                long itemCount = 0;

                // List 2
                _driver.GetListItem(attrId, ref itemCount);

                // PRJobID : ASCII
                string processJobId = string.Empty;
                _driver.GetStringItem(attrId, ref processJobId);

                // PRJobState : U1
                byte stateValue = ReadUint1Item(attrId);

                result.Add(
                    new ControlJobProcessJobStatusInfo(
                        processJobId,
                        (ProcessJobState)stateValue));
            }

            return result.ToArray();
        }
        private ControlJobProcessingControlSpec[] ReadProcessingControlSpecifications(long messageId)
        {
            long attrId = 0;
            long count = 0;

            _driver.GetAttrData(ref attrId, messageId, "ProcessingCtrlSpec");
            _driver.GetListItem(attrId, ref count);

            if (count <= 0)
                return new ControlJobProcessingControlSpec[0];

            var result = new List<ControlJobProcessingControlSpec>();

            for (int i = 0; i < count; ++i)
            {
                long itemCount = 0;

                // List 3
                _driver.GetListItem(attrId, ref itemCount);

                // PRJobID : ASCII
                string processJobId = string.Empty;
                _driver.GetStringItem(attrId, ref processJobId);

                // Rule list
                long ruleCount = 0;
                _driver.GetListItem(attrId, ref ruleCount);

                var ruleNames = new List<string>();
                var ruleValues = new List<string>();

                for (int j = 0; j < ruleCount; ++j)
                {
                    long ruleItemCount = 0;
                    string ruleName = string.Empty;
                    string ruleValue = string.Empty;

                    // List 2
                    _driver.GetListItem(attrId, ref ruleItemCount);

                    // RuleName : ASCII
                    _driver.GetStringItem(attrId, ref ruleName);

                    // RuleValue : ASCII
                    _driver.GetStringItem(attrId, ref ruleValue);

                    ruleNames.Add(ruleName ?? string.Empty);
                    ruleValues.Add(ruleValue ?? string.Empty);
                }

                // Output rule list
                long outputRuleCount = 0;
                _driver.GetListItem(attrId, ref outputRuleCount);

                var outputRuleStates = new List<byte>();
                var outputRuleValues = new List<string>();

                for (int j = 0; j < outputRuleCount; ++j)
                {
                    long outputItemCount = 0;
                    string outputRuleValue = string.Empty;

                    // List 2
                    _driver.GetListItem(attrId, ref outputItemCount);

                    // OutputRuleStatus : U1
                    byte outputRuleStatus = ReadUint1Item(attrId);

                    // OutputRuleValue : ASCII
                    _driver.GetStringItem(attrId, ref outputRuleValue);

                    outputRuleStates.Add(outputRuleStatus);
                    outputRuleValues.Add(outputRuleValue ?? string.Empty);
                }

                result.Add(
                    new ControlJobProcessingControlSpec(
                        processJobId,
                        ruleNames.ToArray(),
                        ruleValues.ToArray(),
                        outputRuleStates.ToArray(),
                        outputRuleValues.ToArray()));
            }

            return result.ToArray();
        }
        private byte ReadProcessOrderManagement(long messageId)
        {
            long attrId = 0;

            _driver.GetAttrData(ref attrId, messageId, "ProcessOrderMgmt");

            return ReadUint1Item(attrId);
        }
        private byte ReadUint1Item(long attrId)
        {
            var buffer = new byte[1];

            _driver.GetUint1Item(attrId, ref buffer);

            if (buffer.Length == 0)
                return 0;

            return buffer[0];
        }

        private byte[] ReadUint1ListItems(long attrId, long count)
        {
            if (count <= 0)
                return new byte[0];

            int itemCount = checked((int)count);
            var result = new byte[itemCount];

            for (int i = 0; i < itemCount; ++i)
                result[i] = ReadUint1Item(attrId);

            return result;
        }

        private uint ReadUint4Item(long attrId)
        {
            var buffer = new uint[1];

            _driver.GetUint4Item(attrId, ref buffer);

            if (buffer.Length == 0)
                return 0;

            return buffer[0];
        }
        private ControlJobState ReadControlJobState(long messageId)
        {
            long attrId = 0;

            _driver.GetAttrData(ref attrId, messageId, "State");

            byte value = ReadUint1Item(attrId);

            return (ControlJobState)value;
        }
        private ControlJobStartMode ReadControlJobStartMode(long messageId)
        {
            long attrId = 0;

            _driver.GetAttrData(ref attrId, messageId, "StartMethod");

            byte value = ReadUint1Item(attrId);

            return (ControlJobStartMode)value;
        }
        private static void EnsureEqualLength(int first, int second, string firstName, string secondName)
        {
            if (first != second)
            {
                throw new ArgumentException(
                    string.Format(
                        "{0} and {1} length must match. {0}:{2}, {1}:{3}",
                        firstName,
                        secondName,
                        first,
                        second));
            }
        }

        private static T[] Slice<T>(T[] source, int start, int count)
        {
            if (source == null || count <= 0 || start >= source.Length)
                return new T[0];

            if (start < 0)
                start = 0;

            int actual = Math.Min(count, source.Length - start);
            T[] buffer = new T[actual];
            Array.Copy(source, start, buffer, 0, actual);
            return buffer;
        }
    }
}