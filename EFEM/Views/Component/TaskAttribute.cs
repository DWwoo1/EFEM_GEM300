using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

using System.Windows.Forms;

namespace FrameOfSystem3.Component
{
    public class MapListAttribute : CustomAttribute
    {
        public MapListAttribute()
        {
            m_lists = Enum.GetNames(typeof(Define.DefineEnumProject.Map.EN_MAP_TYPE));
        }
    }

    /// <summary>
    /// 2021.06.14. by shkim [ADD] 파라미터 설정을 위한 컨트롤의 Task 속성에 리스트를 표시하기 위한 Attribute (각 프로젝트에 맞게 Enum 설정 필요)
    /// </summary>
    public class TaskListAttribute : CustomAttribute
    {
        public TaskListAttribute()
        {
            m_lists = Enum.GetNames(typeof(Define.DefineEnumProject.Task.EN_TASK_LIST));
        }
    }

    public class DesignerWithTaskAxisList : System.Windows.Forms.Design.ControlDesigner
    {
        public DesignerWithTaskAxisList()
        {

        }

        [Category("Recipe"), Description("Axis Name at seleted task")]
        [TypeConverter(typeof(CustomTypeConverter))]
        public string TaskAxisName
        {
            get
            {
                if (Control is CustomParameterButton)
                {
                    CustomParameterButton paraBtn = (CustomParameterButton)Control;
                    return paraBtn.AxisName;
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (Control is CustomParameterButton)
                {
                    CustomParameterButton paraBtn = (CustomParameterButton)Control;
                    paraBtn.AxisName = value;
                }
            }
        }

        /// <summary>
        /// 2021.06.14. by shkim. [ADD] 디자이너에서 컨트롤의 Task를 변경하였을 때, 각 Task의 Axis 리스트를 표시하기 위한 PreFilter (각 프로젝트에 맞게 Enum 변경필요)
        /// </summary>
        /// <param name="properties"></param>
        protected override void PreFilterProperties(System.Collections.IDictionary properties)
        {
            base.PreFilterProperties(properties);

            if (Control is CustomParameterButton)
            {
                CustomParameterButton paraBtn = (CustomParameterButton)Control;
                string[] list = ConvertProjectTaskData.GetAxisListByTask(paraBtn.TaskName);

                PropertyDescriptor pd = TypeDescriptor.CreateProperty(
                    typeof(DesignerWithTaskAxisList),
                    "TaskAxisName",
                    typeof(string),
                    new Attribute[]
                    {
                        new CustomAttribute(list),
                        new DesignOnlyAttribute(true)
                    });
                properties.Add("TaskAxisName", pd);
            }
        }
    }

    /// <summary>
    /// 2021.08.02. by shkim. [ADD] 등록된 축으로 조그 이동하고 파라미터를 설정할 수 있는 버튼의 속성 디자이너
    /// </summary>
    public class DesignerCustomJogButtonAxisList : System.Windows.Forms.Design.ControlDesigner
    {
        public DesignerCustomJogButtonAxisList()
        {

        }

