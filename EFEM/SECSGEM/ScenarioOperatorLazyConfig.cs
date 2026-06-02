using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameOfSystem3.SECSGEM
{
    public sealed class ScenarioOperatorLazyConfig
    {
        public Func<ProcessingScenario> ScenarioFactory { get; private set; }
        public Func<SecsGem> DriverFactory { get; private set; }

        public string CfgPath { get; private set; }
        public string RecipePath { get; private set; }

        public ScenarioOperatorLazyConfig(
            Func<ProcessingScenario> scenarioFactory,
            Func<SecsGem> driverFactory,
            string cfgPath,
            string recipePath)
        {
            if (scenarioFactory == null)
            {
                throw new ArgumentNullException("scenarioFactory");
            }

            if (driverFactory == null)
            {
                throw new ArgumentNullException("driverFactory");
            }

            if (string.IsNullOrWhiteSpace(cfgPath))
            {
                throw new ArgumentException("cfgPath is null or empty.", "cfgPath");
            }

            if (string.IsNullOrWhiteSpace(recipePath))
            {
                throw new ArgumentException("recipePath is null or empty.", "recipePath");
            }

            ScenarioFactory = scenarioFactory;
            DriverFactory = driverFactory;
            CfgPath = cfgPath;
            RecipePath = recipePath;
        }
    }
}
