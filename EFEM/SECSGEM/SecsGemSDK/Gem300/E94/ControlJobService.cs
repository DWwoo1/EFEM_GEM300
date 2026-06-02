using System;
using System.Collections.Generic;

using FrameOfSystem3.SECSGEM.SecsGemSDK.Gem300;
using FrameOfSystem3.SECSGEM.DefineSecsGem;

using EFEM.Defines.Job;

namespace FrameOfSystem3.SECSGEM
{
    internal sealed class ControlJobService : IControlJobService
    {
        private IControlJobDriver _driver;
        private readonly object _driverLock = new object();
        private readonly object _callbackLock = new object();
        private readonly List<IControlJobServiceCallback> _callbacks = new List<IControlJobServiceCallback>();

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

        public void AttachDriver(IControlJobDriver driver)
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

        private IControlJobDriver GetDriver()
        {
            lock (_driverLock)
            {
                if (_driver == null)
                    throw new InvalidOperationException("ControlJob driver is not attached.");

                return _driver;
            }
        }

        public void RegisterCallback(IControlJobServiceCallback callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            lock (_callbackLock)
            {
                if (_callbacks.Contains(callback))
                    return;

                _callbacks.Add(callback);
            }
        }
        public void UnregisterCallback(IControlJobServiceCallback callback)
        {
            if (callback == null)
                return;

            lock (_callbackLock)
            {
                _callbacks.Remove(callback);
            }
        }
        private void NotifyCallbacks(Action<IControlJobServiceCallback> notify)
        {
            foreach (var item in _callbacks)
            {
                notify(item);
            }
        }

        public long Create(string controlJobId, ControlJobStartMode startMode, string[] processJobIds)
        {
            return _driver.Create(controlJobId, startMode, processJobIds);
        }

        public long RequestJob(string controlJobId)
        {
            return _driver.RequestJob(controlJobId);
        }

        public long RequestAllJobIds()
        {
            return _driver.RequestAllJobIds();
        }

        public long RequestSelect(string controlJobId)
        {
            return _driver.RequestSelect(controlJobId);
        }

        public long RequestHeadOfQueue(string controlJobId)
        {
            return _driver.RequestHeadOfQueue(controlJobId);
        }

        public long RequestHeadOfQueueInfo()
        {
            return _driver.RequestHeadOfQueueInfo();
        }

        public long RequestCommand(string controlJobId, ControlJobCommand command, string commandParameterName, string commandParameterValue)
        {
            return _driver.RequestCommand(controlJobId, command, commandParameterName, commandParameterValue);
        }

        public long AcknowledgeVerify(long messageId, string controlJobId, long result, long[] errorCodes, string[] errorTexts)
        {
            return _driver.AcknowledgeVerify(messageId, controlJobId, result, errorCodes, errorTexts);
        }

        public long AcknowledgeCommand(long messageId, string controlJobId, ControlJobCommand command, long result, long[] errorCodes, string[] errorTexts)
        {
            return _driver.AcknowledgeCommand(messageId, controlJobId, command, result, errorCodes, errorTexts);
        }

        public long SetJobInfo(string controlJobId, ControlJobState state, ControlJobStartMode startMode, string[] processJobIds)
        {
            return _driver.SetJobInfo(controlJobId, state, startMode, processJobIds);
        }
        public long Remove(string controlJobId)
        {
            return _driver.Remove(controlJobId);
        }

        public long RemoveAll()
        {
            return _driver.RemoveAll();
        }

        private void SubscribeDriverEvents()
        {
            _driver.ControlJobCreated += HandleCreated;
            _driver.ControlJobStateChanged += HandleStateChanged;
            _driver.ControlJobDeleted += HandleDeleted;
            _driver.ControlJobVerifyRequestedByHost += HandleVerifyRequestedByHost;
            _driver.ControlJobCommandRequestedByHost += HandleCommandRequestedByHost;
            _driver.ControlJobManualStartRequired += HandleManualStartRequired;
            _driver.ControlJobHeadOfQueueChanged += HandleHeadOfQueueChanged;
        }
        private void UnsubscribeDriverEvents()
        {
            _driver.ControlJobCreated -= HandleCreated;
            _driver.ControlJobStateChanged -= HandleStateChanged;
            _driver.ControlJobDeleted -= HandleDeleted;
            _driver.ControlJobVerifyRequestedByHost -= HandleVerifyRequestedByHost;
            _driver.ControlJobCommandRequestedByHost -= HandleCommandRequestedByHost;
            _driver.ControlJobManualStartRequired -= HandleManualStartRequired;
            _driver.ControlJobHeadOfQueueChanged -= HandleHeadOfQueueChanged;
        }
        private void HandleCreated(object sender, ControlJobCreatedEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnCreated(e));
        }

        private void HandleStateChanged(object sender, ControlJobStateChangedEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnStateChanged(e));
        }

        private void HandleDeleted(object sender, ControlJobDeletedEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnDeleted(e));
        }

        private void HandleVerifyRequestedByHost(object sender, ControlJobVerifyRequestedEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnVerifyRequestedByHost(e));
        }

        private void HandleCommandRequestedByHost(object sender, ControlJobCommandRequestedEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnCommandRequestedByHost(e));
        }

        private void HandleManualStartRequired(object sender, ControlJobManualStartEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnManualStartRequired(e));
        }

        private void HandleHeadOfQueueChanged(object sender, ControlJobHoqChangedEventArgs e)
        {
            NotifyCallbacks(callback => callback.OnHeadOfQueueChanged(e));
        }
    }
}