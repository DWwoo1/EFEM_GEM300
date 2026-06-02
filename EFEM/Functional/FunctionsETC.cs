using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

using FrameOfSystem3.Recipe;
using FrameOfSystem3;

namespace FrameOfSystem3.Functional
{
    class FunctionsETC
	{
		#region <CALCULATE>
		/// <summary>
        /// 2019.11.21 by junho [ADD] Target 값이 Min/Max Area 안에 들어와 있는지 여부 반환
        /// (Min, Max 포함)
        /// </summary>
		public static bool IsInRangeMinMax(double target, double min, double max, bool isInclusion = true)
		{
			double realMin = Math.Min(min, max), realMax = Math.Max(min, max);

			if (isInclusion)
			{
				if (realMin <= target && target <= realMax) return true;
			}
			else
			{
				if (realMin < target && target < realMax) return true;
			}
			return false;
		}
		/// <summary>
		/// 2019.11.21 by junho [ADD] Target 값이 Min/Max Area 안에 들어와 있는지 여부 반환
		/// (Min, Max 포함)
		/// </summary>
		public static bool IsInRangeMinMax(int target, int min, int max, bool bMinInclusion = true, bool bMaxInclusion = true)
		{
			int realMin = Math.Min(min, max), realMax = Math.Max(min, max);
			if (realMin <= target && target <= realMax) return true;

			return false;
		}
		/// <summary>
		/// 2022.06.29 by junho [ADD] +- 범위의 공차로 min, max 만들어서 IsInRangeMinMax 반환
		/// </summary>
		public static bool IsInTolerance(double target, double threshold, double tolerance)
		{
			double min = threshold + Math.Abs(tolerance) * -1;
			double max = threshold + Math.Abs(tolerance);

			return IsInRangeMinMax(target, min, max);
		}
        /// <summary>
        /// Range A와 Range B의 겹침 여부 반환
        /// </summary>
        public static bool IsCrossRange(DPointXY ptRangeA, DPointXY ptRangeB)
        {
            double AMax = Math.Max(ptRangeA.x, ptRangeA.y);
			double AMin = Math.Min(ptRangeA.x, ptRangeA.y);
			double BMax = Math.Max(ptRangeB.x, ptRangeB.y);
			double BMin = Math.Min(ptRangeB.x, ptRangeB.y);

            if (AMax < BMin && AMax < BMax) return false;                   // A-----A B-----B
            if (AMin > BMin && AMin > BMax) return false;                   // B-----B A-----A

            return true;
        }
		/// <summary>
		/// Line A와 Line B의 겹침 여부 반환
		/// </summary>
		public static bool IsCrossLine(double startLineA, double endLineA, double startLineB, double endLineB, bool isInclusion = true)
		{
			double AMax = Math.Max(startLineA, endLineA);
			double AMin = Math.Min(startLineA, endLineA);
			double BMax = Math.Max(startLineB, endLineB);
			double BMin = Math.Min(startLineB, endLineB);

			if (isInclusion)
			{
				if (AMax < BMin && AMax < BMax) return false;                   // A-----A B-----B
				if (AMin > BMin && AMin > BMax) return false;                   // B-----B A-----A
			}
			else
			{
				if (AMax <= BMin && AMax <= BMax) return false;                   // A-----A B-----B
				if (AMin >= BMin && AMin >= BMax) return false;                   // B-----B A-----A
			}
			return true;
		}
		/// <summary>
		/// Vactor와 Area의 겹침 여부 반환
		/// </summary>
		public static bool IsRouteInArea(DPointXY vactorStart, DPointXY vactorEnd, DRectangle area, bool isInclusion = true)
		{
			bool isCrossX = IsCrossLine(vactorStart.x, vactorEnd.x, area.left, area.right, isInclusion);
			bool isCrossY = IsCrossLine(vactorStart.y, vactorEnd.y, area.top, area.bottom, isInclusion);

			return isCrossX && isCrossY;
		}
		/// <summary>
		/// Target Point가 threshold Area안에 들어있는지 여부 반환
		/// </summary>
		public static bool IsInRangeArea(DPointXY target, DRectangle area, bool isInclusion = true)
		{
			bool resultX = IsInRangeMinMax(target.x, area.left, area.right, isInclusion);
			bool resultY = IsInRangeMinMax(target.y, area.top, area.bottom, isInclusion);

			return resultX && resultY;
		}

