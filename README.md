# Flags Basic for C#

This is a very simple command line flags library to make console application development easy and effortless.

Just add the Flags.cs file into your project and call it in Main. You're done. You can add your commands as simple classes with a static Run() method and they would be automatically picked up.

    class Program
    {
        static int Main(string[] args)
        {
            return Flags.ParseAndRun(args);
        }
    }

    class ListFiles
    {
        public static int Run(string path)
        {
            foreach(var file in Directory.GetFiles(path))
            {
                Console.WriteLine(file);
            }
            return 0;
        }
    }

That's it! you can get help and run:

    $ program help
    program -path <string>

    $ program -path /tmp
    /tmp/file1
    /tmp/file1
    ...

