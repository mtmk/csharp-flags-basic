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
        public static string ProgramName = System.AppDomain.CurrentDomain.FriendlyName;
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
            var name = OnlyOne(ts) ?? args[0];
            var used = new HashSet<int>();

            foreach (var t in ts)
            {
                if (R(t.Name) != name) continue;

                var m = t.GetMethod(FlagsConfig.RunMethodName);
                if (m != null && m.IsPublic && m.IsStatic && m.ReturnType == typeof(int))
                {
                    var vs = new List<object>();

                    if (args.Length > 0 && args[0] == name) used.Add(0);

                    foreach(var p in m.GetParameters())
                    {
                        var pn = R(p.Name);
                        object v = null;
                        if (p.IsOptional) v = p.RawDefaultValue;

                        var found = p.IsOptional;

                        if (p.ParameterType == typeof(bool))
                        {
                            v = false;
                            found = true;
                        }

                        for (int i = 0; i < args.Length; i++)
                        {
                            if (used.Contains(i)) continue;

                            if (p.ParameterType == typeof(bool) && args[i] == "-"+pn)
                            {
                                used.Add(i);
                                v = true;
                                break;
                            }
                            else 
                            {
                                if (args[i] == "-"+pn && args.Length > i+1)
                                {
                                    used.Add(i);
                                    used.Add(i + 1);
                                    try
                                    {
                                        v = Convert.ChangeType(args[i+1], p.ParameterType);
                                        found = true;
                                        break;
                                    }
                                    catch (System.FormatException e)
                                    {
                                        Console.Error.WriteLine("Argument error: -{0} {1}: {2}", pn, args[i+1], e.Message);
                                        Help(ts, name);
                                        return 2;
                                    }
                                }
                            }
                        }

                        if (!found)
                        {
                            Console.Error.WriteLine("Argument -{0} was not found", pn);
                            Help(ts, name);
                            return 2;
                        }

                        vs.Add(v);
                    }

                    var unknown = new List<string>();
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (used.Contains(i)) continue;
                        unknown.Add(args[i]);
                    }
                    if (unknown.Count > 0)
                    {
                        Console.Error.WriteLine("Unknown argument: {0}", string.Join(" ", unknown));
                        Help(ts, name);
                        return 2;
                    }

                    try 
                    {
                        return (int) t.InvokeMember(FlagsConfig.RunMethodName,
                                                    BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod,
                                                    null, null, vs.ToArray());
                    } 
                    catch (TargetInvocationException e) 
                    {
                        var be = e.GetBaseException();
                        var bet = be.GetType().Name.Replace("Exception", "");
                        if (bet != "")
                        {
                            bet = Regex.Replace(bet, "([a-z])([A-Z])", r => r.Groups[1].Value + " " + r.Groups[2].Value.ToLower()) + ": ";
                        }
                        Console.Error.WriteLine("Error: {0}{1}", bet, be.Message);
                        return 2;
                    }
                }
            }

            Console.Error.WriteLine("Error: Cannot find any matching command for argument '{0}'", name);
            Help(ts);
            return 2;
        }

        static void Help(Type[] ts, string name = null)
        {
            var onlyOne = OnlyOne(ts);
            var program = FlagsConfig.ProgramName;

            Console.Error.WriteLine("Usage:");

            foreach (var t in ts)
            {
                var m = t.GetMethod(FlagsConfig.RunMethodName);
                if (m != null && m.IsPublic && m.IsStatic && m.ReturnType == typeof(int))
                {
                    var tn = R(t.Name);

                    if (name != null && name != tn) continue;

                    Console.Error.Write("  {0} ", program);
                    if (onlyOne == null) Console.Error.Write("{0} ", tn);

                    foreach(var p in m.GetParameters())
                    {
                        var pn = R(p.Name);
                        if (p.ParameterType == typeof(bool))
                        {
                            Console.Error.Write("[ -{0} ] ", pn);
                        }
                        else
                        {
                            var opt1 = p.IsOptional ? "[ " : "";
                            var opt2 = p.IsOptional ? string.Format("|{0} ]", p.RawDefaultValue==null?"<null>":p.RawDefaultValue) : "";
                            Console.Error.Write("{0}-{1} <{2}>{3} ", opt1, pn, R(p.ParameterType.Name).Replace("int32", "int"), opt2);
                        }
                    }

                    Console.Error.WriteLine();
                }
            }
        }

        static string OnlyOne(Type[] ts)
        {
            int i = 0;
            string name = null;
            foreach (var t in ts)
            {
                var m = t.GetMethod(FlagsConfig.RunMethodName);
                if (m != null && m.IsPublic && m.IsStatic && m.ReturnType == typeof(int))
                {
                    name = R(t.Name);
                    i++;
                }
            }
            return i == 1 ? name : null;
        }
    }
}