        /// <summary>
        /// 2020.06.24 by junho [ADD]
        /// 두 Double의 중앙값을 반환한다.
        /// 내부에서 High Low 구분함
        /// </summary>
        public static double GetMedianDtoD(double dValue1, double dValue2, bool bABS = false)
        {
            double dHigh, dLow, dResult;
            if(dValue1 == dValue2) return dValue1;
            if(dValue1 > dValue2)
            {
                dHigh = dValue1;
                dLow = dValue2;
            }
            else
            {
                dHigh = dValue2;
                dLow = dValue1;
            }

            if (dHigh >= 0 && dLow >= 0)
            {
                dResult = (dHigh - dLow) / 2;
            }
            else if (dHigh >= 0 && dLow < 0)
            {
                dResult = (dHigh + dLow) / 2;
            }
            else if (dHigh <= 0 && dLow < 0)
            {
                dResult = (dLow - dHigh) / 2;
            }
            else
            {
                dResult = 0.0;
            }

            if (bABS) dResult = Math.Abs(dResult);

            return dResult;
        }
		/// <summary>
		/// 2021.03.27 by junho [ADD]
		/// 비례식을 계산한다.
		/// A : B = C : D
		/// </summary>
		public static double GetProportionalExpression(double dA, double dB, double dC, double dD, int nUnkownPosition)
		{
			switch(nUnkownPosition)
			{
				case 1:	// x : B = C : D
					if (dD == 0) return 0;
					return (dB * dC) / dD;
				case 2: // A : x = C : D	
					if (dC == 0) return 0;
					return (dA * dD) / dC;
				case 3:	// A : B = x : D
					if (dB == 0) return 0;
					return (dA * dD) / dB;
				case 4: // A : B = C : x
					if (dA == 0) return 0;
					return (dB * dC) / dA;
				default: return 0;
			}
		}
		/// <summary>
		/// 2021.03.27 by junho [ADD]
		/// 비례식을 계산한다.
		/// A : B = C : D
		/// </summary>
		public static double GetProportionalExpression(int nA, int nB, int nC, int nD, int nUnkownPosition)
		{
			switch (nUnkownPosition)
			{
				case 1:	// x : B = C : D
					if (nD == 0) return 0;
					return (nB * nC) / (double)nD;
				case 2: // A : x = C : D	
					if (nC == 0) return 0;
					return (nA * nD) / (double)nC;
				case 3:	// A : B = x : D
					if (nB == 0) return 0;
					return (nA * nD) / (double)nB;
				case 4: // A : B = C : x
					if (nA == 0) return 0;
					return (nB * nC) / (double)nA;
				default: return 0;
			}
		}
		/// <summary>
		/// a:b = c:result
		/// </summary>
		public static double GetRatio(double a, double b, double c)
		{
			return b * c / a;
		}

		public static string GetStringFromPoint(DPointXY point, int round = 3)
		{
			point.x = Math.Round(point.x, round);
			point.y = Math.Round(point.y, round);
			return string.Format("{0},{1}", point.x, point.y);
		}
		public static DPointXY GetPointXyFromString(string sPoint)
		{
			DPointXY dpReturn = new DPointXY(0,0);

			string[] tempSplit = sPoint.Split(',');

			if (false == tempSplit.Length.Equals(2) && false == tempSplit.Length.Equals(3)) return dpReturn;
			if (false == double.TryParse(tempSplit[0], out dpReturn.x)) return dpReturn;
			if (false == double.TryParse(tempSplit[1], out dpReturn.y)) return dpReturn;

			dpReturn.x = Math.Round(dpReturn.x, 3);
			dpReturn.y = Math.Round(dpReturn.y, 3);
			return dpReturn;
		}
		public static string GetStringFromPoint(DPointXYT point)
		{
			point.x = Math.Round(point.x, 3);
			point.y = Math.Round(point.y, 3);
			point.t = Math.Round(point.t, 3);
			return string.Format("{0},{1},{2}", point.x, point.y, point.t);
		}
		public static DPointXYT GetPointXytFromString(string sPoint)
		{
			DPointXYT dpReturn = new DPointXYT(0, 0, 0);

			string[] tempSplit = sPoint.Split(',');

			if (tempSplit.Length.Equals(2))
			{
				if (false == double.TryParse(tempSplit[0], out dpReturn.x)) return dpReturn;
				if (false == double.TryParse(tempSplit[1], out dpReturn.y)) return dpReturn;
				dpReturn.y = 0.0;
			}
			else if (tempSplit.Length.Equals(3))
			{
				if (false == double.TryParse(tempSplit[0], out dpReturn.x)) return dpReturn;
				if (false == double.TryParse(tempSplit[1], out dpReturn.y)) return dpReturn;
				if (false == double.TryParse(tempSplit[2], out dpReturn.t)) return dpReturn;
			}
			else
				return dpReturn;

			dpReturn.x = Math.Round(dpReturn.x, 3);
			dpReturn.y = Math.Round(dpReturn.y, 3);
			dpReturn.t = Math.Round(dpReturn.t, 3);
			return dpReturn;
		}

