using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Configuration;
using System.Linq.Expressions;
using System.Threading;

using FrameOfSystem3.SECSGEM.DefineSecsGem;
using FrameOfSystem3.Functional;

using Define.DefineEnumProject.Mail;
using Define.DefineEnumProject.Task.Global;

namespace FrameOfSystem3.Work
{
	public class ScenarioCirculator
	{
		private ScenarioCirculator()
		{
		}
		private static ScenarioCirculator _instance = null;
		public static ScenarioCirculator Instance
		{
			get
			{
				if (_instance == null)
					_instance = new ScenarioCirculator();

				return _instance;
			}
		}

		public void Initialize(
			Func<EN_SCENARIO, Dictionary<string, string>, bool> funcUpdateParameter,
			Func<EN_SCENARIO, bool> funcExecuteScenraio,
			Action<int, string> funcGenerateAlarm,
			int fdcInterval)
		{
			_updateParameter = funcUpdateParameter;
			_executeScenario = funcExecuteScenraio;
			_generateAlarm = funcGenerateAlarm;

			_postOffice = PostOffice.GetInstance();
			_postOffice.RequestSubscribe(EN_SUBSCRIBER.ScenarioCirculator, (title) => _queueReceiveMail.Enqueue(title));
			_fdcInterval = (uint)fdcInterval;

			Run = false;
			UseOption = false;

			Recipe.Recipe.ParameterChangedNotify += ReceivedParameterChanged;
		}

		Queue<ScenarioData> _queueScenarioList = new Queue<ScenarioData>();
		Func<EN_SCENARIO, Dictionary<string, string>, bool> _updateParameter = null;
		Func<EN_SCENARIO, bool> _executeScenario = null;
		Action<int, string> _generateAlarm = null;
		PostOffice _postOffice = null;
		ConcurrentQueue<EN_MAIL> _queueReceiveMail = new ConcurrentQueue<EN_MAIL>();
		TickCounter_.TickCounter _fdcUpdateCounter = new TickCounter_.TickCounter();
		uint _fdcInterval;

		public bool Run { get; set; }
		public bool UseOption { get; set; }
		public bool IsPossible { get { return Run && UseOption; } }

		public void Execute()
		{
			MailBoxCheck();

			if (false == Run)
				return;

			Circulation();
			FdcUpdate();
		}
		public void SetFdcInterval(int fdcInterval)
		{
			_fdcInterval = (uint)fdcInterval;
			if (_fdcInterval < 1000) _fdcInterval = 1000; // 최소값 1sec
			_fdcUpdateCounter.SetTickCount(_fdcInterval);
		}

