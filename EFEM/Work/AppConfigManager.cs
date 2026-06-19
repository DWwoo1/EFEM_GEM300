using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using FrameOfSystem3.Functional;
using Define.DefineEnumProject.AppConfig;
using Define.DefineEnumProject.Map;

namespace FrameOfSystem3.Work
{
	public class AppConfigManager
	{
		#region singleton
		static private AppConfigManager _instance = new AppConfigManager();
		static public AppConfigManager Instance { get { return _instance; } }
		#endregion /singleton

		private AppConfigManager()
		{
			Update(NameOf(() => MachineName						), ref MachineName						);
			Update(NameOf(() => Language						), ref Language							);
			Update(NameOf(() => Customer						), ref Customer							);
			Update(NameOf(() => ProcessModuleSimulation			), ref ProcessModuleSimulation			);
            Update(NameOf(() => ProcessType						), ref ProcessType						);
			Update(NameOf(() => ControllerMotion				), ref ControllerMotion					);
			Update(NameOf(() => ControllerDigital				), ref ControllerDigital				);
			Update(NameOf(() => ControllerAnalog				), ref ControllerAnalog					);
			Update(NameOf(() => AtmRobotControllerType			), ref AtmRobotControllerType			);
			Update(NameOf(() => LoadPortControllerType			), ref LoadPortControllerType			);
            Update(NameOf(() => ControllerRfidFoup				), ref ControllerRfidFoup				);
			Update(NameOf(() => ControllerRfidCassette			), ref ControllerRfidCassette			);

            Update(NameOf(() => CountLoadPort				    ), ref CountLoadPort					);
            Update(NameOf(() => CountRobot				        ), ref CountRobot					    );
			Update(NameOf(() => CountRfidFoup				    ), ref CountRfidFoup					);
			Update(NameOf(() => CountRfidCassette				), ref CountRfidCassette				);
			Update(NameOf(() => InterfaceTypePIO				), ref InterfaceTypePIO					);

			Update(NameOf(() => CountProcessModule				), ref CountProcessModule				);

			#region <Location>

			#region <LoadPort>

			#region <로딩모드>
			string[] loadingModes = new string[1];
			Update("LoadPortLoadingModes", ref loadingModes);

			var modes = new List<EFEM.Defines.LoadPort.LoadPortLoadingMode>();
			for (int i = 0; loadingModes != null && i < loadingModes.Length; ++i)
            {
                var mode = loadingModes[i];
                if (string.IsNullOrWhiteSpace(mode))
                    continue;

                if (false == Enum.TryParse(mode, out EFEM.Defines.LoadPort.LoadPortLoadingMode modeEnum))
                    continue;

				modes.Add(modeEnum);
			}
			if (modes.Count == 0)
            {
				LoadPortLoadingModes = new List<EFEM.Defines.LoadPort.LoadPortLoadingMode>() { EFEM.Defines.LoadPort.LoadPortLoadingMode.Foup };
			}
			else
            {
				LoadPortLoadingModes = modes;
			}
			#endregion </로딩모드>

			#region <스테이션에서 활용하기 위한 로드포트별 로딩모드이름 할당>
			Dictionary<int, Dictionary<string, string>> temporaryLpType = new Dictionary<int, Dictionary<string, string>>();			
            for (int i = 0; i < CountLoadPort; ++i)
            {
                Dictionary<string, string> locationName = new Dictionary<string, string>();
                foreach (var item in LoadPortLoadingModes)
                {
                    string name = $"LP{i + 1}.{item}";
                    locationName.Add(item.ToString(), name);
                }
				
                temporaryLpType.Add(i, locationName);
            }
            LoadPortLocationNames = new ReadOnlyDictionary<int, Dictionary<string, string>>(temporaryLpType);
			#endregion </스테이션에서 활용하기 위한 로드포트별 로딩모드이름 할당>

			#endregion </LoadPort>

			#region <Process Module>
			Dictionary<int, string[]> temporaryProcessModuleLocation = new Dictionary<int, string[]>();
			for (int i = 0; i < CountProcessModule; ++i)
            {
                string fieldName = string.Format("ProcessModule_{0}.LocationNames", i);
                string[] locationNames = null;
                Update(fieldName, ref locationNames);
				temporaryProcessModuleLocation.Add(i, locationNames);
			}
			ProcessModuleLocationNames = new ReadOnlyDictionary<int, string[]>(temporaryProcessModuleLocation);
			#endregion </Process Module>

			#endregion </Location>

			#region <Station>
			Dictionary<int, Dictionary<string, string>> temporaryStations
				= new Dictionary<int, Dictionary<string, string>>();

			for(int robot = 0; robot < CountRobot; ++robot)
            {
				Dictionary<string, string> stations = new Dictionary<string, string>();
                foreach (var item in LoadPortLocationNames)
                {
                    foreach (var kvp in item.Value)
                    {
                        if (kvp.Value == null)
                            continue;

						string fieldName = string.Format("Robot_{0}.{1}", robot, kvp.Value);
                        string value = string.Empty;

                        Update(fieldName, ref value);
                        stations.Add(kvp.Value, value);
                    }
                }

                foreach (var item in ProcessModuleLocationNames)
                {
                    if (item.Value == null)
                        continue;

                    int length = item.Value.Length;

                    string value;
                    string fieldName;
                    for (int i = 0; i < length; ++i)
                    {
						fieldName = string.Format("Robot_{0}.{1}", robot, item.Value[i]);
                        value = string.Empty;

                        Update(fieldName, ref value);
						stations.Add(item.Value[i], value);
                    }
                }

				temporaryStations.Add(robot, stations);
            }

            RobotStationNames = new ReadOnlyDictionary<int, Dictionary<string, string>>(temporaryStations);
			#endregion </Station>

			Update(NameOf(() => GemSpecification				), ref GemSpecification					);
			Update(NameOf(() => FoupRfidLotIdAddress			), ref FoupRfidLotIdAddress				);
			Update(NameOf(() => FoupRfidLotIdLength				), ref FoupRfidLotIdLength				);
			Update(NameOf(() => FoupRfidCarrierIdAddress		), ref FoupRfidCarrierIdAddress			);
			Update(NameOf(() => FoupRfidCarrierIdLength			), ref FoupRfidCarrierIdLength			);
			Update(NameOf(() => CassetteRfidLotIdAddress		), ref CassetteRfidLotIdAddress			);
			Update(NameOf(() => CassetteRfidLotIdLength			), ref CassetteRfidLotIdLength			);
			Update(NameOf(() => CassetteRfidCarrierIdAddress	), ref CassetteRfidCarrierIdAddress		);
			Update(NameOf(() => CassetteRfidCarrierIdLength		), ref CassetteRfidCarrierIdLength		);
			Update(NameOf(() => CountFanFilterUnit				), ref CountFanFilterUnit				);
			Update(NameOf(() => UseDifferentialPressureMode		), ref UseDifferentialPressureMode		);
		}

