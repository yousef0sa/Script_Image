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

        //Timer start
        public static int TimerStart()
        {

            return System.Environment.TickCount / 1000;
        }

        //Timer end
        public static int TimerStop(int timer)
        {
            return (System.Environment.TickCount / 1000) - timer;
        }
    }
}