		private void Circulation()
		{
			if (_queueScenarioList.Count < 1)
				return;

			ScenarioData scenarioData = _queueScenarioList.Peek();
			if (false == scenarioData.UpdatedData)
			{
				if (false == _updateParameter(scenarioData.Scenario, scenarioData.Datas))
				{
					// alarm 띄우니까 dequeue하고 종료
					_queueScenarioList.Dequeue();

					_generateAlarm((int)ALARM_GLOBAL.WrongScenarioCirculation
						, string.Format("parameter update\nscenario : {0}"
						, scenarioData.Scenario.ToString()));
					return;
				}

				scenarioData.UpdatedData = true;
				return;
			}

			if (false == scenarioData.Finished)
			{
				scenarioData.Finished = _executeScenario(scenarioData.Scenario);
				return;
			}
			else
			{
				_queueScenarioList.Dequeue();
				return;
			}
		}
		private void MailBoxCheck()
		{
			while (_queueReceiveMail.Count > 0)
			{
				EN_MAIL receivedMail;
				_queueReceiveMail.TryDequeue(out receivedMail);

				List<Mail> mailList;
				switch (receivedMail)
				{
					case EN_MAIL.SendScenario:
						#region
						if (false == _postOffice.GetMail(EN_SUBSCRIBER.ScenarioCirculator, receivedMail, out mailList) || mailList == null)
						{
							Console.WriteLine(string.Format("ScenarioCirculator:GetMail failed: {0}", receivedMail.ToString()));
							return;
						}

						foreach (var mail in mailList)
						{
							if (mail.Content.Count != 2) return;
							var scenario = (EN_SCENARIO)mail.Content[0];
							var datas = (Dictionary<string, string>)mail.Content[1];

							if (false == IsPossible)
								return;
							
							_queueScenarioList.Enqueue(new ScenarioData(scenario, datas));
						}
						break;
						#endregion
					case EN_MAIL.ScenarioCurculatorRun:
						#region
						{
							if (false == _postOffice.GetMail(EN_SUBSCRIBER.ScenarioCirculator, receivedMail, out mailList) || mailList == null)
							{
								Console.WriteLine(string.Format("ScenarioCirculator:GetMail failed: {0}", receivedMail.ToString()));
								return;
							}

							bool b = true;
							foreach (var mail in mailList)
							{
								b &= (bool)mail.Content[0];
							}
							Run = b;

							var recipe = Recipe.Recipe.GetInstance();
							if (b)
							{
								UseOption = false;
								//UseOption = recipe.GetValue(Recipe.EN_RECIPE_TYPE.COMMON, Recipe.PARAM_COMMON.Use_Secsgem.ToString(), false);
							}
							else
							{
								//recipe.SetValue(Recipe.EN_RECIPE_TYPE.COMMON, Recipe.PARAM_COMMON.Use_Secsgem.ToString(), false.ToString());
							}
							break;
						}
						#endregion
					case EN_MAIL.ScenarioCurculatorUse:
						#region
						{
							if (false == _postOffice.GetMail(EN_SUBSCRIBER.ScenarioCirculator, receivedMail, out mailList) || mailList == null)
							{
								Console.WriteLine("ScenarioCirculator: GetMail failed");
								return;
							}

							bool b = true;
							foreach (var mail in mailList)
							{
								b &= (bool)mail.Content[0];
							}
							UseOption = b;
							SECSGEM.ScenarioOperator.Instance.SetUse(b);
							break;
						}
						#endregion
					default: return;
				}
			}
		}
		private void FdcUpdate()
		{
			if (false == _fdcUpdateCounter.IsTickOver(true))
				return;

			_queueScenarioList.Enqueue(new ScenarioData(EN_SCENARIO.FdcUpdate, null));

			_fdcUpdateCounter.SetTickCount(_fdcInterval);
		}
		private void ReceivedParameterChanged(bool result, List<Recipe.Recipe.ParameterItem> changedList)
		{
			if (false == result)
				return;

			var ppDatas = new Dictionary<string, string>();
			var scnearioOperator = SECSGEM.ScenarioOperator.Instance;
			foreach (var item in changedList)
			{
				switch(item.Type)
				{
					case Recipe.EN_RECIPE_TYPE.PROCESS:
						{
							string key = string.Format("{0}.{1}", item.TaskName, item.ParameterName);
							if (ppDatas.ContainsKey(key))
								ppDatas.Remove(key);

							ppDatas.Add(key, item.Value);
						}
						break;
					case Recipe.EN_RECIPE_TYPE.COMMON:
						{
							Recipe.PARAM_COMMON enItem;
							if (false == Enum.TryParse(item.ParameterName, out enItem))
								continue;

							scnearioOperator.UpdateCommonParameters(enItem, item.Value);
						}
						break;
					case Recipe.EN_RECIPE_TYPE.EQUIPMENT:
						{
							Recipe.PARAM_EQUIPMENT enItem;
							if (false == Enum.TryParse(item.ParameterName, out enItem))
								continue;

							scnearioOperator.UpdateMachineParameters(enItem, item.Value);
						}
						break;
				}
			}

			if (ppDatas.Count > 0)
			{
				_postOffice.SendMail(EN_SUBSCRIBER.ScenarioCirculator, EN_MAIL.SendScenario
					, EN_SCENARIO.RecipeParameterChanged, ppDatas);
			}
		}

		class ScenarioData
		{
			public ScenarioData(EN_SCENARIO scenario, Dictionary<string, string> datas)
			{
				Scenario = scenario;
				Datas = datas;
				UpdatedData = false;
				Finished = false;
			}

			public readonly EN_SCENARIO Scenario;
			public readonly Dictionary<string, string> Datas;
			public bool UpdatedData { get; set; }
			public bool Finished { get; set; }
		}
	}
}
