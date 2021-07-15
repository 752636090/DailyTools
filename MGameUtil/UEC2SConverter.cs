using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGameUtil
{
    public static class UEC2SConverter
    {
        private static string srcPath = AppDomain.CurrentDomain.BaseDirectory + "src\\UEC2S.txt";
        private static string outPath = AppDomain.CurrentDomain.BaseDirectory + "out\\UEC2S.txt";
        private static string[] srcLines;
        private static HashSet<string> outSet;

        private static void Init()
        {
            Console.WriteLine("初始化UEC2SConverter开始");

            outSet = new HashSet<string>();
            srcLines = IOUtil.GetFileTextArr(IOUtil.GetFileText(srcPath));

            Console.WriteLine("初始化UEC2SConverter完成");
        }

        public static void Convert()
        {
            Init();

            Console.WriteLine("开始转换UEC2S");

            foreach (string line in srcLines)
            {
                if (line.Contains("local data = "))
                {
                    string name = line.Replace("local data = ", "").Replace("();", "").Replace("()", "").Replace("\t", "").Replace(" ", "");
                    if (!outSet.Contains(name))
                    {
                        outSet.Add(name);
                    }
                }
            }

            StringBuilder outSb = new StringBuilder();
            foreach (string name in outSet)
            {
                outSb.AppendLine($"\"{name}\",");
            }
            IOUtil.CreateTextFile(outPath, outSb.ToString());

            Console.WriteLine("完成转换UEC2S");
        }
    }
}
