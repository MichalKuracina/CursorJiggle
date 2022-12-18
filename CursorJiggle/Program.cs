// https://www.pinvoke.net/default.aspx/user32.setcursorpos

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

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
        private static int screenHeight;
        private static int screenWidth;

        private static int Counter { get; set; } = 0;

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]

        private static extern bool GetCursorPos(out POINT point);

        static void Main(string[] args)
        {
            screenHeight = Screen.PrimaryScreen.Bounds.Height;
            screenWidth = 0;
            foreach (Screen screen in Screen.AllScreens)
            {
                screenWidth += screen.Bounds.Width;
            }

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
                            Jiggle(1);
                            break;

                        case int n when (50 < n && n <= 100):
                            Jiggle(2);
                            break;
                        case int n when (125 < n && n <= 200):
                            Jiggle(3);
                            break;
                        case int n when (225 < n && n <= 325):
                            Jiggle(4);
                            break;
                    }

                    jiggleTracker++;

                    if (jiggleTracker == 100 * inactivityTime)
                    {
                        jiggleTracker = 0;
                    }
                }

                Thread.Sleep(10);
            }
        }

        private static void Jiggle(int amplifier)
        {
            if (initialCoordinates.Item1 < 10 & initialCoordinates.Item2 < 10)
            {
                return;
            }

            newX = rnd.Next(-10 * amplifier, 10 * amplifier) + initialCoordinates.Item1;
            newY = rnd.Next(-10 * amplifier, 10 * amplifier) + initialCoordinates.Item2;

            if (newX < 0) newX = 0 + 1;
            if (newY < 0) newY = 0 + 1;
            if (newX > screenWidth) newX = screenWidth - 10;
            if (newY > screenHeight) newY = screenHeight - 10;

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
