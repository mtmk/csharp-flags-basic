# Flags Basic for C#

This is a very simple command line flags library to make console application development easy and effortless.

Just add the FlagsBasic/Flags.cs file into your project and call it in Main. You're done. You can add your commands as simple classes with a static Run() method and they would be automatically picked up.

## Dot Net Versions

I have developed this on macOS using donet 2.0.0. Currently in the process of creating a nuget package for all popular versions (e.g. .Net 4.5) but hopefuly if you include the file you should be able to use the code without much change.

```csharp
class Program
{
    static int Main(string[] args)
    {
        return Flags.ParseAndRun(args);
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
```

That's it! you can get help and run it:

```shell
> example.exe help
Usage:
    Example -path <string> [ -pattern <string>|* ] [ -recursive ]

> example.exe -path /tmp
/tmp/file1
/tmp/file1
...
```
