using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace CursorJiggle
{

    internal class Program
    {
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]

        // https://www.pinvoke.net/default.aspx/user32.setcursorpos



        public static extern bool GetCursorPos(out POINT point);
        public static int Counter { get; set; } = 0;
        public static bool Jiggle { get; set; } = false;

        static void Main(string[] args)
        {
            var previousCoordinates = (0, 0);
            var currentCoordinates = (0, 0);
            POINT point;

            //SetCursorPos(0,0);

            while (true)
            {
                GetCursorPos(out point);
                int x = point.X;
                int y = point.Y;

                currentCoordinates.Item1 = x;
                currentCoordinates.Item2 = y;


                Console.WriteLine($"X: {x} Y: {y}");

                if (true)
                {

                }






                Counter++;
                Thread.Sleep(10);
            }


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
