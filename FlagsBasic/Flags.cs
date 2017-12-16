using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace FlagsBasic
{
    public static class FlagsConfig
    {
        public static string RunMethodName = "Run";
        public static Func<Assembly> GetAssembly = () => Assembly.GetEntryAssembly();
    }

    public class Flags
    {
        static string R(string input)
        {

            return Regex.Replace(input, "([a-z])([A-Z])", "$1-$2").ToLower();
        }

        public static int ParseAndRun(string[] args)
        {
            var a = FlagsConfig.GetAssembly();
            var ts = a.GetTypes();

            if (args.Length == 0 || args[0] == "help") { Help(ts); return 2; }

            return Run(args, ts);
        }

        static int Run(string[] args, Type[] ts)
        {
            foreach (var t in ts)
            {
                if (R(t.Name) != args[0]) continue;

                var m = t.GetMethod(FlagsConfig.RunMethodName);
                if (m != null && m.IsPublic && m.IsStatic && m.ReturnType == typeof(int))
                {
                    var vs = new List<object>();

                    foreach(var p in m.GetParameters())
                    {
                        var pn = R(p.Name);
                        object v = null;
                        if (p.IsOptional) v = p.RawDefaultValue;

                        var found = p.IsOptional;
                        if (args.Length > 1)
                        {
                            for (int i = 1; i < args.Length; i++)
                            {
                                // TODO: Console.Error.WriteLine($"{t.Name} {i} {args.Length} {args[i]}");
                                if (p.ParameterType == typeof(bool))
                                {
                                    
                                }
                                else 
                                {
                                    if (args[i] == $"-{pn}" && args.Length > i+1)
                                    {
                                        v = Convert.ChangeType(args[i+1], p.ParameterType);
                                        found = true;
                                    }
                                }
                            }
                        }

                        if (!found)
                        {
                            Console.Error.WriteLine($"Argument error: {pn}");
                            Help(ts);
                            return 2;
                        }

                        vs.Add(v);
                    }

                    try 
                    {
                        return (int) t.InvokeMember(FlagsConfig.RunMethodName,
                            BindingFlags.Public | BindingFlags.NonPublic | 
                            BindingFlags.Static | BindingFlags.InvokeMethod, null, null, vs.ToArray());
                    } 
                    catch (TargetInvocationException e) 
                    {
                        Console.Error.WriteLine($"ERROR: {e.InnerException.Message}");
                        return 2;
                    }
                }
            }

            Help(ts);
            return 2;
        }

        static void Help(Type[] ts)
        {
            foreach (var t in ts)
            {
                var m = t.GetMethod(FlagsConfig.RunMethodName);
                if (m != null && m.IsPublic && m.IsStatic && m.ReturnType == typeof(int))
                {
                    var tn = R(t.Name);

                    Console.Error.Write($" {tn} ");

                    foreach(var p in m.GetParameters())
                    {
                        var pn = R(p.Name);
                        var opt1 = p.IsOptional ? "[ " : "";
                        var opt2 = p.IsOptional ? $"|{p.RawDefaultValue??"<null>"} ]" : "";
                        Console.Error.Write($"{opt1}-{pn} <{R(p.ParameterType.Name).Replace("int32", "int")}>{opt2} ");
                    }

                    Console.Error.WriteLine();
                }
            }
        }
    }
}
