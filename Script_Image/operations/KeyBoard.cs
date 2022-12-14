using System;

namespace ScriptImage
{
    public class KeyBoard : DllHolder
    {
        private const uint WM_KEYDOWN = 0x100;
        private const uint WM_KEYUP = 0x0101;

        //Key event with window handle
        public static void Press(IntPtr hWnd, Keys keys, double delayTime = 1)
        {
            PostMessage(hWnd, WM_KEYDOWN, (IntPtr)(keys), 0);
            DelayTime.Delay(delayTime);
            PostMessage(hWnd, WM_KEYUP, (IntPtr)(keys), 0);
        }
    }
}