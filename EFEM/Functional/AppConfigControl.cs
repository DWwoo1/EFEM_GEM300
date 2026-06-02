using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace FrameOfSystem3.Functional
{
	class AppConfigControl
	{
		public static string GetValue(string key, string defaultValue)
		{
			try
			{
				if (ConfigurationManager.AppSettings[key] == null)
					return defaultValue;

				return ConfigurationManager.AppSettings[key];
			}
			catch
			{
				return defaultValue;
			}
		}
		public static int GetValue(string key, int defaultValue)
		{
			int result;
			if (false == int.TryParse(GetValue(key, defaultValue.ToString()), out result))
				return defaultValue;

			return result;
		}
		public static double GetValue(string key, double defaultValue)
		{
			double result;
			if (false == double.TryParse(GetValue(key, defaultValue.ToString()), out result))
				return defaultValue;

			return result;
		}
		public static bool GetValue(string key, bool defaultValue)
		{
			bool result;
			if (false == bool.TryParse(GetValue(key, defaultValue.ToString()), out result))
				return defaultValue;

			return result;
		}
		public static string GetValue(string key, string defaultValue, List<string> candidate)
		{
			string result = GetValue(key, defaultValue);

			if (false == candidate.Contains(result))
				return defaultValue;

			return result;
		}

		/// <summary>
		/// 1000회 반복수행에 1ms정도 걸림.
		/// </summary>
        public static T GetValue<T>(string key, T defaultValue, Func<T, bool> funcCheckSum = null) where T : IConvertible
		{
			try
			{
				string readValue = ConfigurationManager.AppSettings[key];
				if (readValue == null)
					return defaultValue;

				T result ;
				if (typeof(T).IsEnum)
					result = (T)Enum.Parse(typeof(T), readValue);
				else
					result = (T)Convert.ChangeType(readValue, typeof(T));

				if (funcCheckSum != null && false == funcCheckSum(result))
					return defaultValue;

				return result;
			}
			catch { return defaultValue; }
		}

        public static T[] GetValue<T>(string key, Func<T, bool> funcCheckSum = null) where T : IConvertible
        {
            try
            {
                string readValue = ConfigurationManager.AppSettings[key];
                if (readValue == null)
                    return null;

				string[] temporaryValue = readValue.Split(',');
                T[] result = new T[temporaryValue.Length];

				for(int i = 0; i < temporaryValue.Length; ++i)
                {
					string splittedValue = temporaryValue[i];

					if (typeof(T).IsEnum)
						result[i] = (T)Enum.Parse(typeof(T), splittedValue);
					else
						result[i] = (T)Convert.ChangeType(splittedValue, typeof(T));

					
					if (funcCheckSum != null && false == funcCheckSum(result[i]))
						return null;
                }


                return result;
            }
            catch { return null; }
        }
    }
}
