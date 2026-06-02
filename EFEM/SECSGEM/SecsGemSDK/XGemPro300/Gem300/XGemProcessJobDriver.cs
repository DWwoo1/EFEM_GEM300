using System;
using System.Collections.Generic;
using System.Linq;

using FrameOfSystem3.SECSGEM.DefineSecsGem;
using FrameOfSystem3.SECSGEM.SecsGemSDK.Gem300;

using EFEM.Defines.LoadPort;
using EFEM.Defines.Common;
using EFEM.Defines.Job;

using XGEM300PRO.Library;

namespace FrameOfSystem3.SECSGEM
{
    internal sealed class XGemProcessJobDriver : IProcessJobDriver
    {
        private readonly XGem300ProW _driver;

        public XGemProcessJobDriver(XGem300ProW driver)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
            SubscribeDriverEvents();
        }

        private sealed class ProcessJobAttributeInfo
        {
            public ProcessJobState State { get; set; }

            public MaterialFormat MaterialFormat { get; set; }

            public ProcessStartMode StartMode { get; set; }

            public RecipeMethod RecipeMethod { get; set; }

            public string RecipeId { get; set; }

            public IReadOnlyDictionary<string, IReadOnlyList<int>> MaterialInfo { get; set; }

            public string[] RecipeParameterNames { get; set; }

            public string[] RecipeParameterValues { get; set; }

            public uint[] PauseEventIds { get; set; }
        }

        public event EventHandler<ProcessJobCreatedEventArgs> ProcessJobCreated;
        public event EventHandler<ProcessJobStateChangedEventArgs> ProcessJobStateChanged;
        public event EventHandler<ProcessJobDeletedEventArgs> ProcessJobDeleted;
        public event EventHandler<ProcessJobVerifyRequestedEventArgs> ProcessJobVerifyRequestedByHost;
        public event EventHandler<ProcessJobCommandRequestedEventArgs> ProcessJobCommandRequestedByHost;
        public event EventHandler<ProcessJobRecipeVariableRequestedEventArgs> ProcessJobRecipeVariablesRequestedByHost;
        public event EventHandler<ProcessJobStartMethodRequestedEventArgs> ProcessJobStartMethodRequestedByHost;
        public event EventHandler<ProcessJobMaterialOrderRequestedEventArgs> ProcessJobMaterialOrderRequestedByHost;
        public event EventHandler<ProcessJobManualStartEventArgs> ProcessJobManualStartRequired;
        public event EventHandler<ProcessJobSettingUpEventArgs> ProcessJobSettingUpRequested;

        public long Create(
            string processJobId,
            MaterialFormat materialFormat,
            ProcessStartMode startMode,
            MaterialOrderMode materialOrder,
            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo,
            RecipeMethod recipeMethod,
            string recipeId,
            string[] recipeParameterNames,
            string[] recipeParameterValues)
        {
            string[] safeMaterialIds;
            string[] safeSlotInfos;

            BuildMaterialArrays(
                materialInfo,
                out safeMaterialIds,
                out safeSlotInfos);

            string[] safeNames = recipeParameterNames ?? new string[0];
            string[] safeValues = recipeParameterValues ?? new string[0];

            EnsureEqualLength(safeNames.Length, safeValues.Length, nameof(recipeParameterNames), nameof(recipeParameterValues));

            return _driver.PJReqCreate(
                processJobId ?? string.Empty,
                (long)materialFormat,
                (long)startMode,
                (long)materialOrder,
                safeMaterialIds.Length,
                safeMaterialIds,
                safeSlotInfos,
                (long)recipeMethod,
                recipeId ?? string.Empty,
                safeNames.Length,
                safeNames,
                safeValues);
        }

        public long CreateWithNumericRecipe(
            string processJobId,
            MaterialFormat materialFormat,
            ProcessStartMode startMode,
            MaterialOrderMode materialOrder,
            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo,
            RecipeMethod recipeMethod,
            string recipeId,
            string[] recipeParameterNames,
            long[] recipeParameterValues)
        {
            string[] safeMaterialIds;
            string[] safeSlotInfos;
            BuildMaterialArrays(
                materialInfo,
                out safeMaterialIds,
                out safeSlotInfos);

            string[] safeNames = recipeParameterNames ?? new string[0];
            long[] safeValues = recipeParameterValues ?? new long[0];

            EnsureEqualLength(safeNames.Length, safeValues.Length, nameof(recipeParameterNames), nameof(recipeParameterValues));

            return _driver.PJReqCreateEx(
                processJobId ?? string.Empty,
                (long)materialFormat,
                (long)startMode,
                (long)materialOrder,
                safeMaterialIds.Length,
                safeMaterialIds,
                safeSlotInfos,
                (long)recipeMethod,
                recipeId ?? string.Empty,
                safeNames.Length,
                safeNames,
                safeValues);
        }

