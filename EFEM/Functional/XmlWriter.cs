using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Xml;

using FrameOfSystem3;
using XmlWriter_.Only;

namespace XmlWriter_
{
	public class XmlWriter
	{
		#region constructor
		public XmlWriter(string subject, string saveDirectory, string defaultSubject = "Default")
		{
			DEFAULT_SUBJECT = defaultSubject;
			FILE_SAVE_DIRECTORY = saveDirectory;

			_subject = subject;
			_rawDataTree = new DataNode(null, ROOT_NAME);

			SetValue("Subject", _subject);
		}
		#endregion /constructor

		#region field
		readonly XmlAsyncSave _xmlAsyncSave = XmlAsyncSave.Instance;
		protected const string ROOT_NAME = "Root";

		readonly string DEFAULT_SUBJECT;
		readonly string FILE_SAVE_DIRECTORY;

		protected string _subject;

		DataNode _rawDataTree;
		#endregion /field

        #region property
        public string FileSaveDirectory { get { return FILE_SAVE_DIRECTORY; } }
        public string FileSaveName { get { return string.Format("{0}.xml", _subject); } }
		public DataNode RootNode { get { return _rawDataTree; } }
        #endregion /property

		#region interface
		public void ChangeSubject(string newSubject)
		{
			Delete();
			_subject = newSubject;
			SetValue("Subject", _subject);
			Save();
		}

		#region set value
		public bool SetValue(string name, string value, params string[] groups)
		{
			DataNode tree = _rawDataTree;
			foreach (string title in groups)
			{
				tree = tree.GetOrAddNode(title);
			}
			return tree.SetValue(name, value);
		}
		public bool SetValue(string name, bool value, params string[] groups)
		{
			DataNode node = _rawDataTree;
			foreach (string title in groups)
			{
				node = node.GetOrAddNode(title);
			}
			return node.SetValue(name, value);
		}
		public bool SetValue(string name, int value, params string[] groups)
		{
			DataNode tree = _rawDataTree;
			foreach (string title in groups)
			{
				tree = tree.GetOrAddNode(title);
			}
			return tree.SetValue(name, value);
		}
		public bool SetValue(string name, double value, params string[] groups)
		{
			DataNode tree = _rawDataTree;
			foreach (string title in groups)
			{
				tree = tree.GetOrAddNode(title);
			}
			return tree.SetValue(name, value);
		}
		public bool SetValue(string name, DateTime value, params string[] groups)
		{
			DataNode tree = _rawDataTree;
			foreach (string title in groups)
			{
				tree = tree.GetOrAddNode(title);
			}
			return tree.SetValue(name, value);
		}
		public bool SetValue(string name, DPointXY value, params string[] groups)
		{
			DataNode tree = _rawDataTree;
			foreach (string title in groups)
			{
				tree = tree.GetOrAddNode(title);
			}
			return tree.SetValue(name, value);
		}
		public bool SetValue(string name, DPointXYT value, params string[] groups)
		{
			DataNode tree = _rawDataTree;
			foreach (string title in groups)
			{
				tree = tree.GetOrAddNode(title);
			}
			return tree.SetValue(name, value);
		}
		public bool SetValue(string name, DPointXYZ value, params string[] groups)
		{
			DataNode tree = _rawDataTree;
			foreach (string title in groups)
			{
				tree = tree.GetOrAddNode(title);
			}
			return tree.SetValue(name, value);
		}
		public bool SetValue(string name, IPointXY value, params string[] groups)
		{
			DataNode tree = _rawDataTree;
			foreach (string title in groups)
			{
				tree = tree.GetOrAddNode(title);
			}
			return tree.SetValue(name, value);
		}
		public bool SetValue(string name, IPointXYT value, params string[] groups)
		{
			DataNode tree = _rawDataTree;
			foreach (string title in groups)
			{
				tree = tree.GetOrAddNode(title);
			}
			return tree.SetValue(name, value);
		}
		#endregion /set value

