using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

using XmlWriter_.Only;

namespace XmlWriter_
{
	class Sys3FileXmlConverter
	{
		const string PREFIX_GROUP_NAME_IS_NUMBER = "_n_";
		public void XmlToSys3File(string sourcePath, string extension = ".cfg")
		{
			string filePath = System.IO.Path.GetDirectoryName(sourcePath);
			string fileName = System.IO.Path.GetFileNameWithoutExtension(sourcePath);
			XmlWriter xml = new XmlWriter(fileName, filePath);
			if (false == xml.Load())
				throw new Exception("file load failed");

			DataNode root = xml.RootNode;

			List<string> contents = new List<string>();
			string s = root.GetAttribute("FileVersion", string.Empty);
			if(string.IsNullOrWhiteSpace(s))
				throw new Exception("not found file version");
			
			contents.Add(string.Format(">> FileVersion {0}", s));
			foreach (var node in root.GetSubNodes().Values)
			{
				ToSys3Format(node, ref contents, 0);
			}

			using (StreamWriter writer = new StreamWriter(sourcePath.Replace(".xml", extension)))
			{
				foreach (string c in contents)
				{
					writer.WriteLine(c);
				}
			}
		}
		void ToSys3Format(DataNode node, ref List<string> contents, int indentCount)
		{
			string indent = string.Empty;
			for (int i = 0; i < indentCount; ++i) { indent += "\t"; }

			string groupName = node.Title.Contains(PREFIX_GROUP_NAME_IS_NUMBER)
				? node.Title.Substring(PREFIX_GROUP_NAME_IS_NUMBER.Length) : node.Title;
			contents.Add(string.Format("{0}GROUP {1}", indent, groupName));
			foreach (var kvp in node.Elements)
			{
				contents.Add(string.Format("{0}\t{1} = {2}",indent,kvp.Key,kvp.Value));
			}

			++indentCount;
			foreach(var subNode in node.GetSubNodes().Values)
			{
				ToSys3Format(subNode, ref contents, indentCount);
			}
			contents.Add(string.Format("{0}END", indent));
		}

		public XmlWriter Sys3FileToXml(string sourcePath, string destinationPath = "")
		{
			try
			{
				var readData = FileRead(sourcePath);
				if (readData == null || readData.Length < 2)
					throw new Exception("file read failed");

				string pattern = @"\d+\.\d+\.\d+\.\d+";
				Match match = Regex.Match(readData[0], pattern);
				if (string.IsNullOrWhiteSpace(destinationPath))
					destinationPath = sourcePath;

				XmlWriter result = new XmlWriter(System.IO.Path.GetFileNameWithoutExtension(destinationPath), System.IO.Path.GetDirectoryName(destinationPath));
				if(false == match.Success)
					throw new Exception("not found file version");

				result.SetAttribute("FileVersion", match.Value);

				List<string> groups = new List<string>();
				readData = readData.Skip(1).ToArray();
				int i = 0;
				foreach (var d in readData)
				{
					++i;
					pattern = @"GROUP (\w+)";
					match = Regex.Match(d, pattern);
					if(match.Success)
					{
						string groupName = match.Value.Substring(6);

						// 2025.05.29 by junho [MOD] improve code
						//try
						//{
						//	groupName = System.Xml.XmlConvert.VerifyName(groupName);
						//}
						//catch
						//{
						//	try
						//	{
						//		groupName = System.Xml.XmlConvert.VerifyName(PREFIX_GROUP_NAME_IS_NUMBER + groupName);
						//	}
						//	catch (Exception e)
						//	{
						//		throw new Exception($"{e.Message} (Line: {i})");
						//	}
						//}
						if (false == IsValidXmlName(groupName))
						{
							groupName = PREFIX_GROUP_NAME_IS_NUMBER + groupName;
							if (false == IsValidXmlName(groupName))
							{
								throw new Exception(string.Format("Invalid XML name: {0} (Line: {1})", groupName, i));
							}
						}

						result.AddGroup(groupName, groups.ToArray());
						groups.Add(groupName);
						continue;
					}

					pattern = @"END\b";
					match = Regex.Match(d, pattern);
					if (match.Success)
					{
						if (groups.Count <= 0)
							throw new Exception(string.Format("abnormal data 1 (Line: {0})", i));

						groups.RemoveAt(groups.Count - 1);
						continue;
					}

					if (d.Contains("="))
					{
						string[] splited = d.Split(new char[] { '=' }, 2); // 빈 값도 유지
						if (splited.Length != 2)
							throw new Exception(string.Format("abnormal data 2 (Line: {0})", i));

						result.SetValue(splited[0].Trim(), splited[1].Trim(), groups.ToArray());
						continue;
					}
				}

				result.Save();
				return result;
			}
			catch (Exception e)
			{
				throw new Exception("Exception catch : " + e.Message);
			}
		}

		#region method
		static string[] FileRead(string filePath)
		{
			if (false == FileExistCheck(filePath))
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
		static bool FileExistCheck(string sTargetPathName)
		{
			if (System.IO.File.Exists(sTargetPathName)) return true;
			else return false;
		}
		static private bool IsValidXmlName(string name)
		{
			return Regex.IsMatch(name, @"^[A-Za-z_][\w.-]*$");
		}
		#endregion /method
	}
}
