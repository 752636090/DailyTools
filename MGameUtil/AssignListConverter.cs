using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGameUtil
{
    public static class AssignListConverter
    {
        private static string srcPath = AppDomain.CurrentDomain.BaseDirectory + "src\\AssignList.txt";
        private static string outPath = AppDomain.CurrentDomain.BaseDirectory + "out\\AssignList.txt";
        private static string[] srcLines;

        private static void Init()
        {
            Console.WriteLine("初始化AssignListConverter开始");

            srcLines = IOUtil.GetFileTextArr(IOUtil.GetFileText(srcPath));

            Console.WriteLine("初始化AssignListConverter完成");
        }

        public static void Convert()
        {
            Init();

            Console.WriteLine("开始转换AssignList");

            StringBuilder outSb = new StringBuilder();

            foreach (string line in srcLines)
            {
                if (line.Length < 5 || line.StartsWith("--"))
                {
                    continue;
                }
                //if (line.Contains("P[ProtocolType."))
                //{
                int iStart = line.IndexOf("=") + 2;
                int iEnd = line.LastIndexOf(";");
                string name = line.Substring(iStart, iEnd - iStart);
                name = $"\"{name}\",";
                outSb.AppendLine(name);
                //}
            }

            IOUtil.CreateTextFile(outPath, outSb.ToString());

            Console.WriteLine("完成转换AssignList");
        }
    }
}
