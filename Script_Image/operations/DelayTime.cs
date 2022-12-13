using System.Threading;

namespace ScriptImage
{
    public class DelayTime
    {
        public static void Delay(double Time)
        {
            Time *= 1000;
            Thread.Sleep(((int)Time));
        }
    }
}
