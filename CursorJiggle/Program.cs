// https://www.pinvoke.net/default.aspx/user32.setcursorpos

using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace CursorJiggle
{

    internal class Program
    {
        private static Random rnd;
        private static int jiggleTracker;
        private static int newX;
        private static int newY;
        private static int inactivityTime;
        private static (int, int) previousCoordinates;
        private static (int, int) currentCoordinates;
        private static (int, int) initialCoordinates;

        private static int Counter { get; set; } = 0;

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]

        private static extern bool GetCursorPos(out POINT point);

        static void Main(string[] args)
        {
            inactivityTime = (args.Length == 0) ? 30 : Convert.ToInt16(args[0]);

            previousCoordinates = (0, 0);
            currentCoordinates = (0, 0);
            initialCoordinates = (0, 0);
            POINT point;
            rnd = new Random();
            jiggleTracker = 0;

            while (true)
            {
                GetCursorPos(out point);
                int x = point.X;
                int y = point.Y;

                currentCoordinates.Item1 = x;
                currentCoordinates.Item2 = y;


                //Console.WriteLine($"X: {x} Y: {y}");

                if (previousCoordinates == currentCoordinates)
                {
                    Counter++;
                }
                else
                {
                    previousCoordinates = currentCoordinates;
                    Counter = 0;
                }

                if (Counter == 100 * inactivityTime)
                {
                    initialCoordinates = currentCoordinates;
                }

                if (Counter > 100 * inactivityTime)
                {
                    switch (jiggleTracker)
                    {
                        case int n when (0 <= n && n <= 25):
                            Jiggle();
                            break;

                        case int n when (50 < n && n <= 75):
                            Jiggle();
                            break;
                    }

                    jiggleTracker++;

                    if (jiggleTracker == 3000)
                    {
                        jiggleTracker = 0;
                    }
                }
                
                Thread.Sleep(10);
            }


        }

        private static void Jiggle()
        {
            newX = rnd.Next(-10,10) + initialCoordinates.Item1;
            newY = rnd.Next(-10,10) + initialCoordinates.Item2;
            SetCursorPos(newX, newY);
            previousCoordinates.Item1 = newX;
            previousCoordinates.Item2 = newY;
        }
    }



    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static implicit operator System.Drawing.Point(POINT p)
        {
            return new System.Drawing.Point(p.X, p.Y);
        }

        public static implicit operator POINT(System.Drawing.Point p)
        {
            return new POINT(p.X, p.Y);
        }
    }
}