		public static string GetStringFromPoint(IPointXY point)
		{
			return string.Format("{0},{1}", point.x, point.y);
		}
		public static IPointXY GetIPointXyFromString(string sPoint)
		{
			IPointXY ipReturn = new IPointXY(0, 0);

			string[] tempSplit = sPoint.Split(',');

			if (false == tempSplit.Length.Equals(2) && false == tempSplit.Length.Equals(3)) return ipReturn;
			if (false == int.TryParse(tempSplit[0], out ipReturn.x)) return ipReturn;
			if (false == int.TryParse(tempSplit[1], out ipReturn.y)) return ipReturn;

			return ipReturn;
		}
		/// <summary>
		/// data table의 value에서 target을 찾아서 key값을 반환 (사잇값 보상)
		/// target이 범위를 벗어날 경우 첫번째 혹은 마지막 key값을 반환
		/// </summary>
		public static double ConvertValueToKey(double target, Dictionary<double, double> dataTable)
		{
			double oldValue = 0.0, newValue;
			double[] values = dataTable.Values.ToArray();

			bool isFirst = true, isFind = false;
			int findIndex = 0;
			double ratio = 0.0;
			for (int i = 0; i < values.Length; ++i )
			{
				if(isFirst)
				{
					isFirst = false;
					oldValue = values[i];
					continue;
				}

				newValue = values[i];

				if(false == IsInRangeMinMax(target, oldValue, newValue))
				{
					oldValue = values[i];
					continue;
				}

				isFind = true;
				findIndex = i;
				ratio = (target - oldValue) * 100 / (newValue - oldValue);
				break;
			}

			double[] keys = dataTable.Keys.ToArray();
			if (isFirst)	// data table이 비어있음
			{
				return 0;
			}
			else if (false == isFind) // data table 범위를 벗어남.
			{
				if (values.Length == 1) return keys[0];	// data table에 값이 1개뿐인 경우 그 값을 반환

				if (values[0] == Math.Min(values[0], values[values.Length - 1]))	// table value가 오름차순인 경우.
				{
					if(target < values[0])	// target이 범위보다 낮은 값이면 첫번째 값 반환
					{
						return keys[0];
					}
					else
					{
						return keys[keys.Length - 1];
					}
				}
				else // table value가 내림차순인 경우.
				{
					if (target > values[0])	// target이 범위보다 높은 값이면 첫번째 값 반환
					{
						return keys[0];
					}
					else
					{
						return keys[keys.Length - 1];
					}
				}
			}
			else
			{
				double result = keys[findIndex - 1] + ((keys[findIndex] - keys[findIndex - 1]) * ratio / 100);
				return result;
			}
		}

		public static double GetExponentValue(double currentValue, double initValue, double destValue, int totalTime, int remainTime, double marginal)
		{
			if (IsInTolerance(currentValue, destValue, marginal)
				|| totalTime <= 0.0
				|| remainTime <= 0)
				return destValue;

			double nextValue = MathFormula.GetExponentValue(initValue, destValue, totalTime, remainTime);

			if (IsInTolerance(nextValue, destValue, marginal))
				return destValue;

			return nextValue;
		}
		public static double GetPPM(int total, int target)
		{
			return Math.Round(FunctionsETC.GetProportionalExpression(total
															, target
															, 1000000	// million
															, 0
															, 4), 3);
		}
		/// <summary>
		/// 2차원 배열을 90도 회전시킨다 (반시계방향)
		/// </summary>
		public static T[,] ArrayRotate90<T>(T[,] original)
		{
			int x = original.GetLength(0);
			int y = original.GetLength(1);
			T[,] rotated = new T[y, x];

			for (int i = 0; i < x; i++)
			{
				for (int j = 0; j < y; j++)
				{
					rotated[j, x - 1 - i] = original[i, j];
				}
			}

			return rotated;
		}
		/// <summary>
		/// 2차원 배열을 180도 회전시킨다
		/// </summary>
		public static T[,] ArrayRotate180<T>(T[,] original)
		{
			int x = original.GetLength(0);
			int y = original.GetLength(1);
			T[,] rotated = new T[x, y];

			for (int i = 0; i < x; i++)
			{
				for (int j = 0; j < y; j++)
				{
					rotated[x - 1 - i, y - 1 - j] = original[i, j];
				}
			}

			return rotated;
		}
		/// <summary>
		/// 2차원 배열을 270도 회전시킨다 (반시계방향)
		/// </summary>
		public static T[,] ArrayRotate270<T>(T[,] original)
		{
			int x = original.GetLength(0);
			int y = original.GetLength(1);
			T[,] rotated = new T[y, x];

			for (int i = 0; i < x; i++)
			{
				for (int j = 0; j < y; j++)
				{
					rotated[y - 1 - j, i] = original[i, j];
				}
			}

			return rotated;
		}
		/// <summary>
		/// 2차원 배열을 상하 반전 시킨다
		/// </summary>
		public static T[,] ArrayFlipVertical<T>(T[,] original)
		{
			int x = original.GetLength(0);
			int y = original.GetLength(1);

			T temp;
			for (int i = 0; i < x; i++)
			{
				for (int j = 0; j < y / 2; j++)
				{
					temp = original[i, j];
					original[i, j] = original[i, y - 1 - j];
					original[i, y - 1 - j] = temp;
				}
			}

			return original;
		}
		/// <summary>
		/// 2차원 배열을 좌주 반전 시킨다
		/// </summary>
		public static T[,] ArrayFlipHorizontal<T>(T[,] original)
		{
			int x = original.GetLength(0);
			int y = original.GetLength(1);

			T temp;
			for (int i = 0; i < x / 2; i++)
			{
				for (int j = 0; j < y; j++)
				{
					temp = original[i, j];
					original[i, j] = original[x - 1 - i, j];
					original[x - 1 - i, j] = temp;
				}
			}

			return original;
		}
		#endregion </CALCULATE>