		#region get value
		public string GetValue(string name, string defaultValue, params string[] groups)
		{
			DataNode node = _rawDataTree;
			foreach (string title in groups)
			{
				DataNode subNode;
				if (false == node.TryGetSubNode(title, out subNode))
					return defaultValue;
				node = subNode;
			}
			return node.GetValue(name, defaultValue);
		}
		public bool GetValue(string name, bool defaultValue, params string[] groups)
		{
			DataNode node = _rawDataTree;
			foreach (string title in groups)
			{
				DataNode subNode;
				if (false == node.TryGetSubNode(title, out subNode))
					return defaultValue;
				node = subNode;
			}
			return node.GetValue(name, defaultValue);
		}
		public int GetValue(string name, int defaultValue, params string[] groups)
		{
			DataNode node = _rawDataTree;
			foreach (string title in groups)
			{
				DataNode subNode;
				if (false == node.TryGetSubNode(title, out subNode))
					return defaultValue;
				node = subNode;
			}
			return node.GetValue(name, defaultValue);
		}
		public double GetValue(string name, double defaultValue, params string[] groups)
		{
			DataNode node = _rawDataTree;
			foreach (string title in groups)
			{
				DataNode subNode;
				if (false == node.TryGetSubNode(title, out subNode))
					return defaultValue;
				node = subNode;
			}
			return node.GetValue(name, defaultValue);
		}
		public DateTime GetValue(string name, DateTime defaultValue, params string[] groups)
		{
			DataNode node = _rawDataTree;
			foreach (string title in groups)
			{
				DataNode subNode;
				if (false == node.TryGetSubNode(title, out subNode))
					return defaultValue;
				node = subNode;
			}
			return node.GetValue(name, defaultValue);
		}
		public DPointXY GetValue(string name, DPointXY defaultValue, params string[] groups)
		{
			DataNode node = _rawDataTree;
			foreach (string title in groups)
			{
				DataNode subNode;
				if (false == node.TryGetSubNode(title, out subNode))
					return defaultValue;
				node = subNode;
			}
			return node.GetValue(name, defaultValue);
		}
		public DPointXYT GetValue(string name, DPointXYT defaultValue, params string[] groups)
		{
			DataNode node = _rawDataTree;
			foreach (string title in groups)
			{
				DataNode subNode;
				if (false == node.TryGetSubNode(title, out subNode))
					return defaultValue;
				node = subNode;
			}
			return node.GetValue(name, defaultValue);
		}
		public DPointXYZ GetValue(string name, DPointXYZ defaultValue, params string[] groups)
		{
			DataNode node = _rawDataTree;
			foreach (string title in groups)
			{
				DataNode subNode;
				if (false == node.TryGetSubNode(title, out subNode))
					return defaultValue;
				node = subNode;
			}
			return node.GetValue(name, defaultValue);
		}
		public IPointXY GetValue(string name, IPointXY defaultValue, params string[] groups)
		{
			DataNode node = _rawDataTree;
			foreach (string title in groups)
			{
				DataNode subNode;
				if (false == node.TryGetSubNode(title, out subNode))
					return defaultValue;
				node = subNode;
			}
			return node.GetValue(name, defaultValue);
		}
		public IPointXYT GetValue(string name, IPointXYT defaultValue, params string[] groups)
		{
			DataNode node = _rawDataTree;
			foreach (string title in groups)
			{
				DataNode subNode;
				if (false == node.TryGetSubNode(title, out subNode))
					return defaultValue;
				node = subNode;
			}
			return node.GetValue(name, defaultValue);
		}
		#endregion /get value

		#region set attribute
		public bool SetAttribute(string name, string value, params string[] groups)
		{
			DataNode tree = _rawDataTree;
			foreach (string title in groups)
			{
				tree = tree.GetOrAddNode(title);
			}
			return tree.SetAttribute(name, value);
		}
		public bool SetAttribute(string name, bool value, params string[] groups)
		{
			DataNode node = _rawDataTree;
			foreach (string title in groups)
			{
				node = node.GetOrAddNode(title);
			}
			return node.SetAttribute(name, value);
		}
		public bool SetAttribute(string name, int value, params string[] groups)
		{
			DataNode tree = _rawDataTree;
			foreach (string title in groups)
			{
				tree = tree.GetOrAddNode(title);
			}
			return tree.SetAttribute(name, value);
		}
		public bool SetAttribute(string name, double value, params string[] groups)
		{
			DataNode tree = _rawDataTree;
			foreach (string title in groups)
			{
				tree = tree.GetOrAddNode(title);
			}
			return tree.SetAttribute(name, value);
		}
		public bool SetAttribute(string name, DateTime value, params string[] groups)
		{
			DataNode tree = _rawDataTree;
			foreach (string title in groups)
			{
				tree = tree.GetOrAddNode(title);
			}
			return tree.SetAttribute(name, value);
		}
		public bool SetAttribute(string name, DPointXY value, params string[] groups)
		{
			DataNode tree = _rawDataTree;
			foreach (string title in groups)
			{
				tree = tree.GetOrAddNode(title);
			}
			return tree.SetAttribute(name, value);
		}
		public bool SetAttribute(string name, DPointXYT value, params string[] groups)
		{
			DataNode tree = _rawDataTree;
			foreach (string title in groups)
			{
				tree = tree.GetOrAddNode(title);
			}
			return tree.SetAttribute(name, value);
		}
		public bool SetAttribute(string name, DPointXYZ value, params string[] groups)
		{
			DataNode tree = _rawDataTree;
			foreach (string title in groups)
			{
				tree = tree.GetOrAddNode(title);
			}
			return tree.SetAttribute(name, value);
		}
		public bool SetAttribute(string name, IPointXY value, params string[] groups)
		{
			DataNode tree = _rawDataTree;
			foreach (string title in groups)
			{
				tree = tree.GetOrAddNode(title);
			}
			return tree.SetAttribute(name, value);
		}
		public bool SetAttribute(string name, IPointXYT value, params string[] groups)
		{
			DataNode tree = _rawDataTree;
			foreach (string title in groups)
			{
				tree = tree.GetOrAddNode(title);
			}
			return tree.SetAttribute(name, value);
		}
		#endregion /set attribute

