using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ScriptImage;



public class WindowCapture
{
    #region DllImport
    [DllImport("user32.dll")]
    private static extern IntPtr GetClientRect(IntPtr hWnd, ref Rect rect);

    [DllImport("user32.dll")]
    private static extern IntPtr ClientToScreen(IntPtr hWnd, ref System.Drawing.Point point);

    [DllImport("user32.dll")]
    private static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("gdi32.dll", SetLastError = true)]
    private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

    [DllImport("gdi32.dll")]
    private static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

    [DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true, SetLastError = true)]
    static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

    [DllImport("gdi32.dll")]
    private static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

    [DllImport("gdi32.dll")]
    private static extern bool DeleteObject(IntPtr hObject);

    [DllImport("gdi32.dll")]
    private static extern bool DeleteDC(IntPtr hObject);

    [DllImport("user32.dll")]
    private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);
    #endregion

    private enum TernaryRasterOperations : uint
    {
        /// <summary>dest = source</summary>
        SRCCOPY = 0x00CC0020,
        /// <summary>dest = source OR dest</summary>
        SRCPAINT = 0x00EE0086,
        /// <summary>dest = source AND dest</summary>
        SRCAND = 0x008800C6,
        /// <summary>dest = source XOR dest</summary>
        SRCINVERT = 0x00660046,
        /// <summary>dest = source AND (NOT dest)</summary>
        SRCERASE = 0x00440328,
        /// <summary>dest = (NOT source)</summary>
        NOTSRCCOPY = 0x00330008,
        /// <summary>dest = (NOT src) AND (NOT dest)</summary>
        NOTSRCERASE = 0x001100A6,
        /// <summary>dest = (source AND pattern)</summary>
        MERGECOPY = 0x00C000CA,
        /// <summary>dest = (NOT source) OR dest</summary>
        MERGEPAINT = 0x00BB0226,
        /// <summary>dest = pattern</summary>
        PATCOPY = 0x00F00021,
        /// <summary>dest = DPSnoo</summary>
        PATPAINT = 0x00FB0A09,
        /// <summary>dest = pattern XOR dest</summary>
        PATINVERT = 0x005A0049,
        /// <summary>dest = (NOT dest)</summary>
        DSTINVERT = 0x00550009,
        /// <summary>dest = BLACK</summary>
        BLACKNESS = 0x00000042,
        /// <summary>dest = WHITE</summary>
        WHITENESS = 0x00FF0062
    }

    //From https://stackoverflow.com/a/46174804/20574919
    //Capture specific Window by using handle
    //Return Mat
    public static Mat Capture(IntPtr hWnd)
    {
        var rect = new Rect();
        GetClientRect(hWnd, ref rect);

        var point = new System.Drawing.Point(0, 0);
        ClientToScreen(hWnd, ref point);

        var bounds = new Rectangle(point.X, point.Y, rect.Right, rect.Bottom);

        IntPtr hWndDc = GetDC(hWnd);
        IntPtr hMemDc = CreateCompatibleDC(hWndDc);
        IntPtr hBitmap = CreateCompatibleBitmap(hWndDc, bounds.Width, bounds.Height);
        SelectObject(hMemDc, hBitmap);

        BitBlt(hMemDc, 0, 0, bounds.Width, bounds.Height, hWndDc, 0, 0, TernaryRasterOperations.SRCAND);
        using (Bitmap bitmap = Bitmap.FromHbitmap(hBitmap))
        {
            DeleteObject(hBitmap);
            ReleaseDC(hWnd, hWndDc);
            ReleaseDC(IntPtr.Zero, hMemDc);
            //DeleteDC(hMemDc);

            return bitmap.ToMat() ;
        }
    }


    //Capture Window image by location using handle
    public static Mat CaptureForeground(IntPtr handle)
    {
        var rect = new Rect();
        GetClientRect(handle, ref rect);

        var point = new System.Drawing.Point(0, 0);
        ClientToScreen(handle, ref point);

        var bounds = new Rectangle(point.X, point.Y, rect.Right, rect.Bottom);

        BringWindowToTop(handle);
        using (var result = new Bitmap(bounds.Width, bounds.Height))
        {
            using (var graphics = Graphics.FromImage(result))
            {
                graphics.CopyFromScreen(point.X, point.Y, 0, 0, result.Size);
            }
            return result.ToMat();

        }

    }


    public static bool BringWindowToTop(IntPtr handle)
    {
        if (handle != IntPtr.Zero)
        {
            return SetForegroundWindow((IntPtr)handle);
        }
        return false;
    }

}
