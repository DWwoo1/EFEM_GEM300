using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EFEM.Defines.Job;
using EFEM.Defines.CarrierManagement;
using EFEM.Defines.MaterialTracking;

using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace FrameOfSystem3.SECSGEM
{
    public interface IGem300ScenarioService
    {
        ICarrierService Carrier { get; }
        IProcessJobService ProcessJob { get; }
        IControlJobService ControlJob { get; }
        ISubstrateService Substrate { get; }

        bool IsDriverAttached { get; }

        void RegisterCarrierServiceCallback(
            string locationName,
            ICarrierServiceCallback callback);

        void RegisterProcessJobServiceCallback(
            IProcessJobServiceCallback callback);

        void RegisterControlJobServiceCallback(
            IControlJobServiceCallback callback);

        void RegisterSubstrateServiceCallback(
            ISubstrateServiceCallback callback);

        void AttachDriver(SecsGem300 driver);
        void DetachDriver();
    }

    public sealed class Gem300ScenarioService : IGem300ScenarioService
    {
        #region <Fields>
        private readonly object _syncRoot = new object();

        private readonly ConcurrentDictionary<string, ICarrierServiceCallback> _carrierCallbacks =
            new ConcurrentDictionary<string, ICarrierServiceCallback>();

        private readonly List<IProcessJobServiceCallback> _processJobCallbacks =
            new List<IProcessJobServiceCallback>();

        private readonly List<IControlJobServiceCallback> _controlJobCallbacks =
            new List<IControlJobServiceCallback>();

        private SecsGem300 _driver;

        private readonly ICarrierService _carrier = new CarrierService();
        private readonly IProcessJobService _processJob = new ProcessJobService();
        private readonly IControlJobService _controlJob = new ControlJobService();
        private readonly ISubstrateService _substrate = new SubstrateService();
        #endregion </Fields>

        #region <Properties>
        public ICarrierService Carrier
        {
            get
            {
                lock (_syncRoot)
                {
                    if (_carrier == null)
                    {
                        throw new InvalidOperationException(
                            "ScenarioGem300Service driver is not attached. Carrier is not available.");
                    }

                    return _carrier;
                }
            }
        }

        public IProcessJobService ProcessJob
        {
            get
            {
                lock (_syncRoot)
                {
                    if (_processJob == null)
                    {
                        throw new InvalidOperationException(
                            "ScenarioGem300Service driver is not attached. ProcessJob is not available.");
                    }

                    return _processJob;
                }
            }
        }

        public IControlJobService ControlJob
        {
            get
            {
                lock (_syncRoot)
                {
                    if (_controlJob == null)
                    {
                        throw new InvalidOperationException(
                            "ScenarioGem300Service driver is not attached. ControlJob is not available.");
                    }

                    return _controlJob;
                }
            }
        }

        public ISubstrateService Substrate
        {
            get
            {
                lock (_syncRoot)
                {
                    if (_substrate == null)
                    {
                        throw new InvalidOperationException(
                            "ScenarioGem300Service driver is not attached. Substrate is not available.");
                    }

                    return _substrate;
                }
            }
        }

        public bool IsDriverAttached
        {
            get
            {
                lock (_syncRoot)
                {
                    return _driver != null
                        && _carrier != null
                        && _processJob != null
                        && _controlJob != null
                        && _substrate != null;
                }
            }
        }
        #endregion </Properties>

        #region <Methods>
        public void RegisterCarrierServiceCallback(
            string locationName,
            ICarrierServiceCallback callback)
        {
            if (string.IsNullOrWhiteSpace(locationName))
                return;

            if (callback == null)
                return;

            _carrier.RegisterCallback(locationName, callback);
        }

        public void RegisterProcessJobServiceCallback(
            IProcessJobServiceCallback callback)
        {
            if (callback == null)
                return;

            _processJob.RegisterCallback(callback);
        }

        public void RegisterControlJobServiceCallback(
            IControlJobServiceCallback callback)
        {
            if (callback == null)
                return;

            _controlJob.RegisterCallback(callback);
        }

        public void RegisterSubstrateServiceCallback(
            ISubstrateServiceCallback callback)
        {
            if (callback == null)
                return;

            _substrate.RegisterCallback(callback);
        }

        public void AttachDriver(SecsGem300 driver)
        {
            if (driver == null)
                throw new ArgumentNullException("driver");

            var cmsDriver = driver.CmsDriver;
            var pjDriver = driver.PjDriver;
            var cjDriver = driver.CjDriver;
            var stsDriver = driver.StsDriver;

            if (cmsDriver == null)
                throw new InvalidOperationException("Carrier driver is not initialized.");

            if (pjDriver == null)
                throw new InvalidOperationException("ProcessJob driver is not initialized.");

            if (cjDriver == null)
                throw new InvalidOperationException("ControlJob driver is not initialized.");

            if (stsDriver == null)
                throw new InvalidOperationException("Substrate driver is not initialized.");

            lock (_syncRoot)
            {
                if (object.ReferenceEquals(_driver, driver)
                    && _carrier.IsDriverAttached
                    && _processJob.IsDriverAttached
                    && _controlJob.IsDriverAttached
                    && _substrate.IsDriverAttached)
                {
                    return;
                }

                DetachDriverCore();

                _driver = driver;

                _carrier.AttachDriver(cmsDriver);
                _processJob.AttachDriver(pjDriver);
                _controlJob.AttachDriver(cjDriver);
                _substrate.AttachDriver(stsDriver);
            }
        }

        public void DetachDriver()
        {
            lock (_syncRoot)
            {
                DetachDriverCore();
            }
        }

        private void DetachDriverCore()
        {
            _carrier.DetachDriver();
            _processJob.DetachDriver();
            _controlJob.DetachDriver();
            _substrate.DetachDriver();

            _driver = null;
        }
        #endregion </Methods>
    }
}
