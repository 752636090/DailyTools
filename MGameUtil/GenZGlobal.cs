using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGameUtil
{
    public static class GenZGlobal
    {
        private static string srcPath = AppDomain.CurrentDomain.BaseDirectory + "out\\NameSet.txt";
        private static string outPath = AppDomain.CurrentDomain.BaseDirectory + "out\\ZGlobal.lua";
        private static string[] srcLines;

        private static void Init()
        {
            Console.WriteLine("初始化GenZGlobal开始");
            srcLines = IOUtil.GetFileTextArr(IOUtil.GetFileText(srcPath));
            Console.WriteLine("初始化GenZGlobal完成");
        }

        public static void Gen()
        {
            Init();

            Console.WriteLine("开始生成");

            StringBuilder outSb = new StringBuilder();
            foreach (string line in srcLines)
            {
                string result;
                if (line.Contains(@"//"))
                {
                    result = line.Replace("//", "--");
                }
                else if (line.Length < 3)
                {
                    continue;
                }
                else
                {
                    string name = line.Replace("\"", "").Replace(",", "");
                    result = $"{name} = _G.{name}";
                }
                outSb.AppendLine(result);
            }
            IOUtil.CreateTextFile(outPath, outSb.ToString());

            Console.WriteLine("完成生成");
        }
    }
}
