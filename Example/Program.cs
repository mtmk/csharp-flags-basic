using System;
using System.IO;

namespace Example
{
    class Program
    {
        static int Main(string[] args)
        {
            return FlagsBasic.Flags.ParseAndRun(args);
        }
    }

    class ListFiles
    {
        public static int Run(string path, string pattern = "*", bool recursive = false)
        {
            foreach(var file in Directory.GetFiles(path, pattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
            {
                Console.WriteLine(file);
            }
            return 0;
        }
    }
}
