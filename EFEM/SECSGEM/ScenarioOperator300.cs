using FrameOfSystem3.SECSGEM.DefineSecsGem;
using FrameOfSystem3.SECSGEM.Scenario;
using FrameOfSystem3.SECSGEM.SecsGemSDK.Gem300;

namespace FrameOfSystem3.SECSGEM
{
    public sealed class ScenarioOperator300 : ScenarioOperator
    {
        private static ScenarioOperator300 _instance;

        private SecsGem300 _gem300Driver;
        private bool _gem300Initialized;

        public static new ScenarioOperator300 Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ScenarioOperator300();

                return _instance;
            }
        }

        public ICarrierService Carrier { get; private set; }
        public IProcessJobService ProcessJob { get; private set; }
        public IControlJobService ControlJob { get; private set; }
        public ISubstrateService Substrate { get; private set; }

        private ScenarioOperator300()
            : base(Communicator.SecsGemHandler.Instance)
        {
        }

        public bool Initialize(
            ProcessingScenario scenario,
            SecsGem300 driver,
            string cfgPath,
            string recipePath)
        {
            if (driver == null)
                return false;

            _gem300Driver = driver;

            if (!base.Initialize(scenario, driver, cfgPath, recipePath))
                return false;

            BuildGem300Services(driver);

            _gem300Initialized = true;
            return true;
        }

        public override void Exit()
        {
            _gem300Initialized = false;
            _gem300Driver = null;

            Carrier = null;
            ProcessJob = null;
            ControlJob = null;
            Substrate = null;

            base.Exit();
        }

        public override bool Reset()
        {
            if (!base.Reset())
                return false;

            return _gem300Initialized;
        }

        private void BuildGem300Services(SecsGem300 driver)
        {
            Carrier = new CarrierService(driver.CmsDriver);
            ProcessJob = new ProcessJobService(driver.PjDriver);
            ControlJob = new ControlJobService(driver.CjDriver);
            Substrate = new SubstrateService(driver.StsDriver);
        }
    }
}