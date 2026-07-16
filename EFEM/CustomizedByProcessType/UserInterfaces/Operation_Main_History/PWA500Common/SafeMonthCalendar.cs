using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using EFEM.Defines.Common;

namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainHistory.PWA500Common
{
    public partial class SafeMonthCalendar : MonthCalendar
    {
        protected override AccessibleObject CreateAccessibilityInstance()
        {
            return new SafeCalendarAccessibleObject(this);
        }

        protected override void WndProc(ref Message m)
        {
            try
            {
                // WM_SETFOCUS 메시지 처리
                if (m.Msg == 0x0007)  // WM_SETFOCUS
                {
                    SetFocusSafely();
                }
                else
                {
                    base.WndProc(ref m);
                }
            }
            catch (Exception ex)
            {
                AsyncLoggerForEfem.Instance.EnqueueLog(BaseLogTypes.LogTypeDebug, string.Format("Calander Proc Exeception : ({0}) -> {1} : {2}", m.ToString(), ex.Message, ex.StackTrace));
            }
        }

        private void SetFocusSafely()
        {
            try
            {
                if (Enabled && Visible)
                {
                    Focus();
                }
                else
                {
                    AsyncLoggerForEfem.Instance.EnqueueLog(BaseLogTypes.LogTypeDebug, "Control is not enabled or visible, cannot set focus.");
                }
            }
            catch (Exception ex)
            {
                AsyncLoggerForEfem.Instance.EnqueueLog(BaseLogTypes.LogTypeDebug, string.Format("Setting Focus Exeception : {0} -> {1}", ex.Message, ex.StackTrace));
            }
        }

        private class SafeCalendarAccessibleObject : ControlAccessibleObject
        {
            public SafeCalendarAccessibleObject(Control owner) : base(owner) { }

            public override AccessibleObject GetChild(int index)
            {
                try
                {
                    if (index < 0 || index >= GetChildCount())
                    {
                        AsyncLoggerForEfem.Instance.EnqueueLog(BaseLogTypes.LogTypeDebug, $"SafeCalendar accChild 예외 캐치 : {index} / { GetChildCount() }");
                        return null;
                    }

                    return base.GetChild(index);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    AsyncLoggerForEfem.Instance.EnqueueLog(BaseLogTypes.LogTypeDebug, $"SafeCalendar accChild 예외: {ex.Message}");
                    return null;
                }
                catch (Exception ex)
                {
                    AsyncLoggerForEfem.Instance.EnqueueLog(BaseLogTypes.LogTypeDebug, $"SafeCalendar GetChild 예외: {ex.Message}");
                    return null;
                }
            }

            public override int GetChildCount()
            {
                try
                {
                    return base.GetChildCount();
                }
                catch
                {
                    return 0; // fallback
                }
            }
        }
    }
}