        public long RequestJob(string processJobId)
        {
            return _driver.PJReqGetJob(processJobId ?? string.Empty);
        }

        public long RequestAllJobIds()
        {
            return _driver.PJReqGetAllJobID();
        }

        public long RequestCommand(string processJobId, ProcessJobCommand command)
        {
            var result = _driver.PJReqCommand((long)command, processJobId ?? string.Empty);
            return result;
        }

        public long AcknowledgeVerify(long messageId, string[] processJobIds, long result, long[] errorCodes, string[] errorTexts)
        {
            string[] safeIds = processJobIds ?? new string[0];
            long[] safeCodes = errorCodes ?? new long[0];
            string[] safeTexts = errorTexts ?? new string[0];

            EnsureEqualLength(safeCodes.Length, safeTexts.Length, nameof(errorCodes), nameof(errorTexts));

            return _driver.PJRspVerify(
                messageId,
                safeIds.Length,
                safeIds,
                result,
                safeCodes.Length,
                safeCodes,
                safeTexts);
        }

        public long AcknowledgeCommand(long messageId, ProcessJobCommand command, string processJobId, long result, long[] errorCodes, string[] errorTexts)
        {
            long[] safeCodes = errorCodes ?? new long[0];
            string[] safeTexts = errorTexts ?? new string[0];

            EnsureEqualLength(safeCodes.Length, safeTexts.Length, nameof(errorCodes), nameof(errorTexts));

            return _driver.PJRspCommand(
                messageId,
                (long)command,
                processJobId ?? string.Empty,
                result,
                safeCodes.Length,
                safeCodes,
                safeTexts);
        }

        public long AcknowledgeRecipeVariables(long messageId, string processJobId, long result, long[] errorCodes, string[] errorTexts)
        {
            long[] safeCodes = errorCodes ?? new long[0];
            string[] safeTexts = errorTexts ?? new string[0];

            EnsureEqualLength(safeCodes.Length, safeTexts.Length, nameof(errorCodes), nameof(errorTexts));

            return _driver.PJRspSetRcpVariable(
                messageId,
                processJobId ?? string.Empty,
                result,
                safeCodes.Length,
                safeCodes,
                safeTexts);
        }

        public long AcknowledgeStartMethod(long messageId, string[] processJobIds, long result, long[] errorCodes, string[] errorTexts)
        {
            string[] safeIds = processJobIds ?? new string[0];
            long[] safeCodes = errorCodes ?? new long[0];
            string[] safeTexts = errorTexts ?? new string[0];

            EnsureEqualLength(safeCodes.Length, safeTexts.Length, nameof(errorCodes), nameof(errorTexts));

            return _driver.PJRspSetStartMethod(
                messageId,
                safeIds.Length,
                safeIds,
                result,
                safeCodes.Length,
                safeCodes,
                safeTexts);
        }

        public long AcknowledgeMaterialOrder(long messageId, long result)
        {
            return _driver.PJRspSetMtrlOrder(messageId, result);
        }

        public long SetJobInfo(
            string processJobId,
            EFEM.Defines.Common.MaterialFormat materialFormat,
            ProcessStartMode startMode,
            MaterialOrderMode materialOrder,
            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo,
            RecipeMethod recipeMethod,
            string recipeId,
            string[] recipeParameterNames,
            string[] recipeParameterValues)
        {
            string[] safeMaterialIds;
            string[] safeSlotInfos;
            BuildMaterialArrays(
                materialInfo,
                out safeMaterialIds,
                out safeSlotInfos);

            string[] safeNames = recipeParameterNames ?? new string[0];
            string[] safeValues = recipeParameterValues ?? new string[0];

            EnsureEqualLength(safeNames.Length, safeValues.Length, nameof(recipeParameterNames), nameof(recipeParameterValues));

            return _driver.PJSetJobInfo(
                processJobId ?? string.Empty,
                (long)materialFormat,
                (long)startMode,
                (long)materialOrder,
                safeMaterialIds.Length,
                safeMaterialIds,
                safeSlotInfos,
                (long)recipeMethod,
                recipeId ?? string.Empty,
                safeNames.Length,
                safeNames,
                safeValues);
        }