		#region get attribute
		public string GetAttribute(string name, string defaultValue, params string[] groups)
		{
			DataNode node = _rawDataTree;
			foreach (string title in groups)
			{
				DataNode subNode;
				if (false == node.TryGetSubNode(title, out subNode))
					return defaultValue;
				node = subNode;
			}
			return node.GetAttribute(name, defaultValue);
		}
		public bool GetAttribute(string name, bool defaultValue, params string[] groups)
		{
			DataNode node = _rawDataTree;
			foreach (string title in groups)
			{
				DataNode subNode;
				if (false == node.TryGetSubNode(title, out subNode))
					return defaultValue;
				node = subNode;
			}
			return node.GetAttribute(name, defaultValue);
		}
		public int GetAttribute(string name, int defaultValue, params string[] groups)
		{
			DataNode node = _rawDataTree;
			foreach (string title in groups)
			{
				DataNode subNode;
				if (false == node.TryGetSubNode(title, out subNode))
					return defaultValue;
				node = subNode;
			}
			return node.GetAttribute(name, defaultValue);
		}
		public double GetAttribute(string name, double defaultValue, params string[] groups)
		{
			DataNode node = _rawDataTree;
			foreach (string title in groups)
			{
				DataNode subNode;
				if (false == node.TryGetSubNode(title, out subNode))
					return defaultValue;
				node = subNode;
			}
			return node.GetAttribute(name, defaultValue);
		}
		public DateTime GetAttribute(string name, DateTime defaultValue, params string[] groups)
		{
			DataNode node = _rawDataTree;
			foreach (string title in groups)
			{
				DataNode subNode;
				if (false == node.TryGetSubNode(title, out subNode))
					return defaultValue;
				node = subNode;
			}
			return node.GetAttribute(name, defaultValue);
		}
		public DPointXY GetAttribute(string name, DPointXY defaultValue, params string[] groups)
		{
			DataNode node = _rawDataTree;
			foreach (string title in groups)
			{
				DataNode subNode;
				if (false == node.TryGetSubNode(title, out subNode))
					return defaultValue;
				node = subNode;
			}
			return node.GetAttribute(name, defaultValue);
		}
		public DPointXYT GetAttribute(string name, DPointXYT defaultValue, params string[] groups)
		{
			DataNode node = _rawDataTree;
			foreach (string title in groups)
			{
				DataNode subNode;
				if (false == node.TryGetSubNode(title, out subNode))
					return defaultValue;
				node = subNode;
			}
			return node.GetAttribute(name, defaultValue);
		}
		public DPointXYZ GetAttribute(string name, DPointXYZ defaultValue, params string[] groups)
		{
			DataNode node = _rawDataTree;
			foreach (string title in groups)
			{
				DataNode subNode;
				if (false == node.TryGetSubNode(title, out subNode))
					return defaultValue;
				node = subNode;
			}
			return node.GetAttribute(name, defaultValue);
		}
		public IPointXY GetAttribute(string name, IPointXY defaultValue, params string[] groups)
		{
			DataNode node = _rawDataTree;
			foreach (string title in groups)
			{
				DataNode subNode;
				if (false == node.TryGetSubNode(title, out subNode))
					return defaultValue;
				node = subNode;
			}
			return node.GetAttribute(name, defaultValue);
		}
		public IPointXYT GetAttribute(string name, IPointXYT defaultValue, params string[] groups)
		{
			DataNode node = _rawDataTree;
			foreach (string title in groups)
			{
				DataNode subNode;
				if (false == node.TryGetSubNode(title, out subNode))
					return defaultValue;
				node = subNode;
			}
			return node.GetAttribute(name, defaultValue);
		}
		#endregion /get attribute

