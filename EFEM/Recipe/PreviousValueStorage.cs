using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XmlWriter_;

namespace FrameOfSystem3.Recipe
{
	class PreviousValueStorage
	{
		#region singleton
		private static PreviousValueStorage _instance = new PreviousValueStorage();
		public static PreviousValueStorage Instance { get { return _instance; } }
		private PreviousValueStorage()
		{
			FILE_PATH = System.IO.Path.Combine(Define.DefineConstant.FilePath.FILEPATH_RECIPE, "PreviousValueStorage");
		}
		public void Init()
		{
			_recipe = Recipe.GetInstance();
			_storage = new Dictionary<EN_RECIPE_TYPE, XmlWriter>();
			_storage.Add(EN_RECIPE_TYPE.COMMON, new XmlWriter("Common", FILE_PATH));
			_storage.Add(EN_RECIPE_TYPE.EQUIPMENT, new XmlWriter("Equipment", FILE_PATH));

			_storage[EN_RECIPE_TYPE.COMMON].Load();
			_storage[EN_RECIPE_TYPE.EQUIPMENT].Load();

			Recipe.ParameterChangedNotify += Apply;
			Recipe.ParameterChangeConfirm += Booking;
		}
		#endregion singleton

		#region field
		readonly string FILE_PATH;
		Recipe _recipe = null;
		Dictionary<EN_RECIPE_TYPE, XmlWriter> _storage = null;
		Dictionary<string, Dictionary<string, string>> _bookingList = new Dictionary<string, Dictionary<string, string>>();
		#endregion field

		#region method
		private XmlWriter GetStorage(EN_RECIPE_TYPE type)
		{
			switch (type)
			{
				case EN_RECIPE_TYPE.COMMON:
				case EN_RECIPE_TYPE.EQUIPMENT:
					// 생성자에서 만들기 때문에 없는 경우는 배제
					return _storage[type];

				case EN_RECIPE_TYPE.PROCESS:
					{
						string currentRecipe = _recipe.GetProcessFileNameWithoutExtension();
						if (false == _storage.ContainsKey(type))
						{
							_storage.Add(type, new XmlWriter(currentRecipe, FILE_PATH));
							_storage[type].Load();
						}
						else if (false == _storage[type].GetValue("Subject", "").Equals(currentRecipe))
						{
							// null이거나 현재 recipe와 다를 경우 교체하고 load 시도
							_storage[type] = new XmlWriter(currentRecipe, FILE_PATH);
							_storage[type].Load();
						}
					}
					break;
				default: return null;
			}

			return _storage[type];
		}
		System.Threading.Tasks.Task<bool> Booking(List<Recipe.ParameterItem> list, TaskCompletionSource<bool> ayncWaitResult)
		{
			_bookingList.Clear();

			var recipe = Recipe.GetInstance();
			foreach(var item in list)
			{
				string key;
				switch(item.Type)
				{
					case EN_RECIPE_TYPE.COMMON:
					case EN_RECIPE_TYPE.EQUIPMENT:
						key = item.Type.ToString();
						break;
					case EN_RECIPE_TYPE.PROCESS:
						key = item.TaskName;
						break;
					default:
						continue;
				}

				if (false == _bookingList.ContainsKey(key))
					_bookingList.Add(key, new Dictionary<string, string>());

				if (_bookingList[key].ContainsKey(item.ParameterName))
					_bookingList[key].Remove(item.ParameterName);

				string prevValue;
				switch (item.Type)
				{
					case EN_RECIPE_TYPE.COMMON:
					case EN_RECIPE_TYPE.EQUIPMENT:
						prevValue = recipe.GetValue(item.Type, item.ParameterName, "");
						break;
					case EN_RECIPE_TYPE.PROCESS:
						prevValue = recipe.GetValue(key, item.ParameterName, "");
						break;
					default: continue;
				}

				_bookingList[key].Add(item.ParameterName, prevValue);
			}

			ayncWaitResult.SetResult(true);
			return ayncWaitResult.Task;
		}
		private void Apply(bool result, List<Recipe.ParameterItem> changedList)
		{
			DateTime now = DateTime.Now;
			HashSet<EN_RECIPE_TYPE> changedType = new HashSet<EN_RECIPE_TYPE>();
			foreach (var item in changedList)
			{
				string key;
				switch (item.Type)
				{
					case EN_RECIPE_TYPE.COMMON:
					case EN_RECIPE_TYPE.EQUIPMENT:
						key = item.Type.ToString();
						break;
					case EN_RECIPE_TYPE.PROCESS:
						key = item.TaskName;
						break;
					default: continue;
				}

				if (false == _bookingList.ContainsKey(key))
					continue;

				if (false == _bookingList[key].ContainsKey(item.ParameterName))
					continue;

				var storage = GetStorage(item.Type);
				if (storage == null)
					continue;

				changedType.Add(item.Type);
				storage.SetValue("Time", now.ToString("yyyy-MM-dd HH:mm:ss.fff"), key, item.ParameterName);
				storage.SetValue("Value", _bookingList[key][item.ParameterName], key, item.ParameterName);	
			}

			_bookingList.Clear();

			foreach (var type in changedType)
			{
				GetStorage(type).Save();
			}
		}
		#endregion method

		#region interface
		public void Get(EN_RECIPE_TYPE type, string taskName, string parameterName, out string value, out string time, string defaultValue = "")
		{
			string key;
			switch (type)
			{
				case EN_RECIPE_TYPE.COMMON:
				case EN_RECIPE_TYPE.EQUIPMENT:
					key = type.ToString();
					break;
				case EN_RECIPE_TYPE.PROCESS:
					key = taskName;
					break;
				default:
					value = defaultValue;
					time = string.Empty;
					return;
			}

			var storage = GetStorage(type);
			if (storage == null)
			{
				value = defaultValue;
				time = string.Empty;
				return;
			}

			value = storage.GetValue("Value", defaultValue, key, parameterName);
			time = storage.GetValue("Time", string.Empty, key, parameterName);
		}

		public void FileRename(string prevName, string nextName)
		{
			try
			{
				var xml = new XmlWriter(prevName, FILE_PATH);
				xml.Load();
				xml.ChangeSubject(nextName);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return;
			}
		}
		public void FileRemove(string recipeName)
		{
			Functional.FunctionsETC.FileDelete(System.IO.Path.Combine(FILE_PATH, recipeName + ".xml"));
		}
		#endregion interface
	}
}