        public long SetJobInfoWithNumericRecipe(
            string processJobId,
            MaterialFormat materialFormat,
            ProcessStartMode startMode,
            MaterialOrderMode materialOrder,
            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo,
            RecipeMethod recipeMethod,
            string recipeId,
            string[] recipeParameterNames,
            long[] recipeParameterValues)
        {
            string[] safeMaterialIds;
            string[] safeSlotInfos;
            BuildMaterialArrays(
                materialInfo,
                out safeMaterialIds,
                out safeSlotInfos);

            string[] safeNames = recipeParameterNames ?? new string[0];
            long[] safeValues = recipeParameterValues ?? new long[0];
            EnsureEqualLength(safeNames.Length, safeValues.Length, nameof(recipeParameterNames), nameof(recipeParameterValues));

            return _driver.PJSetJobInfoEx(
                processJobId ?? string.Empty,
                (long)materialFormat,
                (long)startMode,
                (long)materialOrder,
                safeMaterialIds.Length,
                safeMaterialIds,
                safeSlotInfos,
                (long)recipeMethod,
                recipeId ?? string.Empty,
                safeNames.Length,
                safeNames,
                safeValues);
        }

        public long SetState(string processJobId, ProcessJobState state)
        {
            return _driver.PJSetState(processJobId ?? string.Empty, (long)state);
        }

        public long NotifySettingUpStarted(string processJobId)
        {
            return _driver.PJSettingUpStart(processJobId ?? string.Empty);
        }

        public long NotifySettingUpCompleted(string processJobId)
        {
            return _driver.PJSettingUpCompt(processJobId ?? string.Empty);
        }

        public long Remove(string processJobId)
        {
            return _driver.PJDelJobInfo(processJobId ?? string.Empty);
        }

        public long RemoveAll()
        {
            return _driver.PJDelAllJobInfo();
        }

        private void SubscribeDriverEvents()
        {
            _driver.OnPJCreated += HandleCreated;
            _driver.OnPJStateChanged += HandleStateChanged;
            _driver.OnPJDeleted += HandleDeleted;
            _driver.OnPJReqVerify += HandleVerifyRequested;
            _driver.OnPJReqCommand += HandleCommandRequested;
            _driver.OnPJRspCommand += HandleResponseJobCommand;
            _driver.OnPJReqSetRecipeVariable += HandleRecipeVariablesRequested;
            _driver.OnPJReqSetStartMethod += HandleStartMethodRequested;
            _driver.OnPJReqSetMtrlOrder += HandleMaterialOrderRequested;
            _driver.OnPJManualStartDisplay += HandleManualStartRequired;
            _driver.OnPJSettingUpStart += HandleSettingUpRequested;
        }

        private void HandleCreated(
            string processJobId,
            long materialFormat,
            long startMode,
            long materialOrder,
            long materialCount,
            string[] materialIds,
            string[] slotInfos,
            long recipeMethod,
            string recipeId,
            long recipeParamCount,
            string[] recipeParamNames,
            string[] recipeParamValues)
        {
            var order = (MaterialOrderMode)materialOrder;

            var attributeInfo = ReadProcessJobAttributesOrDefault(processJobId);

            ProcessJobInfo eventInfo;

            if (attributeInfo == null)
            {
                IReadOnlyDictionary<string, IReadOnlyList<int>> info = 
                    BuildMaterialInfo(
                        materialIds,
                        slotInfos,
                        slotWidth: 1);

                eventInfo = new ProcessJobInfo(
                    processJobId,
                    ProcessJobState.JobQueued,
                    (MaterialFormat)materialFormat,
                    (ProcessStartMode)startMode,
                    order,
                    info,
                    (RecipeMethod)recipeMethod,
                    recipeId,
                    CreateRecipeParameters(
                        Slice(recipeParamNames, 0, (int)recipeParamCount),
                        Slice(recipeParamValues, 0, (int)recipeParamCount)),
                    null);
            }
            else
            {
                eventInfo = new ProcessJobInfo(
                    processJobId,
                    ProcessJobState.JobQueued,
                    attributeInfo.MaterialFormat,
                    attributeInfo.StartMode,
                    order, // Attribute 표에 MaterialOrder가 없으므로 callback 값을 유지
                    attributeInfo.MaterialInfo,
                    attributeInfo.RecipeMethod,
                    attributeInfo.RecipeId,
                    CreateRecipeParameters(
                        attributeInfo.RecipeParameterNames,
                        attributeInfo.RecipeParameterValues),
                    attributeInfo.PauseEventIds);
            }

            ProcessJobCreated?.Invoke(
                this,
                new ProcessJobCreatedEventArgs(eventInfo));
        }

        private void HandleStateChanged(string processJobId, long state)
        {
            ProcessJobStateChanged?.Invoke(this, new ProcessJobStateChangedEventArgs(processJobId, (ProcessJobState)state));
        }

        private void HandleDeleted(string processJobId)
        {
            ProcessJobDeleted?.Invoke(this, new ProcessJobDeletedEventArgs(processJobId));
        }

