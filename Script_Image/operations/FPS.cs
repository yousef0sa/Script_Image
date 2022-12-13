using System.Threading;
using System;

namespace ScriptImage
{
    public class FPS
    {
        private static  DateTime _lastCheckTime = DateTime.Now;
        private static long _frameCount = 0;


        private static DateTime LastCheckTime
        {
            get { return _lastCheckTime; }
            set { _lastCheckTime = value; }
        }
        
        // called whenever a map is updated
        private static void OnMapUpdated()
        {
            Interlocked.Increment(ref _frameCount);
        }

        // called every once in a while
        public static double GetFps()
        {
            OnMapUpdated();

            double secondsElapsed = (DateTime.Now - LastCheckTime).TotalSeconds;
            long count = Interlocked.Exchange(ref _frameCount, 0);
            double fps = count / secondsElapsed;
            LastCheckTime = DateTime.Now;
            return fps;
        }
    }
}
