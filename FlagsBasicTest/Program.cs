using System;
using FlagsBasic;

namespace FlagsBasicTest
{
    class Program
    {
        static int Main(string[] args)
        {
            return Flags.ParseAndRun(args);
        }
    }

    class MyCmd
    {
        public static int Run(string argBla1, int num1, bool b1, int num2 = 42, string argFoo2 = "fourtee two", bool b2 = false)
        {
            Console.WriteLine($"You said arg1: {argBla1} {num1} {num2} {argFoo2} {b1} {b2}");
            return 0;
        }
    }
}
