using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;

namespace MGameUtil
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                foreach (string item in args)
                {
                    Thread.Sleep(20);
                    string[] inputs = item.Split('|');
                    Handle(inputs[0], inputs[1]);
                }
                return;
            }

            while (true)
            {
                Thread.Sleep(20);
                string key = Console.ReadLine();
                Handle(key);
            }
        }

        private static void Handle(string key, string param = null)
        {
            if (key == "1")
            {
                UEC2SConverter.Convert();
            }
            else if (key == "2")
            {
                AssignListConverter.Convert();
            }
            else if (key == "3")
            {
                HandleSet.Handle();
            }
            else if (key == "4")
            {
                GenZGlobal.Gen();
            }
            else if (key == "5")
            {
                CodeStyleConverter.ConvertToOther(param != null, param);
            }
            else if (key == "6")
            {
                CodeStyleConverter.ConvertToMine(param != null, param);
            }
            else if (new Regex(@"(.*)\|(.*)").IsMatch(key))
            {
                string[] input = key.Split("|");
                string exePath;
                switch (int.Parse(input[0]))
                {
                    case 5:
                    case 6:
                        exePath = @"E:\DailyTools\MGameUtil\bin\Debug\net5.0\MGameUtil.exe";
                        break;
                    default:
                        exePath = @"E:\DailyTools\MGameUtil\bin\Debug\net5.0\MGameUtil.exe";
                        break;
                }
                Process.Start(exePath, new string[] { @$"{key}" });
            }
            else if (key == "123")
            {
                Console.WriteLine("\t".Length);
            }
        }
    }
}