		public List<string> ToStringList()
		{
			List<string> result = new List<string>();
			_rawDataTree.GetStringList(ref result, 0);
			return result;
		}

		#region about group
		public void AddGroup(string name, params string[] groups)
		{
			DataNode tree = _rawDataTree;
			foreach (string title in groups)
			{
				tree = tree.GetOrAddNode(title);
			}
			tree.GetOrAddNode(name);
		}
		public List<string> GetSubNodeTitles(params string[] groups)
		{
			DataNode node = _rawDataTree;
			foreach(string title in groups)
			{
				DataNode subNode;
				if (false == node.TryGetSubNode(title, out subNode))
					return null;

				node = subNode;
			}
			return node.GetSubNodeTitles();
		}
		public Dictionary<string, string> GetElements(params string[] groups)
		{
			DataNode node = _rawDataTree;
			foreach (string title in groups)
			{
				DataNode subNode;
				if (false == node.TryGetSubNode(title, out subNode))
					return null;

				node = subNode;
			}
			return node.Elements;
		}
		#endregion /about group

		#endregion /interface

		#region init / save / load
		public void Init()
		{
			if (false == Load())
			{
				InitInformation();
				Save();
			}
		}
		protected virtual void InitInformation()
		{
		}
		public virtual void Clear()
		{
			_rawDataTree.Clear();
		}
		/// <summary>
		/// T:제거 성공
		/// F:해당하는 node 없음
		/// </summary>
		public bool RemoveNode(params string[] groups)
		{
			if (groups == null || groups.Length < 1)
				return true;

			DataNode node = _rawDataTree;
			DataNode lastNode = null;
			string lastTitle = string.Empty;
			foreach (string title in groups)
			{
				DataNode subNode;
				if (false == node.TryGetSubNode(title, out subNode))
					return false;

				node = subNode;
				lastNode = node;
				lastTitle = title;
			}

			node.Clear();
			if (lastNode == null)
				return true;

			return lastNode.RemoveSubNode(lastTitle);
		}
		public bool Save()
		{
			if (_subject.Equals(DEFAULT_SUBJECT))
				return true;

			var dirInfo = new System.IO.DirectoryInfo(FILE_SAVE_DIRECTORY);
			if (false == dirInfo.Exists)
			{
				dirInfo.Create();
			}

			var xml = new XmlDocument();
			if (false == _rawDataTree.Save(ref xml))
				return false;

			string path = string.Format("{0}\\{1}.xml", FILE_SAVE_DIRECTORY, _subject);
			_xmlAsyncSave.PushSave(path, xml);
			return true;
		}
		public bool SyncSave()
		{
			TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
			if (false == _xmlAsyncSave.RegisterComplateTcs(tcs))
				return false;

			if (false == Save())
				return false;

			var tr = Task.Run<bool>(async () => { return await tcs.Task; }) ;
			bool r = tr.Result;
			return r;
		}

		public bool Load()
		{
			if (_subject.Equals(DEFAULT_SUBJECT))
				return false;

			return Load(_subject);
		}
		protected bool Load(string subject)
		{
			if (subject.Equals(DEFAULT_SUBJECT))
				return false;

			// 2025.02.28 by junho [MOD] improve code
			//return Load(FILE_SAVE_DIRECTORY, string.Format("{0}.xml", subject));
			return Load(FILE_SAVE_DIRECTORY, string.Format("{0}", subject));
		}
		protected bool Load(string filePath, string fileName)
		{
            if (false == fileName.Contains(".xml"))
                fileName = string.Format("{0}{1}", fileName, ".xml");
			string fileFullPathName = string.Format("{0}\\{1}", filePath, fileName);
			if (false == System.IO.File.Exists(fileFullPathName))
				return false;

			var dirInfo = new System.IO.DirectoryInfo(fileFullPathName);
			var xml = new XmlDocument();
			try
			{
				xml.Load(dirInfo.FullName);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return false;
			}

			if (xml.ChildNodes.Count != 1)
				return false;

			XmlNode root = xml.ChildNodes[0];	// root
			if (root.Name != ROOT_NAME)
				return false;
			
			// 2024.08.05 by junho [MOD] improve code
            var node = root.SelectSingleNode("Subject");
            if (node == null || _subject != node.InnerText)
				return false;

			_rawDataTree = new DataNode(null, ROOT_NAME);
			if (false == _rawDataTree.Load(ref root))
				return false;

			return true;
		}