		readonly public string MachineName									= "NONE";
		readonly public Language_.TYPE_LANGUAGE Language 					= Language_.TYPE_LANGUAGE.ENGLISH;
		readonly public EN_CUSTOMER Customer								= EN_CUSTOMER.NONE;
		readonly public bool ProcessModuleSimulation						= false;
		readonly public EN_SECSGEM_SPEC GemSpecification					= EN_SECSGEM_SPEC.SECSGEM;
        readonly public EN_PROCESS_TYPE ProcessType                         = EN_PROCESS_TYPE.NONE;
        readonly public EN_ROBOT_CONTROLLER AtmRobotControllerType			= EN_ROBOT_CONTROLLER.NONE;
		readonly public EN_LOADPORT_CONTROLLER LoadPortControllerType		= EN_LOADPORT_CONTROLLER.NONE;
		readonly public EN_MOTION_CONTROLLER ControllerMotion				= EN_MOTION_CONTROLLER.NONE;
		readonly public EN_DIGITAL_IO_CONTROLLER ControllerDigital			= EN_DIGITAL_IO_CONTROLLER.NONE;
		readonly public EN_ANALOG_IO_CONTROLLER ControllerAnalog			= EN_ANALOG_IO_CONTROLLER.NONE;
		readonly public EN_RFID_CONTROLLER ControllerRfidFoup				= EN_RFID_CONTROLLER.NONE;
		readonly public EN_RFID_CONTROLLER ControllerRfidCassette			= EN_RFID_CONTROLLER.NONE;
        
		readonly public int CountLoadPort                                   = 0;
		readonly public int CountRobot                                      = 0;
		readonly public int CountRfidFoup									= 0;
		readonly public int CountRfidCassette								= 0;
		readonly public int CountProcessModule								= 0;

		readonly public int FoupRfidLotIdAddress							= 0;
		readonly public int FoupRfidLotIdLength								= 0;
		readonly public int FoupRfidCarrierIdAddress						= 0;
		readonly public int FoupRfidCarrierIdLength							= 0;
		readonly public int CassetteRfidLotIdAddress						= 0;
		readonly public int CassetteRfidLotIdLength							= 0;
		readonly public int CassetteRfidCarrierIdAddress					= 0;
		readonly public int CassetteRfidCarrierIdLength						= 0;

		readonly public int CountFanFilterUnit								= 0;
		readonly public bool UseDifferentialPressureMode					= true;
		public readonly IReadOnlyList<EFEM.Defines.LoadPort.LoadPortLoadingMode> LoadPortLoadingModes = null;
		public ReadOnlyDictionary<int, string[]> ProcessModuleLocationNames				= null;
		public ReadOnlyDictionary<int, Dictionary<string, string>> LoadPortLocationNames = null;
        public ReadOnlyDictionary<int, Dictionary<string, string>> RobotStationNames	= null;

		readonly public EN_PIO_INTERFACE_TYPE InterfaceTypePIO				= EN_PIO_INTERFACE_TYPE.NONE;
		//readonly public int CountAligner                                    = 0;
		string NameOf<T>(Expression<Func<T>> expr)
		{
			var body = (MemberExpression)expr.Body;

			return (body.Member.Name);
		}
		void Update(string fieldName, ref string target)
		{
			target = AppConfigControl.GetValue(fieldName, target);
		}
		void Update(string fieldName, ref int target)
		{
			target = AppConfigControl.GetValue(fieldName, target);
		}
		void Update(string fieldName, ref double target)
		{
			target = AppConfigControl.GetValue(fieldName, target);
		}
		void Update(string fieldName, ref bool target)
		{
			target = AppConfigControl.GetValue(fieldName, target);
		}
		void Update<T>(string fieldName, ref T target) where T : IConvertible
		{
			target = AppConfigControl.GetValue<T>(fieldName, target);
		}

		// 배열용(',' 구분)
		void Update<T>(string fieldName, ref T[] target) where T : IConvertible
        {
            target = AppConfigControl.GetValue<T>(fieldName);
        }
    }
}