        private void HandleVerifyRequested(
            long messageId,
            long jobCount,
            string[] jobIds,
            long[] materialFormats,
            long[] startModes,
            long[] materialOrders,
            long[] materialCounts,
            string[] materialIds,
            string[] slotInfos,
            long[] recipeMethods,
            string[] recipeIds,
            long[] recipeParamCounts,
            string[] recipeParamNames,
            string[] recipeParamValues)
        {
            var jobs = new List<ProcessJobInfo>();

            int materialIndex = 0;
            int recipeIndex = 0;

            IReadOnlyDictionary<string, IReadOnlyList<int>> info =
                BuildMaterialInfo(
                    materialIds,
                    slotInfos,
                    slotWidth: 1);

            for (int i = 0; i < jobCount; i++)
            {
                int currentMaterialCount = (int)materialCounts[i];
                int currentRecipeCount = (int)recipeParamCounts[i];

                jobs.Add(
                    new ProcessJobInfo(
                        jobIds[i],
                        ProcessJobState.JobQueued,
                        (MaterialFormat)materialFormats[i],
                        (ProcessStartMode)startModes[i],
                        (MaterialOrderMode)materialOrders[i],
                        info,
                        (RecipeMethod)recipeMethods[i],
                        recipeIds[i],
                        CreateRecipeParameters(
                            Slice(recipeParamNames, recipeIndex, currentRecipeCount),
                            Slice(recipeParamValues, recipeIndex, currentRecipeCount)), null));

                materialIndex += currentMaterialCount;
                recipeIndex += currentRecipeCount;
            }

            // TODO : 잡 생성 정보를 보고 Verify
            // 1. 매니저에서함
            //_driver.PJRspVerify(
            //    messageId,
            //    jobCount,
            //    jobIds,
            //    0,
            //    0,
            //    Array.Empty<long>(),
            //    Array.Empty<string>());
            // 2. Callback: PJCreated()
            // 3. Callback: PJStateChanged()

            ProcessJobVerifyRequestedByHost?.Invoke(this, new ProcessJobVerifyRequestedEventArgs(messageId, jobs));
        }

        // TODO : 외부로 빼기 필요
        private void HandleResponseJobCommand(string processJobId, long command, long result)
        {
            // 커맨드에 따라 설정해야하는듯
            var cmd = (ProcessJobCommand)command;
            switch (cmd)
            {
                case ProcessJobCommand.Resume:
                case ProcessJobCommand.Start:
                    _driver.PJSetState(processJobId, (long)ProcessJobState.Processing);
                    break;
                case ProcessJobCommand.Pause:
                    {
                        _driver.PJSetState(processJobId, (long)ProcessJobState.Pausing);
                        _driver.PJSetState(processJobId, (long)ProcessJobState.Paused);
                    }
                    break;
                case ProcessJobCommand.Stop:
                    {
                        _driver.PJSetState(processJobId, (long)ProcessJobState.Stopping);
                        _driver.PJSetState(processJobId, (long)ProcessJobState.Stopped);
                    }
                    break;
                case ProcessJobCommand.Abort:
                    {
                        _driver.PJSetState(processJobId, (long)ProcessJobState.Aborting);
                        _driver.PJSetState(processJobId, (long)ProcessJobState.Aborted);
                    }
                    break;
                case ProcessJobCommand.Cancel:
                    {
                        _driver.PJSetState(processJobId, (long)ProcessJobState.JobCanceled);
                    }
                    break;
                default:
                    break;
            }
        }

        // TODO : 외부로 빼기 필요
        private void HandleCommandRequested(long messageId, string processJobId, long command)
        {
            // 매니저에서함
            //var result = _driver.PJRspCommand(messageId, command, processJobId,
            //    0,
            //    0,
            //    Array.Empty<long>(),
            //    Array.Empty<string>());

            ProcessJobCommandRequestedByHost?.Invoke(this,
                new ProcessJobCommandRequestedEventArgs(
                    messageId,
                    processJobId, 
                    (ProcessJobCommand)command));

            // 커맨드에 따라 설정해야하는듯
            var cmd = (ProcessJobCommand)command;
            switch (cmd)
            {
                case ProcessJobCommand.Resume:
                case ProcessJobCommand.Start:
                    _driver.PJSetState(processJobId, (long)ProcessJobState.Processing);
                    break;
                case ProcessJobCommand.Pause:
                    {
                        _driver.PJSetState(processJobId, (long)ProcessJobState.Pausing);
                        _driver.PJSetState(processJobId, (long)ProcessJobState.Paused);
                    }
                    break;
                case ProcessJobCommand.Stop:
                    {
                        _driver.PJSetState(processJobId, (long)ProcessJobState.Stopping);
                        _driver.PJSetState(processJobId, (long)ProcessJobState.Stopped);
                    }
                    break;
                case ProcessJobCommand.Abort:
                    {
                        _driver.PJSetState(processJobId, (long)ProcessJobState.Aborting);
                        _driver.PJSetState(processJobId, (long)ProcessJobState.Aborted);
                    }
                    break;
                case ProcessJobCommand.Cancel:
                    {
                        _driver.PJSetState(processJobId, (long)ProcessJobState.JobCanceled);
                    }
                    break;
                default:
                    break;
            }
        }

