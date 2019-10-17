# Flags Basic for C#

This is a very simple command line flags library to make console application development easy and effortless.

Just add the [FlagsBasic NuGet package](https://www.nuget.org/packages/FlagsBasic/) to your project and you're done.
It supports Net Standard 2.0 as well as older .Net Framework versions as old as .Net Framework 2.0.

```shell
> dotnet new console
> dotnet add package flagsbasic
```

## Usage

The idea is simple. .Net has amazing reflection capabilities and there is already a ton of meta data about the code
in .Net assembilies. Using this information only we inspect the current assembly and run the selected method.
Method selection is done by matching the first command line argument to a class name, then using the static Run()
method of that class, passing the rest of the command line arguments as the parameters to the Run() method.

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

class Cat
{
    public static int Run(string path)
    {
        using var sr = new StreamReader(path);
        while (true)
        {
            var line = sr.ReadLine();
            if (line == null) break;
            Console.WriteLine(line);
        }
        return 0;
    }
}
```

That's it! you can get help and run it:

```shell
> example.exe help
Usage:
    Example list-files -path <string> [ -pattern <string>|* ] [ -recursive ]
    Example cat -path <string>

> example.exe list-files -path /tmp -recursive
/tmp/file1
/tmp/dir1/file1

> example.exe cat -path /tmp/file1
```

### Supported types

String is the most common type as well as other numeric types. We match the name of the parameter to the argument
begining with a dash (-) then parse the next argument using .Net Convert functionality. Only boolean typed parameters
are treated a little differently by using them as flags with no value followed by it. Because it's only set to
true if existed and currently there is no way to negate them, it only makes sense for bool parameters to be default to false.

Optional parameters are supported too.
