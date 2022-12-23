using OpenCvSharp;
using System;

namespace ScriptImage
{
    public class Mouse : DllHolder
    {
        private enum WMessages : uint
        {
            WM_RBUTTONDOWN = 0x204, //Right mouse-button down
            WM_RBUTTONUP = 0x205,   //Right mouse-button up
            WM_LBUTTONDOWN = 0x201, //Left  mouse-button down
            WM_LBUTTONUP = 0x202,   //Left  mouse-button up
            WM_MOUSEMOVE = 0x206,   //Mouse move
        }

        //mouse Left Click
        public static void Right_Click(IntPtr hWnd, (int X, int Y) Location, double delayTime = 0.5)
        {
            PostMessage(hWnd, (uint)WMessages.WM_RBUTTONDOWN, IntPtr.Zero, MakeLParam(Location.X, Location.Y));
            DelayTime.Delay(delayTime);
            PostMessage(hWnd, (uint)WMessages.WM_RBUTTONUP, IntPtr.Zero, MakeLParam(Location.X, Location.Y));
        }

        //mouse Left Click
        public static void Left_Click(IntPtr hWnd, (int X, int Y) Location, double delayTime = 0.5)
        {
            PostMessage(hWnd, (uint)WMessages.WM_LBUTTONDOWN, IntPtr.Zero, MakeLParam(Location.X, Location.Y));
            DelayTime.Delay(delayTime);
            PostMessage(hWnd, (uint)WMessages.WM_LBUTTONUP, IntPtr.Zero, MakeLParam(Location.X, Location.Y));
        }

        //Mouse double click
        public static void Double_Click(IntPtr hWnd, (int X, int Y) Location, double delayTime = 0.5)
        {
            PostMessage(hWnd, (uint)WMessages.WM_LBUTTONDOWN, IntPtr.Zero, MakeLParam(Location.X, Location.Y));
            DelayTime.Delay(delayTime);
            PostMessage(hWnd, (uint)WMessages.WM_LBUTTONUP, IntPtr.Zero, MakeLParam(Location.X, Location.Y));
            DelayTime.Delay(delayTime);
            PostMessage(hWnd, (uint)WMessages.WM_LBUTTONDOWN, IntPtr.Zero, MakeLParam(Location.X, Location.Y));
            DelayTime.Delay(delayTime);
            PostMessage(hWnd, (uint)WMessages.WM_LBUTTONUP, IntPtr.Zero, MakeLParam(Location.X, Location.Y));
        }

        //return LParam
        private static int MakeLParam(int LoWord, int HiWord)
        {
            return (HiWord << 16) | (LoWord & 0xFFFF);
        }

        //function to return mouse position
        public static (int X, int Y) GetCursorPosition(IntPtr hWnd = default)
        {
            GetCursorPos(out Point lpPoint);
            if (hWnd != IntPtr.Zero)
            {
                ScreenToClient(hWnd, ref lpPoint);
                return (lpPoint.X, lpPoint.Y);
            }
            else
                return (lpPoint.X, lpPoint.Y);
        }

        //move mouse
        public static void MoveMouse(IntPtr hWnd, (int X, int Y) Location, int speed = 1)
        {
            var cp = GetCursorPosition(hWnd);
            var (x1, y1) = Location;
            var (x2, y2) = (x1 - cp.X, y1 - cp.Y);
            var (x3, y3) = (x2 / speed, y2 / speed);
            for (int i = 0; i < speed; i++)
            {
                PostMessage(hWnd, (uint)WMessages.WM_MOUSEMOVE, IntPtr.Zero, MakeLParam(cp.X + x3, cp.Y + y3));
            }
        }

        //drag and drop.
        public static void DragAndDrop(IntPtr hWnd, (int X, int Y) Location, (int X, int Y) Location2, int speed = 1)
        {
            var cp = GetCursorPosition(hWnd);
            var (x1, y1) = Location;
            var (x2, y2) = (x1 - cp.X, y1 - cp.Y);
            var (x3, y3) = (x2 / speed, y2 / speed);
            for (int i = 0; i < speed; i++)
            {
                PostMessage(hWnd, (uint)WMessages.WM_MOUSEMOVE, IntPtr.Zero, MakeLParam(cp.X + x3, cp.Y + y3));
            }
            PostMessage(hWnd, (uint)WMessages.WM_LBUTTONDOWN, IntPtr.Zero, MakeLParam(Location.X, Location.Y));
            DelayTime.Delay(0.5);
            var cp2 = GetCursorPosition(hWnd);
            var (x5, y5) = Location2;
            var (x6, y6) = (x5 - cp2.X, y5 - cp2.Y);
            var (x7, y7) = (x6 / speed, y6 / speed);
            for (int i = 0; i < speed; i++)
            {
                PostMessage(hWnd, (uint)WMessages.WM_MOUSEMOVE, IntPtr.Zero, MakeLParam(cp2.X + x7, cp2.Y + y7));
            }
            PostMessage(hWnd, (uint)WMessages.WM_LBUTTONUP, IntPtr.Zero, MakeLParam(Location2.X, Location2.Y));
        }

        //return start position and end position by mouse of the window handle.
        public static ((int x, int y) Start, (int x, int y) End) RangeMaker(IntPtr hWnd)
        {
            //Delay Time
            DelayTime.Delay(0.5);

            //Get window Rectangle
            var rect = new Rect();
            GetClientRect(hWnd, ref rect);


            (int x, int y) Start = (0, 0);
            (int x, int y) End = (0, 0);

            //Start Position
            while (true)
            {
                var cp = GetCursorPosition(hWnd);
                //When the mouse is inside the handle window == True
                if (cp.X <= rect.BottomRight.X && cp.Y <= rect.BottomRight.Y && cp.X >= 0 && cp.Y >= 0)
                {
                    if (GetAsyncKeyState(Keys.LButton) == true)
                    {
                        Start = cp;
                        break;
                    }
                    if (GetAsyncKeyState(Keys.Escape) == true)
                        return (Start, End); ;
                }
            }

            //Delay Time
            DelayTime.Delay(0.5);

            //End Position
            while (true)
            {
                var cp = GetCursorPosition(hWnd);
                //When the mouse is inside the handle window == True
                if (cp.X <= rect.BottomRight.X && cp.Y <= rect.BottomRight.Y && cp.X >= 0 && cp.Y >= 0)
                {
                    if (GetAsyncKeyState(Keys.LButton) == true)
                    {
                        End = cp;
                        break;
                    }
                    if (GetAsyncKeyState(Keys.Escape) == true)
                        return (Start, End); ;
                }
            }
            return (Start, End);
        }
    }
}
