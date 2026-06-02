using System;
using System.Collections.Generic;

using FrameOfSystem3.SECSGEM.SecsGemSDK.Gem300;
using FrameOfSystem3.SECSGEM.DefineSecsGem;

using EFEM.Defines.Common;
using EFEM.Defines.Job;

namespace FrameOfSystem3.SECSGEM
{
    internal sealed class ProcessJobService : IProcessJobService
    {
        private readonly object _driverLock = new object();

        private IProcessJobDriver _driver;

        private readonly object _callbackLock = new object();
        private readonly List<IProcessJobServiceCallback> _callbacks = new List<IProcessJobServiceCallback>();

        public bool IsDriverAttached
        {
            get
            {
                lock (_driverLock)
                {
                    return _driver != null;
                }
            }
        }

        public void AttachDriver(IProcessJobDriver driver)
        {
            if (driver == null)
                throw new ArgumentNullException("driver");

            lock (_driverLock)
            {
                if (object.ReferenceEquals(_driver, driver))
                    return;

                DetachDriverCore();

                _driver = driver;
                SubscribeDriverEvents();
            }
        }

        public void DetachDriver()
        {
            lock (_driverLock)
            {
                DetachDriverCore();
            }
        }

        private void DetachDriverCore()
        {
            if (_driver == null)
                return;

            UnsubscribeDriverEvents();
            _driver = null;
        }

        private IProcessJobDriver GetDriver()
        {
            lock (_driverLock)
            {
                if (_driver == null)
                    throw new InvalidOperationException("ProcessJob driver is not attached.");

                return _driver;
            }
        }

        public void RegisterCallback(IProcessJobServiceCallback callback)
        {
            if (callback == null)
                throw new ArgumentNullException("callback");

            lock (_callbackLock)
            {
                if (_callbacks.Contains(callback))
                    return;

                _callbacks.Add(callback);
            }
        }

        public void UnregisterCallback(IProcessJobServiceCallback callback)
        {
            if (callback == null)
                return;

            lock (_callbackLock)
            {
                _callbacks.Remove(callback);
            }
        }

        private void NotifyCallbacks(Action<IProcessJobServiceCallback> notify)
        {
            foreach (var item in _callbacks)
            {
                notify(item);
            }
        }
        //public void SetHostRequestHandler(IProcessJobHostRequestHandler handler)
        //{
        //    if (handler == null)
        //        throw new ArgumentNullException(nameof(handler));

        //    lock (_callbackLock)
        //    {
        //        if (_hostRequestHandler != null && !ReferenceEquals(_hostRequestHandler, handler))
        //            throw new InvalidOperationException("ProcessJob host request handler is already registered.");

        //        _hostRequestHandler = handler;
        //    }
        //}
        //public void ClearHostRequestHandler(IProcessJobHostRequestHandler handler)
        //{
        //    if (handler == null)
        //        return;

        //    lock (_callbackLock)
        //    {
        //        if (ReferenceEquals(_hostRequestHandler, handler))
        //            _hostRequestHandler = null;
        //    }
        //}

        public long Create(string processJobId, MaterialFormat materialFormat, ProcessStartMode startMode, MaterialOrderMode materialOrder, IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo, RecipeMethod recipeMethod, string recipeId, string[] recipeParameterNames, string[] recipeParameterValues)
        {
            return _driver.Create(processJobId, materialFormat, startMode, materialOrder, materialInfo, recipeMethod, recipeId, recipeParameterNames, recipeParameterValues);
        }

        public long CreateWithNumericRecipe(string processJobId, MaterialFormat materialFormat, ProcessStartMode startMode, MaterialOrderMode materialOrder, IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo, RecipeMethod recipeMethod, string recipeId, string[] recipeParameterNames, long[] recipeParameterValues)
        {
            return _driver.CreateWithNumericRecipe(processJobId, materialFormat, startMode, materialOrder, materialInfo, recipeMethod, recipeId, recipeParameterNames, recipeParameterValues);
        }

        public long RequestJob(string processJobId)
        {
            return _driver.RequestJob(processJobId);
        }

        public long RequestAllJobIds()
        {
            return _driver.RequestAllJobIds();
        }

        public long RequestCommand(string processJobId, ProcessJobCommand command)
        {
            return _driver.RequestCommand(processJobId, command);
        }

        public long AcknowledgeVerify(long messageId, string[] processJobIds, long result, long[] errorCodes, string[] errorTexts)
        {
            return _driver.AcknowledgeVerify(messageId, processJobIds, result, errorCodes, errorTexts);
        }

        public long AcknowledgeCommand(long messageId, ProcessJobCommand command, string processJobId, long result, long[] errorCodes, string[] errorTexts)
        {
            return _driver.AcknowledgeCommand(messageId, command, processJobId, result, errorCodes, errorTexts);
        }

        public long AcknowledgeRecipeVariables(long messageId, string processJobId, long result, long[] errorCodes, string[] errorTexts)
        {
            return _driver.AcknowledgeRecipeVariables(messageId, processJobId, result, errorCodes, errorTexts);
        }