		#region <Dictionary control>
		/// <summary>
		/// Dictionary를 정렬하여 반환한다.
		/// Key 오름차순 정렬
		/// </summary>
		public static void SortingDictionaryKey<T, U>(ref ConcurrentDictionary<T, U> dicTarget)
		{
			ConcurrentDictionary<T, U> dicBuffer = new ConcurrentDictionary<T, U>();
			List<T> listKey = dicTarget.Keys.ToList();
			foreach (KeyValuePair<T, U> kvp in dicTarget)
			{
				dicBuffer.TryAdd(kvp.Key, kvp.Value);
			}
			listKey.Sort();

			dicTarget.Clear();
			foreach (T nKey in listKey)
			{
				dicTarget.TryAdd(nKey, dicBuffer[nKey]);
			}
		}
		/// <summary>
		/// Dictionary를 정렬하여 반환한다.
		/// Key 오름차순 정렬
		/// </summary>
		public static void SortingDictionaryKey<T, U>(ref Dictionary<T, U> dicTarget)
		{
			Dictionary<T, U> dicBuffer = new Dictionary<T, U>();
			List<T> listKey = dicTarget.Keys.ToList();
			foreach (KeyValuePair<T, U> kvp in dicTarget)
			{
				dicBuffer.Add(kvp.Key, kvp.Value);
			}
			listKey.Sort();

			dicTarget.Clear();
			foreach (T nKey in listKey)
			{
				dicTarget.Add(nKey, dicBuffer[nKey]);
			}
		}
		/// <summary>
		/// Dictionary를 정렬하여 반환한다.
		/// Value 오름차순 정렬
		/// </summary>
		public static void SortingDictionaryValue<T>(ref ConcurrentDictionary<T, T> dicTarget)
		{
			ConcurrentDictionary<T, T> dicBuffer = new ConcurrentDictionary<T, T>();
			List<T> listValue = dicTarget.Values.ToList();
			foreach (KeyValuePair<T, T> kvp in dicTarget)
			{
				dicBuffer.TryAdd(kvp.Value, kvp.Key);
			}
			listValue.Sort();

			dicTarget.Clear();
			foreach (T nValue in listValue)
			{
				dicTarget.TryAdd(dicBuffer[nValue], nValue);
			}
		}
		/// <summary>
		/// Dictionary를 정렬하여 반환한다.
		/// Value 오름차순 정렬
		/// </summary>
		public static void SortingDictionaryValue<T>(ref Dictionary<T, T> dicTarget)
		{
			Dictionary<T, T> dicBuffer = new Dictionary<T, T>();
			List<T> listValue = dicTarget.Values.ToList();
			foreach (KeyValuePair<T, T> kvp in dicTarget)
			{
				dicBuffer.Add(kvp.Value, kvp.Key);
			}
			listValue.Sort();

			dicTarget.Clear();
			foreach (T nValue in listValue)
			{
				dicTarget.Add(dicBuffer[nValue], nValue);
			}
		}
		public static double GetInterpolationValueInLookupTable(Dictionary<double, double> table, double targetKey)
		{
			SortingDictionaryKey(ref table);
			double preKey, postKey;
			double min = table.Keys.ToList().Min();
			double max = table.Keys.ToList().Max();
			if (targetKey <= min) return table[min];
			if (targetKey >= max) return table[max];

			preKey = postKey = min;
			foreach (var kvp in table)
			{
				if(targetKey <= kvp.Key)
				{
					postKey = kvp.Key;
					break;
				}

				preKey = kvp.Key;
			}

			double ratio = GetRatio((postKey - preKey), 100, (targetKey - preKey));
			double result = GetRatio(100, table[postKey] - table[preKey], ratio) + table[preKey];
			return result;
		}
		#endregion </Dictionary control>

