using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGameUtil
{
    public static class LuaEnumConverter
    {
        private static string srcPath = AppDomain.CurrentDomain.BaseDirectory + "src\\LuaEnum.txt";
        private static string outPath = AppDomain.CurrentDomain.BaseDirectory + "out\\LuaEnum.txt";
        private static string[] srcLines;

        private static void Init()
        {
            Console.WriteLine("初始化LuaEnum开始");
            srcLines = IOUtil.GetFileTextArr(IOUtil.GetFileText(srcPath));
            Console.WriteLine("初始化LuaEnum完成");
        }

        public static void Convert()
        {
            Init();

            Console.WriteLine("开始生成");

            int enumValue = 0;
            StringBuilder outSb = new StringBuilder();
            foreach (string line in srcLines)
            {
                string result;
                result = line.Trim();
                if (result == "")
                {
                    continue;
                }
                if (result.Contains(@"//"))
                {
                    result = line.Replace("//", "--");
                }
                if (result.StartsWith("--"))
                {
                    _ = int.TryParse(line[2..].Trim().Split(' ', '\t')[0].Trim(), out enumValue);
                }
                else if (result.Contains("="))
                {
                    string[] ss = result.Split('=');
                    string valueStr = ss[1].Trim();
                    if (valueStr.Contains(","))
                    {
                        _ = int.TryParse(valueStr.Substring(0, valueStr.IndexOf(',')), out enumValue);
                    }
                }
                else
                {
                    string enumName = result.Split(' ', ',')[0];
                    result = result.Replace(enumName, $"{enumName} = {enumValue++}");
                }
                outSb.AppendLine(result);
            }
            IOUtil.CreateTextFile(outPath, outSb.ToString());

            Console.WriteLine("完成生成");
        }
    }
}