		public void Delete()
		{
			if (_subject.Equals(DEFAULT_SUBJECT))
				return;

			string path = System.IO.Path.Combine(FILE_SAVE_DIRECTORY, string.Format("{0}.xml", _subject));
			if (System.IO.File.Exists(path))
			{
				try
				{
					System.IO.File.Delete(path);
				}
				catch (System.IO.IOException e)
				{
					Console.WriteLine(e.Message);
					return;
				}
			}
		}
		#endregion /init / save / load
	}
}
namespace XmlWriter_.Only
{
	public class DataNode
	{
		#region constructor
		public DataNode(DataNode parent, string title)
		{
			_parent = parent;
			_title = title;
		}
		#endregion /constructor

		#region field
		string _title;
		DataNode _parent = null;
		Dictionary<string, string> _elements = new Dictionary<string, string>();
		Dictionary<string, string> _attributes = new Dictionary<string, string>();
		Dictionary<string, DataNode> _subNodes = new Dictionary<string, DataNode>();
		#endregion /field

		#region interface
		public string Title { get { return _title; } }

		public void Clear()
		{
			_elements.Clear();
			foreach (var tree in _subNodes.Values)
			{
				tree.Clear();
			}
			_subNodes.Clear();
			_parent = null;
		}

		#region set value
		public bool SetValue(string name, string value)
		{
			if (false == _elements.ContainsKey(name))
				_elements.Add(name, value);
			else
				_elements[name] = value;
			return true;
		}
		public bool SetValue(string name, bool value)
		{
			if (false == _elements.ContainsKey(name))
				_elements.Add(name, value.ToString());
			else
				_elements[name] = value.ToString();
			return true;
		}
		public bool SetValue(string name, int value)
		{
			if (false == _elements.ContainsKey(name))
				_elements.Add(name, value.ToString());
			else
				_elements[name] = value.ToString();
			return true;
		}
		public bool SetValue(string name, double value)
		{
			if (false == _elements.ContainsKey(name))
				_elements.Add(name, value.ToString());
			else
				_elements[name] = value.ToString();
			return true;
		}
		public bool SetValue(string name, DateTime value)
		{
			if (false == _elements.ContainsKey(name))
				_elements.Add(name, value.ToString());
			else
				_elements[name] = value.ToString();
			return true;
		}
		public bool SetValue(string name, DPointXY value)
		{
			if (false == _elements.ContainsKey(name))
				_elements.Add(name, value.ToString());
			else
				_elements[name] = value.ToString();
			return true;
		}
		public bool SetValue(string name, DPointXYT value)
		{
			if (false == _elements.ContainsKey(name))
				_elements.Add(name, value.ToString());
			else
				_elements[name] = value.ToString();
			return true;
		}
		public bool SetValue(string name, DPointXYZ value)
		{
			if (false == _elements.ContainsKey(name))
				_elements.Add(name, value.ToString());
			else
				_elements[name] = value.ToString();
			return true;
		}
		public bool SetValue(string name, IPointXY value)
		{
			if (false == _elements.ContainsKey(name))
				_elements.Add(name, value.ToString());
			else
				_elements[name] = value.ToString();
			return true;
		}
		public bool SetValue(string name, IPointXYT value)
		{
			if (false == _elements.ContainsKey(name))
				_elements.Add(name, value.ToString());
			else
				_elements[name] = value.ToString();
			return true;
		}
		#endregion /set value

		#region get value
		public Dictionary<string, string> Elements { get { return _elements; } }
		public string GetValue(string name, string defaultValue)
		{
			if (false == _elements.ContainsKey(name))
				return defaultValue;

			return _elements[name];
		}
		public bool GetValue(string name, bool defaultValue)
		{
			if (false == _elements.ContainsKey(name))
				return defaultValue;

			bool v;
			if (false == bool.TryParse(_elements[name], out v))
				return defaultValue;

			return v;
		}
		public int GetValue(string name, int defaultValue)
		{
			if (false == _elements.ContainsKey(name))
				return defaultValue;

			int v;
			if (false == int.TryParse(_elements[name], out v))
				return defaultValue;

			return v;
		}
		public double GetValue(string name, double defaultValue)
		{
			if (false == _elements.ContainsKey(name))
				return defaultValue;

			double v;
			if (false == double.TryParse(_elements[name], out v))
				return defaultValue;

			return v;
		}
		public DateTime GetValue(string name, DateTime defaultValue)
		{
			if (false == _elements.ContainsKey(name))
				return defaultValue;

			DateTime v;
			if (false == DateTime.TryParse(_elements[name], out v))
				return defaultValue;

			return v;
		}
		public DPointXY GetValue(string name, DPointXY defaultValue)
		{
			if (false == _elements.ContainsKey(name))
				return defaultValue;

			DPointXY v;
			if (false == DPointXY.TryParse(_elements[name], out v))
				return defaultValue;

			return v;
		}
		public DPointXYT GetValue(string name, DPointXYT defaultValue)
		{
			if (false == _elements.ContainsKey(name))
				return defaultValue;

			DPointXYT v;
			if (false == DPointXYT.TryParse(_elements[name], out v))
				return defaultValue;

			return v;
		}
		public DPointXYZ GetValue(string name, DPointXYZ defaultValue)
		{
			if (false == _elements.ContainsKey(name))
				return defaultValue;

			DPointXYZ v;
			if (false == DPointXYZ.TryParse(_elements[name], out v))
				return defaultValue;

			return v;
		}
		public IPointXY GetValue(string name, IPointXY defaultValue)
		{
			if (false == _elements.ContainsKey(name))
				return defaultValue;

			IPointXY v;
			if (false == IPointXY.TryParse(_elements[name], out v))
				return defaultValue;

			return v;
		}
		public IPointXYT GetValue(string name, IPointXYT defaultValue)
		{
			if (false == _elements.ContainsKey(name))
				return defaultValue;

			IPointXYT v;
			if (false == IPointXYT.TryParse(_elements[name], out v))
				return defaultValue;

			return v;
		}
		#endregion /get value