		#region <TRANSFORM>
		/// <summary>
		/// 숫자 -> 알파벳
		/// ex) 0 > A, 27 > AB, 16383 > XFD
		/// </summary>
		public static string DecToAlphabet(int num)
		{
			int rest; //나눗셈 계산에 사용될 나머지 값
			string alphabet; //10진수에서 알파벳으로 변환될 값

			byte[] asciiA = Encoding.ASCII.GetBytes("A"); // 0=>A
			rest = num % 26; // A~Z 26자
			asciiA[0] += (byte)rest; // num 0일 때 A, num 4일 때 A+4 => E

			alphabet = Encoding.ASCII.GetString(asciiA); //변환된 알파벳 저장

			num = num / 26 - 1; // 그 다음 자리의 알파벳 계산을 재귀하기 위해, 받은 수/알파벳수 -1 (0은 A라는 문자값이 있으므로 -1을 기준으로 계산함)
			if (num > -1)
			{
				alphabet = alphabet.Insert(0, DecToAlphabet(num)); //재귀 호출하며 결과를 앞자리에 insert
			}
			return alphabet; // 최종값 return
		}
		#endregion </TRANSFORM>

		#region <EXPORT>
		/// <summary>
		/// Grid view를 CSV file로 export한다.
		/// </summary>
		public static bool CsvExportToGridView(System.Windows.Forms.DataGridView gridView)
		{
			List<string> writeData = new List<string>();
			List<string> oneLineItem = new List<string>();
			string oneLine = "";

			// Header
			for (int i = 0; i < gridView.Columns.Count; i++)
			{
				oneLineItem.Add(gridView.Columns[i].HeaderText);
				oneLine = string.Join(",", oneLineItem);
			}
			writeData.Add(oneLine);
			oneLineItem.Clear();

			// Data
			int rowCount = gridView.Rows.Count;
			if (gridView.AllowUserToAddRows == true)
			{
				rowCount--;
			}

			for (int i = 0; i < rowCount; ++i)
			{
				for (int j = 0; j < gridView.Columns.Count; ++j)
				{
					if (gridView[j, i].Value == null)
					{
						oneLineItem.Add("null");
					}
					else
					{
						//oneLineItem.Add(gridView[j, i].Value.ToString());
						oneLineItem.Add(gridView[j, i].Value.ToString().Replace(", ", ""));
					}
				}
				oneLine = string.Join(",", oneLineItem);
				writeData.Add(oneLine);
				oneLineItem.Clear();
			}

			return WriteCsvFile(writeData);
		}
		public static bool WriteCsvFile(List<string> fullData)
		{
			string filePath, fileName;
			if (false == GetFilePathWithSaveFileDialog(out filePath, out fileName))
				return false;

			if(FileExistCheck(filePath, fileName))
			{
				if (false == FileDelete(filePath, fileName)) return false;
			}

			System.IO.FileStream pFStream = new System.IO.FileStream(string.Format(@"{0}\{1}.csv", filePath, fileName), System.IO.FileMode.Append, System.IO.FileAccess.Write);
			using (System.IO.StreamWriter writer = new System.IO.StreamWriter(pFStream, Encoding.UTF8))
			{
				fullData.ForEach(oneLine => writer.WriteLine(oneLine));
				writer.Close();
			}

			Console.WriteLine("Csv File Write Finish");
			return true;
		}
		#endregion </EXPORT>

		#region <FILE CONTROL>
		/// <summary>
		/// Source의 file을 Target으로 copy
		/// </summary>
		public static bool FileCopy(string sourcePath, string sourceName, string targetPath, string targetName)
		{
			string sourceFile = System.IO.Path.Combine(sourcePath, sourceName);
			string destFile = System.IO.Path.Combine(targetPath, targetName);
			return FileCopy(sourceFile, destFile);
		}
		public static bool FileCopy(string source, string target)
		{
			if (System.IO.File.Exists(source))
			{
				try
				{
					System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(target));
					System.IO.File.Copy(source, target, true);
				}
				catch (System.IO.IOException e)
				{
					Console.WriteLine(e.Message);
					return false;
				}
			}
			else
			{
				Console.WriteLine("Source File Not Exists");
				return false;
			}