        public long AcknowledgeStartMethod(long messageId, string[] processJobIds, long result, long[] errorCodes, string[] errorTexts)
        {
            return _driver.AcknowledgeStartMethod(messageId, processJobIds, result, errorCodes, errorTexts);
        }

        public long AcknowledgeMaterialOrder(long messageId, long result)
        {
            return _driver.AcknowledgeMaterialOrder(messageId, result);
        }

        public long SetJobInfo(string processJobId, MaterialFormat materialFormat, ProcessStartMode startMode, MaterialOrderMode materialOrder, IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo, RecipeMethod recipeMethod, string recipeId, string[] recipeParameterNames, string[] recipeParameterValues)
        {
            return _driver.SetJobInfo(processJobId, materialFormat, startMode, materialOrder, materialInfo, recipeMethod, recipeId, recipeParameterNames, recipeParameterValues);
        }

        public long SetJobInfoWithNumericRecipe(string processJobId, MaterialFormat materialFormat, ProcessStartMode startMode, MaterialOrderMode materialOrder, IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo, RecipeMethod recipeMethod, string recipeId, string[] recipeParameterNames, long[] recipeParameterValues)
        {
            return _driver.SetJobInfoWithNumericRecipe(processJobId, materialFormat, startMode, materialOrder, materialInfo, recipeMethod, recipeId, recipeParameterNames, recipeParameterValues);
        }

        public long SetState(string processJobId, ProcessJobState state)
        {
            return _driver.SetState(processJobId, state);
        }

        public long NotifySettingUpStarted(string processJobId)
        {
            return _driver.NotifySettingUpStarted(processJobId);
        }

        public long NotifySettingUpCompleted(string processJobId)
        {
            return _driver.NotifySettingUpCompleted(processJobId);
        }

        public long Remove(string processJobId)
        {
            return _driver.Remove(processJobId);
        }

        public long RemoveAll()
        {
            return _driver.RemoveAll();
        }

        private void SubscribeDriverEvents()
        {
            _driver.ProcessJobCreated += HandleCreated;
            _driver.ProcessJobStateChanged += HandleStateChanged;
            _driver.ProcessJobDeleted += HandleDeleted;
            _driver.ProcessJobVerifyRequestedByHost += HandleVerifyRequestedByHost;
            _driver.ProcessJobCommandRequestedByHost += HandleCommandRequestedByHost;
            _driver.ProcessJobRecipeVariablesRequestedByHost += HandleRecipeVariablesRequestedByHost;
            _driver.ProcessJobStartMethodRequestedByHost += HandleStartMethodRequestedByHost;
            _driver.ProcessJobMaterialOrderRequestedByHost += HandleMaterialOrderRequestedByHost;
            _driver.ProcessJobManualStartRequired += HandleManualStartRequired;
            _driver.ProcessJobSettingUpRequested += HandleSettingUpRequested;
        }
        private void UnsubscribeDriverEvents()
        {
            _driver.ProcessJobCreated -= HandleCreated;
            _driver.ProcessJobStateChanged -= HandleStateChanged;
            _driver.ProcessJobDeleted -= HandleDeleted;
            _driver.ProcessJobVerifyRequestedByHost -= HandleVerifyRequestedByHost;
            _driver.ProcessJobCommandRequestedByHost -= HandleCommandRequestedByHost;
            _driver.ProcessJobRecipeVariablesRequestedByHost -= HandleRecipeVariablesRequestedByHost;
            _driver.ProcessJobStartMethodRequestedByHost -= HandleStartMethodRequestedByHost;
            _driver.ProcessJobMaterialOrderRequestedByHost -= HandleMaterialOrderRequestedByHost;
            _driver.ProcessJobManualStartRequired -= HandleManualStartRequired;
            _driver.ProcessJobSettingUpRequested -= HandleSettingUpRequested;
        }
        private void HandleCreated(object sender, ProcessJobCreatedEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnCreated(e));
            // 2
        }

        private void HandleStateChanged(object sender, ProcessJobStateChangedEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnStateChanged(e));

            // 3
        }

        private void HandleDeleted(object sender, ProcessJobDeletedEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnDeleted(e));
        }

        private void HandleVerifyRequestedByHost(object sender, ProcessJobVerifyRequestedEventArgs e)
        {
            // 1
            NotifyCallbacks(callback => callback.OnVerifyRequestedByHost(e));
        }

        private void HandleCommandRequestedByHost(object sender, ProcessJobCommandRequestedEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnCommandRequestedByHost(e));
        }

        private void HandleRecipeVariablesRequestedByHost(object sender, ProcessJobRecipeVariableRequestedEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnRecipeVariablesRequestedByHost(e));
        }

        private void HandleStartMethodRequestedByHost(object sender, ProcessJobStartMethodRequestedEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnStartMethodRequestedByHost(e));
        }

        private void HandleMaterialOrderRequestedByHost(object sender, ProcessJobMaterialOrderRequestedEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnMaterialOrderRequestedByHost(e));
        }

        private void HandleManualStartRequired(object sender, ProcessJobManualStartEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnManualStartRequired(e));
        }

        private void HandleSettingUpRequested(object sender, ProcessJobSettingUpEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnSettingUpRequested(e));
        }
    }
}