		#region set attribute
		public bool SetAttribute(string name, string value)
		{
			if (false == _attributes.ContainsKey(name))
				_attributes.Add(name, value);
			else
				_attributes[name] = value;
			return true;
		}
		public bool SetAttribute(string name, bool value)
		{
			if (false == _attributes.ContainsKey(name))
				_attributes.Add(name, value.ToString());
			else
				_attributes[name] = value.ToString();
			return true;
		}
		public bool SetAttribute(string name, int value)
		{
			if (false == _attributes.ContainsKey(name))
				_attributes.Add(name, value.ToString());
			else
				_attributes[name] = value.ToString();
			return true;
		}
		public bool SetAttribute(string name, double value)
		{
			if (false == _attributes.ContainsKey(name))
				_attributes.Add(name, value.ToString());
			else
				_attributes[name] = value.ToString();
			return true;
		}
		public bool SetAttribute(string name, DateTime value)
		{
			if (false == _attributes.ContainsKey(name))
				_attributes.Add(name, value.ToString());
			else
				_attributes[name] = value.ToString();
			return true;
		}
		public bool SetAttribute(string name, DPointXY value)
		{
			if (false == _attributes.ContainsKey(name))
				_attributes.Add(name, value.ToString());
			else
				_attributes[name] = value.ToString();
			return true;
		}
		public bool SetAttribute(string name, DPointXYT value)
		{
			if (false == _attributes.ContainsKey(name))
				_attributes.Add(name, value.ToString());
			else
				_attributes[name] = value.ToString();
			return true;
		}
		public bool SetAttribute(string name, DPointXYZ value)
		{
			if (false == _attributes.ContainsKey(name))
				_attributes.Add(name, value.ToString());
			else
				_attributes[name] = value.ToString();
			return true;
		}
		public bool SetAttribute(string name, IPointXY value)
		{
			if (false == _attributes.ContainsKey(name))
				_attributes.Add(name, value.ToString());
			else
				_attributes[name] = value.ToString();
			return true;
		}
		public bool SetAttribute(string name, IPointXYT value)
		{
			if (false == _attributes.ContainsKey(name))
				_attributes.Add(name, value.ToString());
			else
				_attributes[name] = value.ToString();
			return true;
		}
		#endregion /set attribute