			return true;
		}
		/// <summary>
		/// Target의 file을 Delete
		/// </summary>
		public static bool FileDelete(string path, string name)
		{
			string filePath = System.IO.Path.Combine(path, name);
			return FileDelete(filePath);
		}
		/// <summary>
		/// Target의 file을 Delete
		/// </summary>
		public static bool FileDelete(string path)
		{
			if (System.IO.File.Exists(path))
			{
				try
				{
					System.IO.File.Delete(path);
				}
				catch (System.IO.IOException e)
				{
					Console.WriteLine(e.Message);
					return false;
				}
			}

			return true;
		}
		/// <summary>
		/// Source의 file을 Target으로 Move
		/// 같은 이름의 file을 덮어씀
		/// </summary>
		public static bool FileMove(string sourcePath, string sourceName, string targetPath, string targetName)
		{
			string sourceFile = System.IO.Path.Combine(sourcePath, sourceName);
			string destFile = System.IO.Path.Combine(targetPath, targetName);
			return FileMove(sourceFile, destFile);
		}
		public static bool FileMove(string sourcePath, string destinationPath)
		{
			if (false == FileCopy(sourcePath, destinationPath)) return false;
			if (false == FileDelete(sourcePath)) return false;
			return true;
		}
		/// <summary>
		/// 해당 File이 존재 하는지 확인
		/// </summary>
		public static bool FileExistCheck(string sTargetPath, string sTargetName)
		{
			string destFile = System.IO.Path.Combine(sTargetPath, sTargetName);

			if (System.IO.File.Exists(destFile)) return true;
			else return false;
		}
		/// <summary>
		/// 해당 File이 존재 하는지 확인
		/// </summary>
		public static bool FileExistCheck(string sTargetPathName)
		{
			if (System.IO.File.Exists(sTargetPathName)) return true;
			else return false;
		}
		/// <summary>
		/// Save File Dialog를 표시하고 File Path와 File Name을 반환한다.
		/// </summary>
		public static bool GetFilePathWithSaveFileDialog(out string filePath, out string fileName)
		{
			var saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			saveFileDialog.Title = "Save an File";
			if (System.Windows.Forms.DialogResult.OK == saveFileDialog.ShowDialog())
			{
				filePath = System.IO.Path.GetDirectoryName(saveFileDialog.FileName);
				fileName = System.IO.Path.GetFileNameWithoutExtension(saveFileDialog.FileName);
				return true;
			}
			else
			{
				filePath = fileName = "";
				return false;
			}
		}
		public static bool GetFilePathWithOpenFileDialog(out string filePath, out string fileName
			, string title = ""
			, string initialDirectory = ""
			, string filter = ""
			, string defaultExt = "")
		{
			var openFileDialog = new System.Windows.Forms.OpenFileDialog();
			openFileDialog.Title = title;
			openFileDialog.InitialDirectory = initialDirectory;
			openFileDialog.Filter = filter;
			openFileDialog.DefaultExt = defaultExt;
			if (System.Windows.Forms.DialogResult.OK == openFileDialog.ShowDialog())
			{
				filePath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
				fileName = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);
				return true;
			}
			else
			{
				filePath = fileName = "";
				return false;
			}
		}
		/// <summary>
		/// 해당File의 내용을 String[]로 반환한다.
		/// </summary>
		public static string[] GetFileRead(string filePath)
		{
			if (false == FunctionsETC.FileExistCheck(filePath))
				return null;

			string[] readLines = null;

			try
			{
				// 해당 위치의 비전 결과를 전부 읽어온다.
				readLines = System.IO.File.ReadAllLines(filePath);
			}
			catch (Exception e)
			{
				System.Console.WriteLine(e.ToString());
				return null;
			}

			return readLines;
		}
		public static string[] GetFileRead(string path, string name)
		{
			string filePath = System.IO.Path.Combine(path, name);
			return GetFileRead(filePath);
		}
		/// <summary>
		/// targetPath의 하위 파일과 디렉토리를 모두 삭제
		/// </summary>
		public static bool FolderDelete(string targetPath)
		{
			var dirInfo = new System.IO.DirectoryInfo(targetPath);
			try { dirInfo.Delete(true); }
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
				return false;
			}

			return true;
		}
		/// <summary>
		/// Folder 존재 여부 반환
		/// </summary>
		public static bool FolderExistCheck(string targetPath)
		{
			var dirInfo = new System.IO.DirectoryInfo(targetPath);
			return dirInfo.Exists;
		}
		/// <summary>
		/// sourch path의 폴더명을 target path로 변경한다.
		/// 이미 동일한 target path의 폴더가 있으면 false 반환
		/// </summary>
		public static bool FolderNameChange(string sourcePath, string targetPath)
		{
			if (sourcePath == targetPath) return false;
			if (false == FolderExistCheck(sourcePath)) return false;
			if (FolderExistCheck(targetPath)) return false;

			var sourceInfo = new System.IO.DirectoryInfo(sourcePath);
			try { sourceInfo.MoveTo(targetPath); }
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return false;
			}

			return true;
		}
		/// <summary>
		/// target path 생성
		/// </summary>
		public static void FolderCreate(string path)
		{
			var dirInfo = new System.IO.DirectoryInfo(path);
			if(false == dirInfo.Exists)
			{
				dirInfo.Create();
			}
		}
		public static void FolderCopy(string sourcePath, string targetPath)
		{
			System.IO.DirectoryInfo sourceDir = new System.IO.DirectoryInfo(sourcePath);
			System.IO.DirectoryInfo targetDir = new System.IO.DirectoryInfo(targetPath);

			// 대상 폴더가 없을 경우 생성
			if (!targetDir.Exists)
			{
				targetDir.Create();
				targetDir.Attributes = sourceDir.Attributes;
			}

			// 소스 폴더의 모든 파일 복사
			foreach (System.IO.FileInfo file in sourceDir.GetFiles())
			{
				string targetFilePath = System.IO.Path.Combine(targetDir.FullName, file.Name);
				file.CopyTo(targetFilePath, true);
			}

			// 소스 폴더의 모든 하위 폴더 재귀적으로 복사
			foreach (System.IO.DirectoryInfo subDir in sourceDir.GetDirectories())
			{
				string targetSubDirPath = System.IO.Path.Combine(targetDir.FullName, subDir.Name);
				FolderCopy(subDir.FullName, targetSubDirPath);
			}
		}

		/// <summary>
		/// 폴더를 압축해서 지정 경로에 생성
		/// 2023.05.23 by junho [MOD] 폴더 복사 후 압축 > 복사된 폴더 삭제하는 방식으로 변경
		/// </summary>
		public static bool FolderCompress(string targetPath, string savePath, string saveName)
		{
			if (false == FileDelete(savePath, saveName)) return false;

			// 일단 폴더 복사
			string copiedPath = System.IO.Path.Combine(savePath, System.IO.Path.GetFileNameWithoutExtension(saveName));
			FolderCopy(targetPath, copiedPath);

			// 복사해둔 폴더를 압축한다.
			string resultFullPath = System.IO.Path.Combine(savePath, saveName);
			System.IO.Compression.ZipFile.CreateFromDirectory(copiedPath, resultFullPath);

			// 압축 완료 후 복사했던 폴더 삭제
			FolderDelete(copiedPath);
			return true;
		}

		/// <summary>
		/// 경로 유효성 검사
		/// </summary>
		public static bool IsValidPath(string path)
		{
			// 경로 길이 체크
			if (path.Length < 3)
				return false;

			// 드라이브 문자열 체크
			var driveCheck = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z]:\\$");
			if (driveCheck.IsMatch(path.Substring(0, 3)) == false)
				return false;

			// 경로 이름에 사용할 수 없는 문자가 있는지 체크
			string invalidPathChars = new string(System.IO.Path.GetInvalidPathChars());
			invalidPathChars += @":/?*" + "\"";

			var regexInvalidPath = new System.Text.RegularExpressions.Regex(string.Format("[{0}]"
				, System.Text.RegularExpressions.Regex.Escape(invalidPathChars)));
			if (regexInvalidPath.IsMatch(path.Substring(3, path.Length - 3)))
				return false;

			// 실제 경로의 드라이브가 존재하는지 체크 
			try
			{
				var dir = new System.IO.DirectoryInfo(System.IO.Path.GetFullPath(path));
				if (dir.Exists == false)
				{
					string drive = System.IO.Path.GetPathRoot(path);
					if (System.IO.Directory.Exists(drive) == false)
					{
						return false;
					}
				}
			}
			catch
			{
				return false;
			}

			return true;
		}
		#endregion </FILE CONTROL>

		#region <SYSTEM>

		#region <SYSTEM PERFORMANCE>
		/// <summary>
		/// CPU 사용량을 %로 반환
		/// </summary>
		public static async Task<double> GetCpuUsage()
		{
			using (var wmi = new System.Management.ManagementObjectSearcher("select * from Win32_PerfFormattedData_PerfOS_Processor where Name != '_Total'"))
			{
				var cpuUsages = wmi.Get().Cast<System.Management.ManagementObject>().Select(mo => (long)(ulong)mo["PercentProcessorTime"]);
				double totalUsage = 0.0;

				await System.Threading.Tasks.Task.Run(() => { totalUsage = cpuUsages.Average(); });

				return (double)totalUsage;
			}
		}
		/// <summary>
		/// Memory사용량을 %로 반환
		/// </summary>
		public static double GetMemoryUsage()
		{
			int totalSize = 0;
			int freeSize = 0;
			System.Management.ManagementClass cls = new System.Management.ManagementClass("Win32_OperatingSystem");
			System.Management.ManagementObjectCollection moc = cls.GetInstances();

			foreach(System.Management.ManagementObject mo in moc)
			{
				totalSize = int.Parse(mo["TotalVisibleMemorySize"].ToString());
				freeSize = int.Parse(mo["FreePhysicalMemory"].ToString());
			}

			double result = 100 - ((freeSize * 100) / totalSize);
			return result;
		}
		/// <summary>
		/// 드라이브 사용량을 %로 반환
		/// </summary>
		public static double GetDriveCapacityUsage(string driveName)
		{
			// Find Drive
			System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
			System.IO.DriveInfo targetDrive = null;

			foreach(System.IO.DriveInfo drive in drives)
			{
				if (drive.DriveType != System.IO.DriveType.Fixed)
					continue;

				if (false == drive.Name.Contains(driveName))
					continue;

				targetDrive = drive;
			}

			if (targetDrive == null)
				return 0;

			double totalSize = targetDrive.TotalSize / 1024 / 1024 / 1024;
			double freeSize = targetDrive.AvailableFreeSpace / 1024 / 1024 / 1024;

			double result = 100 - ((freeSize * 100) / totalSize);
			return result;
		}
		#endregion </SYSTEM PERFORMANCE>

		#region <SYSTEM TIME>
		private struct SYSTEMTIME
		{
			public ushort wYear;
			public ushort wMonth;
			public ushort wDayOfWeek;
			public ushort wDay;
			public ushort wHour;
			public ushort wMinute;
			public ushort wSecond;
			public ushort wMilliseconds;
		}
		[DllImport("kernel32.dll")]
		private static extern uint SetSystemTime(ref SYSTEMTIME lpSystemTime);
		private static void ChangeSystemTime(DateTime targetTime)
		{
			// 사용 시 관리자 권한을 얻도록 프로그램 수정 필요
			var st = new SYSTEMTIME();
            st.wYear = (ushort)targetTime.Year;
			st.wDayOfWeek = (ushort)targetTime.DayOfWeek;
			st.wMonth = (ushort)targetTime.Month;
			st.wDay = (ushort)targetTime.Day;
			st.wHour = (ushort)targetTime.Hour;
			st.wMinute = (ushort)targetTime.Minute;
			st.wSecond = (ushort)targetTime.Second;
			st.wMilliseconds = 0;

			SetSystemTime(ref st);
		}
		#endregion </SYSTEM TIME>

		#endregion </SYSTEM>

		#region <ETC>
		public static void ImportantFileBackup()
		{
			DateTime now = DateTime.Now;
			string backupPath = string.Format("{0}\\..\\..\\BACKUP", Define.DefineConstant.FilePath.FILEPATH_EXE); 
			string savePath = string.Format("{0}\\{1}",backupPath, now.ToString("yyyy.MM.dd"));
			string sourcePath;

			Task<bool>.Run(() =>
			{
				bool result = true;
				try
				{
					// 경로 만들기
					FolderCreate(savePath);

					// Release folder
					sourcePath = Define.DefineConstant.FilePath.FILEPATH_EXE;
					result &= FolderCompress(sourcePath, savePath, "Release.zip");

					// Recipe folder
					sourcePath = Define.DefineConstant.FilePath.FILEPATH_RECIPE;
					result &= FolderCompress(sourcePath, savePath, "Recipe.zip");

					// old backup delete
					DeleteOldBackupFolders(backupPath, 30);

					return result;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
					return false;
				}
			});
		}
		static void DeleteOldBackupFolders(string backupPath, int days)
		{
			var directories = System.IO.Directory.GetDirectories(backupPath);
			DateTime thresholdDate = DateTime.Now.AddDays(-days);

			foreach (var directory in directories)
			{
				DateTime creationTime = System.IO.Directory.GetCreationTime(directory);
				if (creationTime < thresholdDate)
				{
					try
					{
						System.IO.Directory.Delete(directory, true);
						Console.WriteLine(string.Format("Deleted old backup folder: {0}", directory));
					}
					catch (Exception ex)
					{
						Console.WriteLine(string.Format("Error deleting folder {0}: {1}", directory, ex.Message));
					}
				}
			}
		}
		#endregion </ETC>

		#region time
		public static string GetElapsedTime(DateTime start, DateTime end)
		{
			return (end - start).TotalMilliseconds.ToString();
		}
		public static string GetElapsedTime(DateTime start)
		{
			return (DateTime.Now - start).TotalMilliseconds.ToString();
		}
		#endregion /time
	}
}
