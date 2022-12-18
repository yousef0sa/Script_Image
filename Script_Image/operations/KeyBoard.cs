using System;
using System.Text;

namespace ScriptImage
{
    public class KeyBoard : DllHolder
    {
        private const uint WM_KEYDOWN = 0x100;
        private const UInt32 WM_CHAR = 0x0102;
        private const uint WM_KEYUP = 0x0101;
        private const int WM_SETTEXT = 0X000C;
        private const int WM_GETTEXT = 0x000D;
        private const int WM_GETTEXTLENGTH = 0x000E;


        //Key event with window handle
        public static void Press(IntPtr hWnd, Keys keys, double delayTime = 0.5)
        {
            SendMessage(hWnd, WM_KEYDOWN, (IntPtr)keys, IntPtr.Zero);
            DelayTime.Delay(delayTime);
            SendMessage(hWnd, WM_KEYUP, (IntPtr)keys, IntPtr.Zero);
        }

        //key press down
        public static void PressDown(IntPtr hWnd, Keys keys)
        {
            SendMessage(hWnd, WM_KEYDOWN, (IntPtr)keys, IntPtr.Zero);
        }

        //Key press up
        public static void PressUp(IntPtr hWnd, Keys keys)
        {
            SendMessage(hWnd, WM_KEYUP, (IntPtr)keys, IntPtr.Zero);
        }

        //send text to handle
        public static void SendText(IntPtr hWnd, string text)
        {
                SendMessage(hWnd, WM_SETTEXT, 0, text);
        }

        //get text from handle
        public static string GetText(IntPtr hWnd)
        {

            // Get the size of the string required to hold the window title (including trailing null.)
            Int32 titleSize = SendMessage(hWnd, WM_GETTEXTLENGTH, 0, 0).ToInt32();

            // If titleSize is 0, there is no title so return an empty string (or null)
            if (titleSize == 0)
                return String.Empty;

            StringBuilder title = new StringBuilder(titleSize + 1);

            SendMessage(hWnd, WM_GETTEXT, title.Capacity, title);

            return title.ToString();
        }

    }
}