		#region get attribute
		public Dictionary<string, string> Attributes { get { return _attributes; } }
		public string GetAttribute(string name, string defaultValue)
		{
			if (false == _attributes.ContainsKey(name))
				return defaultValue;

			return _attributes[name];
		}
		public bool GetAttribute(string name, bool defaultValue)
		{
			if (false == _attributes.ContainsKey(name))
				return defaultValue;

			bool v;
			if (false == bool.TryParse(_attributes[name], out v))
				return defaultValue;

			return v;
		}
		public int GetAttribute(string name, int defaultValue)
		{
			if (false == _attributes.ContainsKey(name))
				return defaultValue;

			int v;
			if (false == int.TryParse(_attributes[name], out v))
				return defaultValue;

			return v;
		}
		public double GetAttribute(string name, double defaultValue)
		{
			if (false == _attributes.ContainsKey(name))
				return defaultValue;

			double v;
			if (false == double.TryParse(_attributes[name], out v))
				return defaultValue;

			return v;
		}
		public DateTime GetAttribute(string name, DateTime defaultValue)
		{
			if (false == _attributes.ContainsKey(name))
				return defaultValue;

			DateTime v;
			if (false == DateTime.TryParse(_attributes[name], out v))
				return defaultValue;

			return v;
		}
		public DPointXY GetAttribute(string name, DPointXY defaultValue)
		{
			if (false == _attributes.ContainsKey(name))
				return defaultValue;

			DPointXY v;
			if (false == DPointXY.TryParse(_attributes[name], out v))
				return defaultValue;

			return v;
		}
		public DPointXYT GetAttribute(string name, DPointXYT defaultValue)
		{
			if (false == _attributes.ContainsKey(name))
				return defaultValue;

			DPointXYT v;
			if (false == DPointXYT.TryParse(_attributes[name], out v))
				return defaultValue;

			return v;
		}
		public DPointXYZ GetAttribute(string name, DPointXYZ defaultValue)
		{
			if (false == _attributes.ContainsKey(name))
				return defaultValue;

			DPointXYZ v;
			if (false == DPointXYZ.TryParse(_attributes[name], out v))
				return defaultValue;

			return v;
		}
		public IPointXY GetAttribute(string name, IPointXY defaultValue)
		{
			if (false == _attributes.ContainsKey(name))
				return defaultValue;

			IPointXY v;
			if (false == IPointXY.TryParse(_attributes[name], out v))
				return defaultValue;

			return v;
		}
		public IPointXYT GetAttribute(string name, IPointXYT defaultValue)
		{
			if (false == _attributes.ContainsKey(name))
				return defaultValue;

			IPointXYT v;
			if (false == IPointXYT.TryParse(_attributes[name], out v))
				return defaultValue;

			return v;
		}
		#endregion /get attribute

		#region for sub node
		public DataNode GetOrAddNode(string title)
		{
			if (false == _subNodes.ContainsKey(title))
				_subNodes.Add(title, new DataNode(this, title));

			return _subNodes[title];
		}
		public Dictionary<string, DataNode> GetSubNodes()
		{
			return _subNodes;
		}
		public bool TryGetSubNode(string title, out DataNode result)
		{
			if (false == _subNodes.ContainsKey(title))
			{
				result = null;
				return false;
			}

			result = _subNodes[title];
			return true;
		}
		public DataNode GetSubNode(string title)
		{
			if (false == _subNodes.ContainsKey(title))
				return null;

			return _subNodes[title];
		}
		public List<string> GetSubNodeTitles()
		{
			return _subNodes.Keys.ToList();
		}
		public bool IsExistSubNodeTitle(string title)
		{
			return _subNodes.ContainsKey(title);
		}
		public bool IsTail()
		{
			return _subNodes.Count <= 0;
		}
		public bool RemoveSubNode(string title)
		{
			if (false == _subNodes.ContainsKey(title))
				return false;

			_subNodes.Remove(title);
			return true;
		}
		#endregion /for sub Node

		#region for root
		public bool IsRoot()
		{
			return _parent == null;
		}
		public DataNode Parent { get { return _parent; } }
		public DataNode Root
		{
			get
			{
				if (IsRoot())
					return this;
				else
					return _parent.Root;
			}
		}
		#endregion /for root

		#region xml save load
		public bool Save(ref XmlDocument xml)
		{
			var node = xml.CreateElement(_title);

			if (false == Save(node, ref xml))
				return false;

			xml.AppendChild(node);
			return true;
		}
		private bool Save(ref XmlDocument xml, ref XmlElement parentNode)
		{
			var node = xml.CreateElement(_title);

			if (false == Save(node, ref xml))
				return false;

			parentNode.AppendChild(node);
			return true;
		}
		private bool Save(XmlElement node, ref XmlDocument xml)
		{
			foreach (var attribute in _attributes)
			{
				node.SetAttribute(attribute.Key, attribute.Value);
			}

			XmlElement dataNode;
			foreach (var element in _elements)
			{
				dataNode = xml.CreateElement(element.Key);
				dataNode.InnerXml = element.Value;
				node.AppendChild(dataNode);
			}

			foreach (var subNode in _subNodes.Values)
			{
				if (false == subNode.Save(ref xml, ref node))
					return false;
			}
			return true;
		}

