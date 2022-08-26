using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace CursorJiggle
{

    internal class Program
    {
        private static bool point2;

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]



        static extern bool GetCursorPos(out POINT point);

        static void Main(string[] args)
        {

            POINT point;
            //GetCursorPos(out point);
            //SetCursorPos(0,0);

            while (true)
            {
                GetCursorPos(out point);
                int x = point.X;
                int y = point.Y;
                Console.WriteLine($"X: {x} Y: {y}");
                Thread.Sleep(500);
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
