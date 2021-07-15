using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGameUtil
{
    public static class HandleSet
    {
        private static string srcPath = AppDomain.CurrentDomain.BaseDirectory + "src\\NameSet.txt";
        private static string outPath = AppDomain.CurrentDomain.BaseDirectory + "out\\NameSet.txt";
        private static string[] srcLines;
        private static HashSet<string> outSet;

        private static void Init()
        {
            Console.WriteLine("初始化HandleSetr开始");

            outSet = new HashSet<string>();
            srcLines = IOUtil.GetFileTextArr(IOUtil.GetFileText(srcPath));

            Console.WriteLine("初始化HandleSet完成");
        }

        public static void Handle()
        {
            Init();

            Console.WriteLine("开始处理");

            StringBuilder outSb = new StringBuilder();
            foreach (string line in srcLines)
            {
                if (!line.Contains("//"))
                {
                    if (outSet.Contains(line))
                    {
                        continue;
                    }
                    else
                    {
                        outSet.Add(line);
                    }
                }
                outSb.AppendLine(line);
            }
            IOUtil.CreateTextFile(outPath, outSb.ToString());

            Console.WriteLine("完成处理");
        }
    }
}
