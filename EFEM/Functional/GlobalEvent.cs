using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Diagnostics;

using System.Runtime.InteropServices;

namespace FrameOfSystem3.Functional
{
    public class GlobalEvent
    {
        #region dllPort
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc callback, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        #endregion //dllPort

        #region 싱글톤
        private GlobalEvent()
        {
        }

        private static readonly Lazy<GlobalEvent> lazyInstance = new Lazy<GlobalEvent>(() => new GlobalEvent());
        static public GlobalEvent GetInstance()
        {
            return lazyInstance.Value;
        }
        #endregion

        #region Variable
        private delegate IntPtr HookProc(int nCode, int wParam, IntPtr lParam);

        private HookProc hookDelegate;
        private IntPtr hHook = IntPtr.Zero;
        #endregion /Variable

        #region Const
        private const int WH_MOUSE_LL = 14;
        private const int WH_KEYBOARD_LL = 13;

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;

        private const uint WM_LBUTTONDOWN = 0x0201;
        #endregion /Const

        public void SetHook()
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                hookDelegate = new HookProc(MouseHookCallback);
                hHook = SetWindowsHookEx(WH_MOUSE_LL, hookDelegate, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        public void UnHook()
        {
            UnhookWindowsHookEx(hHook);
        }

        private IntPtr MouseHookCallback(int code, int wParam, IntPtr lParam)
        {
            if (wParam == WM_LBUTTONDOWN)
            {
                Account.CAccount.GetInstance().ResetLogoutTime();
            }

            return CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
        }
    }
}
