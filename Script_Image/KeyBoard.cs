using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace ScriptImage
{
    public class KeyBoard
    {


        


        #region DllImport
        [DllImport("user32.dll")]
        private static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        #endregion

        const uint WM_KEYDOWN = 0x100;
        const uint WM_KEYUP = 0x0101;


        public static void Press(IntPtr hWnd, Keys keys, int delayTime = 1)
        {
            PostMessage(hWnd, WM_KEYDOWN, (IntPtr)(keys), IntPtr.Zero);
            Thread.Sleep(delayTime + 000);
            PostMessage(hWnd, WM_KEYUP, (IntPtr)(keys), IntPtr.Zero);
        }

    }
}