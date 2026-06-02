using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameOfSystem3.SECSGEM
{
    public static class ScenarioOperatorLazyConfigFactory
    {
        public static bool TryCreate(
            Define.DefineEnumProject.AppConfig.EN_CUSTOMER customer,
            out ScenarioOperatorLazyConfig config)
        {
            config = null;

            switch (customer)
            {
                case Define.DefineEnumProject.AppConfig.EN_CUSTOMER.S_TP:
                    config = CreateTp(customer);
                    return true;

                case Define.DefineEnumProject.AppConfig.EN_CUSTOMER.S_NRD:
                    config = CreateNrd(customer);
                    return true;

                case Define.DefineEnumProject.AppConfig.EN_CUSTOMER.S_NRD_300:
                    config = CreateNrd300(customer);
                    return true;

                default:
                    return false;
            }
        }

        private static ScenarioOperatorLazyConfig CreateTp(
            Define.DefineEnumProject.AppConfig.EN_CUSTOMER customer)
        {
            string cfgPath = CreateCfgPath(customer);
            string recipePath = CreateRecipePath();

            return new ScenarioOperatorLazyConfig(
                delegate
                {
                    return new Scenario.ProcessingScenarioPWA500BIN_TP();
                },
                delegate
                {
                    return new SecsGemDll.XGemPro300WithWCF(1, new int[] { 4 });
                },
                cfgPath,
                recipePath);
        }

        private static ScenarioOperatorLazyConfig CreateNrd(
            Define.DefineEnumProject.AppConfig.EN_CUSTOMER customer)
        {
            string cfgPath = CreateCfgPath(customer);
            string recipePath = CreateRecipePath();

            return new ScenarioOperatorLazyConfig(
                delegate
                {
                    return new Scenario.ProcessingScenarioPWA500W_NRD();
                },
                delegate
                {
                    return new SecsGemDll.XGemPro300WithWCF(1, new int[] { 6 });
                },
                cfgPath,
                recipePath);
        }

        private static ScenarioOperatorLazyConfig CreateNrd300(
            Define.DefineEnumProject.AppConfig.EN_CUSTOMER customer)
        {
            string cfgPath = CreateCfgPath(customer);
            string recipePath = CreateRecipePath();

            return new ScenarioOperatorLazyConfig(
                delegate
                {
                    return new Scenario.ProcessingScenarioPWA500W_NRD_300();
                },
                delegate
                {
                    return new SecsGemDll.XGemPro300WithWCF(1, new int[] { 6 });
                },
                cfgPath,
                recipePath);
        }
        private static string CreateCfgPath(
            Define.DefineEnumProject.AppConfig.EN_CUSTOMER customer)
        {
            return string.Format(
                @"{0}{1}\{2}",
                DefineSecsGem.PATH.FILE_PATH_CFG,
                customer,
                Work.AppConfigManager.Instance.ProcessType.ToString());
        }

        private static string CreateRecipePath()
        {
            return string.Format(
                @"{0}\EFEM\RMS",
                Define.DefineConstant.FilePath.FILEPATH_RECIPE);
        }
    }
}
