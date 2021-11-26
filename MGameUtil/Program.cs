using Common;
using System;
using System.Collections.Generic;
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
                    List<string> paramLst = new();
                    for (int i = 1; i < inputs.Length; i++)
                    {
                        paramLst.Add(inputs[i]);
                    }
                    Handle(inputs[0], paramLst);
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

        private static void Handle(string key, List<string> paramLst = null)
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
                CodeStyleConverter.ConvertToOther(paramLst != null, paramLst[0]);
            }
            else if (key == "6")
            {
                CodeStyleConverter.ConvertToMine(paramLst != null, paramLst[0]);
            }
            else if (key == "7")
            {
                ReplaceUtil.ReplaceText(paramLst[0], paramLst[1], paramLst[2], paramLst[3]);
            }
            else if (new Regex(@"(.*)\|(.*)").IsMatch(key))
            {
                string[] input = key.Split("|");
                string exePath = int.Parse(input[0]) switch
                {
                    5 or 6 => @"E:\DailyTools\MGameUtil\bin\Debug\net6.0\MGameUtil.exe",
                    _ => @"E:\DailyTools\MGameUtil\bin\Debug\net6.0\MGameUtil.exe",
                };
                Process.Start(exePath, new string[] { @$"{key}" });
            }
            else if (key == "123")
            {
                Console.WriteLine("\t".Length);
            }
        }
    }
}
