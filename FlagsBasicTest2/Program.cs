using System;
using FlagsBasic;

namespace FlagsBasicTest2
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
        public static int Run(string s1, int n1, bool b1, int n2 = 42, string s2 = "two")
        {
            Console.WriteLine($"cmd:MyCmd s1:{s1} n1:{n1} b1:{b1} n2:{n2} s2:{s2}");
            return 0;
        }
    }

    class MyCmd2
    {
        public static int Run(string s1, int n1, bool b1, int n2 = 42, string s2 = "two")
        {
            Console.WriteLine($"cmd:MyCmd2 s1:{s1} n1:{n1} b1:{b1} n2:{n2} s2:{s2}");
            return 0;
        }
    }

    class MyCmd3
    {
        public static int Run(string s1, int n1, bool b1, int n2 = 42, string s2 = "two")
        {
            throw new Exception("Boom!");
        }
    }

    class MyCmd4
    {
        public static int Run(string s1, int n1, bool b1, int n2 = 42, string s2 = "two")
        {
            throw new InvalidOperationException("Boom again!");
        }
    }
}