		public bool Load(ref XmlNode parentNode)
		{
			bool result = true;
			foreach (XmlAttribute attribute in parentNode.Attributes)
			{
				SetAttribute(attribute.Name, attribute.Value);
			}

			foreach (XmlNode n in parentNode.ChildNodes)
			{
				XmlNode node = n;

				// 2025.05.29 by junho [MOD] improve code. 자식도 없고 텍스트도 없으면 빈 값을 가지는 value node로 판단하도록 한다.
				//if (node.ChildNodes.Count == 1 && node.ChildNodes[0].Value != null)	// node is data node
				//{
				//	XmlNode subNode = node.ChildNodes[0];
				//	result &= SetValue(node.Name, subNode.Value);
				//}
				//else if (node.ChildNodes.Count > 0)
				//{
				//	var subNode = GetOrAddNode(node.Name);
				//	result &= subNode.Load(ref node);
				//}
				//else
				//{
				//	var subNode = GetOrAddNode(node.Name);
				//	result &= subNode.Load(ref node);
				//}
				if (node.ChildNodes.Count == 1 && node.FirstChild.NodeType == XmlNodeType.Text)
				{
					result &= SetValue(node.Name, node.InnerText); // 정상적인 값 처리
				}
				else if (node.ChildNodes.Count > 0)
				{
					var subNode = GetOrAddNode(node.Name);
					result &= subNode.Load(ref node);
				}
				else
				{
					// 자식도 없고 텍스트도 없으면 빈 문자열로 저장
					result &= SetValue(node.Name, string.Empty); // 빈 태그를 빈 문자열로 인식
				}
			}

			return result;
		}
		#endregion /xml save load

		public void GetStringList(ref List<string> result, int indentCount)
		{
			string indent = string.Empty;
			for (int i = 0; i < indentCount; ++i)
			{
				indent += "\t";
			}

			result.Add(string.Format("{0}<{1}>", indent, _title));

			indent += "\t";
			++indentCount;
			foreach (var kvp in _elements)
			{
				result.Add(string.Format("{0}{1} : {2}", indent, kvp.Key, kvp.Value));
			}

			++indentCount;
			foreach (var subNode in _subNodes.Values)
			{
				subNode.GetStringList(ref result, indentCount);
			}
		}
		#endregion /interface
	}

	public class XmlAsyncSave
	{
		#region interface
		public void PushSave(string path, XmlDocument data)
		{
			_saveList.AddOrUpdate(path, data, (s, d) => data);
			_threadWait.Set();
		}

		public bool RegisterComplateTcs(TaskCompletionSource<bool> tcs)
		{
			if (tcs == null || _currentSaveTcs != null)
				return false;

			lock (_saveTcsLock)
			{
				_currentSaveTcs = tcs;
			}
			return true;
		}
		#endregion /interface

		#region singleton
		static private XmlAsyncSave _instance = null;
		private XmlAsyncSave()
		{
			Activate();
			System.Windows.Forms.Application.ApplicationExit += Deactivate;
		}
		public static XmlAsyncSave Instance
		{
			get
			{
				if (_instance == null) _instance = new XmlAsyncSave();
				return _instance;
			}
		}
		#endregion /singleton

		#region field
		Thread _thread;
		AutoResetEvent _threadWait = new AutoResetEvent(false);
		bool _exit = false;

		// key : save path
		ConcurrentDictionary<string, XmlDocument> _saveList = new ConcurrentDictionary<string, XmlDocument>();
		readonly object _saveTcsLock = new object();
		TaskCompletionSource<bool> _currentSaveTcs = null;
		#endregion /field

		#region method
		private void Activate()
		{
			_thread = new Thread(Execute);
			_thread.IsBackground = true;
			_thread.Name = "XmlWriter";
			_thread.Priority = ThreadPriority.BelowNormal;
			_thread.Start();
		}
		private void Deactivate(object sender, EventArgs e)
		{
			_exit = true;
			_threadWait.Set();
		}
		private void Execute()
		{
			while (true)
			{
				_threadWait.WaitOne();

				XmlDocument saveXml = null;
				if (_saveList.Count <= 0)
				{
					if (_exit) break;
					else continue;
				}

				string filePath = _saveList.Keys.ToArray()[0];
				if(false == _saveList.TryRemove(filePath, out saveXml))
					continue;

				try
				{
					saveXml.Save(filePath);
				}
				catch (Exception e)
				{
					Console.WriteLine(filePath + " >> Exception");
					Console.WriteLine(e.Message);
					continue;
				}

				if (_saveList.Count > 0)
				{
					_threadWait.Set();
				}
				else
				{
					if (_currentSaveTcs != null)
					{
						TaskCompletionSource<bool> tcs = null;
						lock (_currentSaveTcs)
						{
							tcs = _currentSaveTcs;
							_currentSaveTcs = null;
						}
						tcs.TrySetResult(true);
					}
				}
			}

			Console.WriteLine("Xml writer thread closed");
		}
		#endregion /method
	}
}