        [Category("Recipe"), Description("Axis 1 Name at seleted task")]
        [TypeConverter(typeof(CustomTypeConverter))]
        [RefreshProperties(RefreshProperties.All)]
        public string TaskAxisName1
        {
            get
            {

                if (Control is CustomJogButton)
                {
                    CustomJogButton jogBtn = (CustomJogButton)Control;
                    return jogBtn.AxisName1;
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (Control is CustomJogButton)
                {
                    CustomJogButton jogBtn = (CustomJogButton)Control;
                    jogBtn.AxisName1 = value;
                }
            }
        }

        [Category("Recipe"), Description("Axis 2 Name at seleted task")]
        [TypeConverter(typeof(CustomTypeConverter))]
        [RefreshProperties(RefreshProperties.All)]
        public string TaskAxisName2
        {
            get
            {

                if (Control is CustomJogButton)
                {
                    CustomJogButton jogBtn = (CustomJogButton)Control;
                    return jogBtn.AxisName2;
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (Control is CustomJogButton)
                {
                    CustomJogButton jogBtn = (CustomJogButton)Control;
                    jogBtn.AxisName2 = value;
                }
            }
        }

        protected override void PreFilterProperties(System.Collections.IDictionary properties)
        {
            base.PreFilterProperties(properties);

            if (Control is CustomJogButton)
            {
                CustomJogButton jogBtn = (CustomJogButton)Control;
                string[] list;

				list = Enum.GetNames(typeof(Define.DefineEnumProject.Motion.EN_AXIS));

                PropertyDescriptor pdName1 = TypeDescriptor.CreateProperty(
                    typeof(DesignerCustomJogButtonAxisList),
                    "TaskAxisName1",
                    typeof(string),
                    new Attribute[]
                    {
                        new CustomAttribute(list),
                        new DesignOnlyAttribute(true)
                    });
                properties.Add("TaskAxisName1", pdName1);

                if (jogBtn.AxisCount == 2)
                {
                    PropertyDescriptor pdName2 = TypeDescriptor.CreateProperty(
                         typeof(DesignerCustomJogButtonAxisList),
                         "TaskAxisName2",
                         typeof(string),
                         new Attribute[]
                    {
                        new CustomAttribute(list),
                        new DesignOnlyAttribute(true)
                    });
                    properties.Add("TaskAxisName2", pdName2);
                }
            }
        }
    }

    public class DesignerWithMapList : System.Windows.Forms.Design.ControlDesigner
    {
        public DesignerWithMapList()
        {

        }
        
        [Category("Recipe"), Description("Map List")]
        [TypeConverter(typeof(CustomTypeConverter))]
        public string MapName
        {
            get
            {
                if (Control is CustomParameterButton)
                {
                    CustomParameterButton paraBtn = (CustomParameterButton)Control;
                    return paraBtn.AssociatedMap;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (Control is CustomParameterButton)
                {
                    CustomParameterButton paraBtn = (CustomParameterButton)Control;
                    paraBtn.AxisName = value;
                }
            }
        }

        /// <summary>
        /// 2021.06.14. by shkim. [ADD] 디자이너에서 컨트롤의 Task를 변경하였을 때, 각 Task의 Axis 리스트를 표시하기 위한 PreFilter (각 프로젝트에 맞게 Enum 변경필요)
        /// </summary>
        /// <param name="properties"></param>
        protected override void PreFilterProperties(System.Collections.IDictionary properties)
        {
            base.PreFilterProperties(properties);

            if (Control is CustomParameterButton)
            {
                CustomParameterButton paraBtn = Control as CustomParameterButton;
				string[] list = ConvertProjectTaskData.GetAxisListByTask(paraBtn.TaskName);

                PropertyDescriptor pd = TypeDescriptor.CreateProperty(
                    typeof(DesignerWithTaskAxisList),
                    "TaskAxisName",
                    typeof(string),
                    new Attribute[]
                    {
                        new CustomAttribute(list),
                        new DesignOnlyAttribute(true)
                    });
                properties.Add("TaskAxisName", pd);
            }
        }
    }

    public class CustomAttribute : Attribute
    {
        protected string[] m_lists = null;
        public string[] List
        {
            get
            {
                return m_lists;
            }
        }
        public CustomAttribute(string[] list)
        {
            m_lists = list;
        }
        public CustomAttribute()
        {

        }
    }

    public class CustomTypeConverter : TypeConverter
    {
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool CanConvertTo(ITypeDescriptorContext pContext, Type pDestinationType)
        {
            return base.CanConvertTo(pContext, pDestinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext pContext, CultureInfo pCulture, object pValue, Type pDestinationType)
        {
            return base.ConvertTo(pContext, pCulture, pValue, pDestinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext pContext, Type pSourceType)
        {
            if (pSourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(pContext, pSourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext pContext, CultureInfo pCulture, object pValue)
        {
            //             if (pCulture is null)
            //             {
            //                 throw new ArgumentNullException(nameof(pCulture));
            //             }

            if (pValue is string)
            {
                return pValue.ToString();
            }

            return base.ConvertFrom(pContext, pCulture, pValue);
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext pContext)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext pContext)
        {
            List<string> values = new List<string>();

            AttributeCollection ua = pContext.PropertyDescriptor.Attributes;

            foreach (Attribute attr in ua)
            {
                if (attr is CustomAttribute)
                {
                    CustomAttribute slAttr = attr as CustomAttribute;
                    if (slAttr.List != null)
                    {
                        values.AddRange(slAttr.List);
                    }
                    else
                    {
                        values.Add(string.Empty);
                    }
                }
            }
            return new StandardValuesCollection(values);
        }
    }


	public class DesignerWithTaskActionList : System.Windows.Forms.Design.ControlDesigner
	{
		public DesignerWithTaskActionList()
		{

		}

		[Category("Information"), Description("Action Name at seleted task")]
		[TypeConverter(typeof(CustomTypeConverter))]
		public string TaskActionName
		{
			get
			{
				if (Control is CustomActionButton)
				{
					CustomActionButton paraBtn = (CustomActionButton)Control;
					return paraBtn.ActionName;
				}
				else
				{
					return string.Empty;
				}
			}
			set
			{
				if (Control is CustomActionButton)
				{
					CustomActionButton paraBtn = (CustomActionButton)Control;
					paraBtn.ActionName = value;
				}
			}
		}

		/// <summary>
		/// 2021.08.04. by junho [ADD] 디자이너에서 컨트롤의 Task를 변경하였을 때, 각 Task의 Action 리스트를 표시하기 위한 PreFilter (각 프로젝트에 맞게 Enum 변경필요)
		/// </summary>
		/// <param name="properties"></param>
		protected override void PreFilterProperties(System.Collections.IDictionary properties)
		{
			base.PreFilterProperties(properties);

			if (Control is CustomActionButton)
			{
				CustomActionButton paraBtn = (CustomActionButton)Control;
				string[] list = ConvertProjectTaskData.GetActionListByTask(paraBtn.TaskName);

				PropertyDescriptor pd = TypeDescriptor.CreateProperty(
					typeof(DesignerWithTaskActionList),
					"TaskActionName",
					typeof(string),
					new Attribute[]
                    {
                        new CustomAttribute(list),
                        new DesignOnlyAttribute(true)
                    });
				properties.Add("TaskActionName", pd);
			}
		}
	}

	public class ConvertProjectTaskData
	{
		public static string[] GetActionListByTask(string taskName)
		{
			Define.DefineEnumProject.Task.EN_TASK_LIST enTask;
			if (false == Enum.TryParse<Define.DefineEnumProject.Task.EN_TASK_LIST>(taskName, out enTask))
				return new string[] { string.Empty };

			switch (enTask)
			{
				default:
					return new string[] { string.Empty };
			}
		}
		public static string[] GetAxisListByTask(string taskName)
		{
			Define.DefineEnumProject.Task.EN_TASK_LIST enTask;
			if (false == Enum.TryParse<Define.DefineEnumProject.Task.EN_TASK_LIST>(taskName, out enTask))
				return new string[] { string.Empty };

			switch (enTask)
			{
                default:
					return new string[] { string.Empty };
			}
		}
		public static bool IsDefinedParameter(Define.DefineEnumProject.Task.EN_TASK_LIST enTask, string parameterName)
		{
			Type type = GetDefinedParameter(enTask);
			if (type == null || false == type.IsEnum) return false;
			return Enum.IsDefined(type, parameterName);

		}
		public static Type GetDefinedParameter(Define.DefineEnumProject.Task.EN_TASK_LIST enTask)
		{
			switch (enTask)
			{
				
				case Define.DefineEnumProject.Task.EN_TASK_LIST.LoadPort1:
                case Define.DefineEnumProject.Task.EN_TASK_LIST.LoadPort2:
                case Define.DefineEnumProject.Task.EN_TASK_LIST.LoadPort3:
                case Define.DefineEnumProject.Task.EN_TASK_LIST.LoadPort4:
#if PWA500W
#else
                case Define.DefineEnumProject.Task.EN_TASK_LIST.LoadPort5:
                case Define.DefineEnumProject.Task.EN_TASK_LIST.LoadPort6:
#endif
                    return typeof(Define.DefineEnumProject.Task.LoadPort.PARAM_PROCESS);

                case Define.DefineEnumProject.Task.EN_TASK_LIST.AtmRobot:
                    return typeof(Define.DefineEnumProject.Task.AtmRobot.PARAM_PROCESS);

                case Define.DefineEnumProject.Task.EN_TASK_LIST.Global:
					return typeof(Define.DefineEnumProject.Task.Global.PARAM_GLOBAL);
			}
            return null;
		}
	}
}
