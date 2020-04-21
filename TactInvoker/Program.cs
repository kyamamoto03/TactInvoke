using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace TactInvoker
{
    class Program
    {

        public const int INPUT = 0;
        public const int OUTPUT = 1;

        public const int INT_EDGE_FALLING = 1;
        public const int INT_EDGE_RISING = 2;

        public const int Tact_PIN = 17;

        [DllImport("wiringPi")]
        extern static int wiringPiSetupGpio();

        [DllImport("wiringPi")]
        extern static void pinMode(int pin, int mode);

        [DllImport("wiringPi")]
        extern static void digitalWrite(int pin, int mode);


        [DllImport("wiringPi")]
        extern static int wiringPiISR(int pin, int edgeType, CallbackFunc func);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CallbackFunc();

        static void Main(string[] args)
        {
            int ret = 0;

           wiringPiSetupGpio();

            pinMode(Tact_PIN, INPUT);

            CallbackFunc callBackFunc = delegate () {
                Console.WriteLine("CallbackFunc is called !");

            };

            ret = wiringPiISR(Tact_PIN, INT_EDGE_RISING, callBackFunc);

            Thread.Sleep(Int32.MaxValue);

        }
    }
}