        private void HandleRecipeVariablesRequested(long messageId, string processJobId, long count, string[] names, string[] values)
        {
            ProcessJobRecipeVariablesRequestedByHost?.Invoke(
                this,
                new ProcessJobRecipeVariableRequestedEventArgs(
                    messageId,
                    processJobId,
                    CreateRecipeParameters(Slice(names, 0, (int)count), Slice(values, 0, (int)count))));
        }

        private void HandleStartMethodRequested(long messageId, long count, string[] processJobIds, long startMode)
        {
            ProcessJobStartMethodRequestedByHost?.Invoke(
                this,
                new ProcessJobStartMethodRequestedEventArgs(
                    messageId,
                    Slice(processJobIds, 0, (int)count),
                    startMode));
        }

        private void HandleMaterialOrderRequested(long messageId, long materialOrder)
        {
            ProcessJobMaterialOrderRequestedByHost?.Invoke(this, new ProcessJobMaterialOrderRequestedEventArgs(messageId, (MaterialOrderMode)materialOrder));
        }

        private void HandleManualStartRequired(string processJobId)
        {
            ProcessJobManualStartRequired?.Invoke(this, new ProcessJobManualStartEventArgs(processJobId));
        }

        private void HandleSettingUpRequested(string processJobId)
        {
            ProcessJobSettingUpRequested?.Invoke(this, new ProcessJobSettingUpEventArgs(processJobId));
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

        private ProcessJobAttributeInfo ReadProcessJobAttributesOrDefault(string processJobId)
        {
            if (string.IsNullOrWhiteSpace(processJobId))
                return null;

            long result = 0;
            long messageId = 0;

            result = _driver.OpenGEMObject(
                ref messageId,
                "ProcessJob",
                processJobId);

            if (result < 0)
                return null;

            try
            {
                var info = new ProcessJobAttributeInfo();

                info.PauseEventIds = ReadPauseEventIds(messageId);
                info.State = ReadProcessJobState(messageId);
                ReadMaterialNameList(
                    messageId,
                    out var materialInfo);

                info.MaterialInfo = materialInfo;
                info.MaterialFormat = ReadMaterialFormat(messageId);
                info.StartMode = ReadProcessStartMode(messageId);
                info.RecipeMethod = ReadRecipeMethod(messageId);
                info.RecipeId = ReadRecipeId(messageId);

                ReadRecipeVariables(
                    messageId,
                    out string[] recipeParameterNames,
                    out string[] recipeParameterValues);

                info.RecipeParameterNames = recipeParameterNames;
                info.RecipeParameterValues = recipeParameterValues;

                return info;
            }
            finally
            {
                _driver.CloseGEMObject(messageId);
            }
        }
        private uint[] ReadPauseEventIds(long messageId)
        {
            long attrId = 0;
            long count = 0;

            _driver.GetAttrData(ref attrId, messageId, "PauseEvent");
            _driver.GetListItem(attrId, ref count);

            if (count <= 0)
                return new uint[0];

            var values = new uint[count];

            for (int i = 0; i < count; ++i)
            {
                uint[] buffer = new uint[1];

                _driver.GetUint4Item(attrId, ref buffer);

                if (buffer.Length > 0)
                    values[i] = buffer[0];
            }

            return values;
        }
        private ProcessJobState ReadProcessJobState(long messageId)
        {
            long attrId = 0;

            _driver.GetAttrData(ref attrId, messageId, "PRJobState");

            var value = new byte[1];
            _driver.GetUint1Item(attrId, ref value);

            if (value.Length == 0)
                return ProcessJobState.JobQueued;

            return (ProcessJobState)value[0];
        }
        private MaterialFormat ReadMaterialFormat(long messageId)
        {
            long attrId = 0;

            _driver.GetAttrData(ref attrId, messageId, "PRMtlType");

            var value = new byte[1];
            _driver.GetUint1Item(attrId, ref value);

            if (value.Length == 0)
                return default(MaterialFormat);

            return (MaterialFormat)value[0];
        }
        private ProcessStartMode ReadProcessStartMode(long messageId)
        {
            long attrId = 0;

            _driver.GetAttrData(ref attrId, messageId, "PRProcessStart");

            var value = new byte[1];
            _driver.GetUint1Item(attrId, ref value);

            if (value.Length == 0)
                return default(ProcessStartMode);

            return (ProcessStartMode)value[0];
        }
        private RecipeMethod ReadRecipeMethod(long messageId)
        {
            long attrId = 0;

            _driver.GetAttrData(ref attrId, messageId, "PRRecipeMethod");

            var value = new byte[1];
            _driver.GetUint1Item(attrId, ref value);

            if (value.Length == 0)
                return default(RecipeMethod);

            return (RecipeMethod)value[0];
        }
        private string ReadRecipeId(long messageId)
        {
            long attrId = 0;
            string value = string.Empty;

            _driver.GetAttrData(ref attrId, messageId, "RecID");
            _driver.GetStringItem(attrId, ref value);

            return value ?? string.Empty;
        }
        private void ReadMaterialNameList(
            long messageId,
            out IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo)
        {
            var info = new Dictionary<string, IReadOnlyList<int>>();

            long attrId = 0;
            long materialCount = 0;

            _driver.GetAttrData(ref attrId, messageId, "PRMtlNameList");
            _driver.GetListItem(attrId, ref materialCount);
            for (int i = 0; i < materialCount; ++i)
            {
                long itemCount = 0;
                string materialId = string.Empty;

                // List 2
                _driver.GetListItem(attrId, ref itemCount);

                // Id : ASCII
                _driver.GetStringItem(attrId, ref materialId);

                // SlotNo List n
                long slotCount = 0;
                _driver.GetListItem(attrId, ref slotCount);

                var slotNumbers = new List<int>();

                for (int j = 0; j < slotCount; ++j)
                {
                    var slotValue = new byte[1];
                    _driver.GetUint1Item(attrId, ref slotValue);

                    if (slotValue.Length > 0)
                        slotNumbers.Add(slotValue[0]);
                }

                info[materialId] = slotNumbers;
            }

            materialInfo = info;
        }
        private void ReadRecipeVariables(
            long messageId,
            out string[] recipeParameterNames,
            out string[] recipeParameterValues)
        {
            long attrId = 0;
            long count = 0;

            _driver.GetAttrData(ref attrId, messageId, "RecVariableList");
            _driver.GetListItem(attrId, ref count);

            if (count <= 0)
            {
                recipeParameterNames = new string[0];
                recipeParameterValues = new string[0];
                return;
            }

            var names = new List<string>();
            var values = new List<string>();

            for (int i = 0; i < count; ++i)
            {
                long itemCount = 0;
                string name = string.Empty;
                string value = string.Empty;

                // List 2
                _driver.GetListItem(attrId, ref itemCount);

                // Name : ASCII
                _driver.GetStringItem(attrId, ref name);

                // Value : ASCII
                _driver.GetStringItem(attrId, ref value);

                names.Add(name ?? string.Empty);
                values.Add(value ?? string.Empty);
            }

            recipeParameterNames = names.ToArray();
            recipeParameterValues = values.ToArray();
        }

        //private void GetProcessJobInfoFromGettingAttribute(string processJobId)
        //{
        //    long nRet = 0;
        //    long lMsgId = 0;
        //    long lAttrId = 0;
        //    short nValue = 0;
        //    long lCount = 0;
        //    long lItem2 = 0, lItem3 = 0;
        //    string sValue = "";

        //    // U1 값을 읽기 위한 버퍼.
        //    // PRJobState, PRMtlType, PRProcessStart, PRRecipeMethod 등이 U1 형식이다.
        //    byte[] pnaValue = new byte[1];

        //    // PauseEvent는 List n / EventID : 4-byte unsigned integer 형식이므로 uint 배열로 읽는다.
        //    uint[] pndValue;

        //    // GEM Object "ProcessJob"을 processJobId 기준으로 연다.
        //    // 성공하면 lMsgId를 통해 이후 Attribute를 조회할 수 있다.
        //    nRet = _driver.OpenGEMObject(ref lMsgId, "ProcessJob", processJobId);

        //    if (nRet >= 0)
        //    {
        //        // ------------------------------------------------------------
        //        // PauseEvent
        //        // Format:
        //        //   List n
        //        //     EventID : 4-byte unsigned integer
        //        //
        //        // 의미:
        //        //   ProcessJob이 Pause될 때 사용되는 Event ID 목록.
        //        // ------------------------------------------------------------
        //        _driver.GetAttrData(ref lAttrId, lMsgId, "PauseEvent");
        //        _driver.GetListItem(lAttrId, ref lCount);

        //        pndValue = new uint[lCount];

        //        for (int i = 0; i < lCount; i++)
        //        {
        //            // 각 EventID를 U4 값으로 읽는다.
        //            // SDK 함수가 배열에 누적 저장하는 방식인지, 현재 index에 저장하는 방식인지는 SDK 동작 확인 필요.
        //            _driver.GetUint4Item(lAttrId, ref pndValue);
        //        }

        //        // ------------------------------------------------------------
        //        // PRJobState
        //        // Format:
        //        //   1-byte unsigned integer
        //        //
        //        // 의미:
        //        //   ProcessJob의 현재 상태.
        //        //   예: JobQueued, SettingUp, WaitingForStart, Processing 등.
        //        // ------------------------------------------------------------
        //        _driver.GetAttrData(ref lAttrId, lMsgId, "PRJobState");

        //        pnaValue = new byte[1];
        //        _driver.GetUint1Item(lAttrId, ref pnaValue);

        //        // ------------------------------------------------------------
        //        // PRMtlNameList
        //        // Format:
        //        //   List n
        //        //     List 2
        //        //       Id       : ASCII
        //        //       List n
        //        //         SlotNo : 1-byte unsigned integer
        //        //
        //        // 의미:
        //        //   ProcessJob에 포함된 Material ID와 Slot 번호 목록.
        //        //   MaterialFormat / MaterialIds / SlotInfos와 매핑될 수 있다.
        //        // ------------------------------------------------------------
        //        _driver.GetAttrData(ref lAttrId, lMsgId, "PRMtlNameList");
        //        _driver.GetListItem(lAttrId, ref lCount);

        //        for (int i = 0; i < lCount; i++)
        //        {
        //            // Material 항목 하나는 List 2 구조.
        //            // [0] Material Id
        //            // [1] SlotNo List
        //            _driver.GetListItem(lAttrId, ref lItem2);

        //            // Material Id : ASCII
        //            _driver.GetStringItem(lAttrId, ref sValue);

        //            // SlotNo List n
        //            _driver.GetListItem(lAttrId, ref lItem2);

        //            pnaValue = new byte[lItem2];

        //            for (int j = 0; j < lItem2; j++)
        //            {
        //                // SlotNo : U1
        //                // SDK가 배열을 어떻게 채우는지는 확인 필요.
        //                _driver.GetUint1Item(lAttrId, ref pnaValue);
        //            }
        //        }

        //        // ------------------------------------------------------------
        //        // PRMtlType
        //        // Format:
        //        //   1-byte unsigned integer
        //        //
        //        // 의미:
        //        //   Material 형식.
        //        //   코드에서는 MaterialFormat 또는 관련 enum으로 변환 가능.
        //        // ------------------------------------------------------------
        //        _driver.GetAttrData(ref lAttrId, lMsgId, "PRMtlType");

        //        pnaValue = new byte[1];
        //        _driver.GetUint1Item(lAttrId, ref pnaValue);

        //        // ------------------------------------------------------------
        //        // PRProcessStart
        //        // Format:
        //        //   1-byte unsigned integer
        //        //
        //        // 의미:
        //        //   ProcessJob 시작 방식.
        //        //   ProcessStartMode와 매핑될 수 있다.
        //        // ------------------------------------------------------------
        //        _driver.GetAttrData(ref lAttrId, lMsgId, "PRProcessStart");

        //        pnaValue = new byte[1];
        //        _driver.GetUint1Item(lAttrId, ref pnaValue);

        //        // ------------------------------------------------------------
        //        // PRRecipeMethod
        //        // Format:
        //        //   1-byte unsigned integer
        //        //
        //        // 의미:
        //        //   Recipe 지정 방식.
        //        //   RecipeMethod와 매핑될 수 있다.
        //        // ------------------------------------------------------------
        //        _driver.GetAttrData(ref lAttrId, lMsgId, "PRRecipeMethod");

        //        pnaValue = new byte[1];
        //        _driver.GetUint1Item(lAttrId, ref pnaValue);

        //        // ------------------------------------------------------------
        //        // RecID
        //        // Format:
        //        //   ASCII
        //        //
        //        // 의미:
        //        //   ProcessJob에서 사용하는 Recipe ID.
        //        // ------------------------------------------------------------
        //        _driver.GetAttrData(ref lAttrId, lMsgId, "RecID");
        //        _driver.GetStringItem(lAttrId, ref sValue);

        //        // ------------------------------------------------------------
        //        // RecVariableList
        //        // Format:
        //        //   List n
        //        //     List 2
        //        //       Name  : ASCII
        //        //       Value : ASCII
        //        //
        //        // 의미:
        //        //   Recipe Parameter 이름/값 목록.
        //        //   recipeParameterNames / recipeParameterValues와 매핑될 수 있다.
        //        // ------------------------------------------------------------
        //        _driver.GetAttrData(ref lAttrId, lMsgId, "RecVariableList");
        //        _driver.GetListItem(lAttrId, ref lCount);

        //        for (int i = 0; i < lCount; i++)
        //        {
        //            // Recipe Variable 항목 하나는 List 2 구조.
        //            // [0] Name
        //            // [1] Value
        //            _driver.GetListItem(lAttrId, ref lItem2);

        //            // Name : ASCII
        //            _driver.GetStringItem(lAttrId, ref sValue);

        //            // Value : ASCII
        //            _driver.GetStringItem(lAttrId, ref sValue);
        //        }
        //    }

        //    // GEM Object를 닫는다.
        //    // 가능하면 OpenGEMObject 성공 시에만 CloseGEMObject를 호출하는 것이 안전하다.
        //    _driver.CloseGEMObject(lMsgId);
        //}
        private static void BuildMaterialArrays(
            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo,
            out string[] safeMaterialIds,
            out string[] safeSlotInfos)
        {
            if (materialInfo == null || materialInfo.Count == 0)
            {
                safeMaterialIds = new string[0];
                safeSlotInfos = new string[0];
                return;
            }

            var materialIds = new List<string>(materialInfo.Count);
            var slotInfos = new List<string>(materialInfo.Count);

            foreach (KeyValuePair<string, IReadOnlyList<int>> item in materialInfo)
            {
                // Key는 Driver로 전달할 MaterialId 또는 CarrierId.
                string materialId = item.Key ?? string.Empty;

                // Value는 슬롯 목록.
                // 예: ["1", "2", "3"] -> "123"
                string slotInfo = BuildCompactSlotInfoText(item.Value);

                materialIds.Add(materialId);
                slotInfos.Add(slotInfo);
            }

            safeMaterialIds = materialIds.ToArray();
            safeSlotInfos = slotInfos.ToArray();
        }

        private static string BuildCompactSlotInfoText(IReadOnlyList<int> slots)
        {
            if (slots == null || slots.Count == 0)
                return string.Empty;

            var builder = new System.Text.StringBuilder();

            for (int i = 0; i < slots.Count; ++i)
            {
                // 구분자 없이 붙인다.
                // ["1", "2", "3"] -> "123"
                builder.Append(slots[i]);
            }

            return builder.ToString();
        }
        private static IReadOnlyDictionary<string, IReadOnlyList<int>> BuildMaterialInfo(
            string[] materialIds,
            string[] slotInfos,
            int slotWidth)
        {
            if (slotWidth <= 0)
                throw new ArgumentOutOfRangeException(nameof(slotWidth));

            string[] safeMaterialIds = materialIds ?? new string[0];
            string[] safeSlotInfos = slotInfos ?? new string[0];

            EnsureEqualLength(
                safeMaterialIds.Length,
                safeSlotInfos.Length,
                nameof(materialIds),
                nameof(slotInfos));

            var result = new Dictionary<string, IReadOnlyList<int>>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < safeMaterialIds.Length; ++i)
            {
                string materialId = safeMaterialIds[i] ?? string.Empty;

                if (string.IsNullOrWhiteSpace(materialId))
                    continue;

                string slotText = safeSlotInfos[i] ?? string.Empty;

                IReadOnlyList<int> slots = SplitCompactSlotInfoText(
                    slotText,
                    slotWidth);

                result[materialId] = slots;
            }

            return result;
        }

