using OpenCvSharp;
using ScriptImage;
using System;
using System.Runtime.InteropServices;
using System.Text;

public class DllHolder
{
    #region Dll FindWindow
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private protected static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private protected static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("user32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private protected static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr i);
    private protected delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);

    #endregion

    #region Dll WindowCapture 


    [DllImport("user32.dll")]
    private protected static extern IntPtr GetClientRect(IntPtr hWnd, ref Rect rect);

    [DllImport("user32.dll")]
    private protected static extern IntPtr ClientToScreen(IntPtr hWnd, ref System.Drawing.Point point);

    [DllImport("user32.dll")]
    private protected static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("gdi32.dll", SetLastError = true)]
    private protected static extern IntPtr CreateCompatibleDC(IntPtr hdc);

    [DllImport("gdi32.dll")]
    private protected static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

    [DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true, SetLastError = true)]
    private protected static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

    [DllImport("gdi32.dll")]
    private protected static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

    [DllImport("gdi32.dll")]
    private protected static extern bool DeleteObject(IntPtr hObject);

    [DllImport("gdi32.dll")]
    private protected static extern bool DeleteDC(IntPtr hObject);

    [DllImport("user32.dll")]
    private protected static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [DllImport("user32.dll")]
    private protected static extern bool SetForegroundWindow(IntPtr hWnd);
    #endregion

    #region Dll KeyBorder And Mouse 
    [DllImport("user32.dll")]
    private protected static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, int lParam);

    [DllImport("user32.dll")]
    private protected static extern bool GetCursorPos(out OpenCvSharp.Point lpPoint);

    [DllImport("user32.dll")]
    private protected static extern bool ScreenToClient(IntPtr hWnd, ref OpenCvSharp.Point lpPoint);
    #endregion

}

