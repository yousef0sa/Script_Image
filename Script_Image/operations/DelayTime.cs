using System.Threading;

namespace ScriptImage
{
    public class DelayTime
    {
        //Delay Time in milliseconds
        public static void Delay(double seconds)
        {
            Thread.Sleep((int)(seconds * 1000));
        }
    }
}