        private static IReadOnlyList<int> SplitCompactSlotInfoText(
            string slotText,
            int slotWidth)
        {
            if (slotWidth <= 0)
                throw new ArgumentOutOfRangeException(nameof(slotWidth));

            var result = new List<int>();

            if (string.IsNullOrWhiteSpace(slotText))
                return result;

            if (slotText.Length % slotWidth != 0)
            {
                throw new ArgumentException(
                    "Slot text length is not divisible by slotWidth. SlotText="
                    + slotText
                    + ", SlotWidth="
                    + slotWidth,
                    nameof(slotText));
            }

            for (int i = 0; i < slotText.Length; i += slotWidth)
            {
                string slot = slotText.Substring(i, slotWidth);
                if (false == string.IsNullOrWhiteSpace(slot) &&
                    int.TryParse(slot, out var state) &&
                    state == (int)CarrierSlotMapStates.CorrectlyOccupied)
                {
                    result.Add(i + 1);
                }
            }

            return result;
        }
        private static ProcessRecipeParameter[] CreateRecipeParameters(string[] names, string[] values)
        {
            int count = Math.Min(names == null ? 0 : names.Length, values == null ? 0 : values.Length);
            if (count <= 0)
                return new ProcessRecipeParameter[0];

            ProcessRecipeParameter[] result = new ProcessRecipeParameter[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = new ProcessRecipeParameter(names[i], values[i]);
            }

            return result;
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