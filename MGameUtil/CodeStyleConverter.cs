using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MGameUtil
{
    public static class CodeStyleConverter
    {
        private static List<FileInfo> fileLst;
        private static Regex regexClass = new("class \"(.*)\"");
        private static Regex regexFunctionMine = new(@"function (.*):(.*)\((.*)\)");
        private static Regex regexFunctionMineStatic = new(@"function (.*).(.*)\((.*)\)");
        private static Regex regexFunctionOther = new(@"function (.*)\((.*)\)");


        private static void Init()
        {
            fileLst = new List<FileInfo>();

            string configPath = AppDomain.CurrentDomain.BaseDirectory + "Configs\\CodeStyleConfig.xml";
            XDocument doc = XDocument.Load(configPath);

            IEnumerable<XElement> fileElems = doc.Root.Element("Files").Elements("Item");
            foreach (XElement item in fileElems)
            {
                string path = item.Attribute("Path").Value;
                FileInfo file = new(path);
                fileLst.Add(file);
            }
        }

        public static void ConvertToOther()
        {
            Init();

            if (!IsNeedConvert(fileLst, regexFunctionOther))
            {
                return;
            }

            foreach (FileInfo file in fileLst)
            {
                StringBuilder sb = new StringBuilder();
                Stack<(string, int)> classStack = new(); // (类名, 前面几个tab)
                string[] content = IOUtil.GetFileTextArr(file);
                for (int iLine = 0; iLine < content.Length; iLine++)
                {
                    string line = content[iLine];

                    if (CollectClassInfo(line, classStack))
                    {
                        sb.AppendLine(line);
                        continue;
                    }


                    Match matchFunc = regexFunctionMine.Match(line);
                    if (matchFunc.Success)
                    {
                        string className = classStack.Peek().Item1; // matchFunc.Groups[1].Value也是className
                        string funcName = matchFunc.Groups[2].Value;
                        string param = matchFunc.Groups[3].Value;
                        line = line.Replace($"{matchFunc.Groups[1].Value}:", "");
                        if (matchFunc.Groups[3].Value == "")
                        {
                            line = line.Replace($"{funcName}()", $"{funcName}(self)");
                        }
                        else
                        {
                            line = line.Replace($"{param}", $"self, {param}");
                        }
                    }
                    else
                    {
                        Match matchFuncStatic = regexFunctionMineStatic.Match(line);
                        if (matchFuncStatic.Success)
                        {
                            string className = classStack.Peek().Item1;
                            line = line.Replace($"{className}.", "");
                        }
                    }

                    sb.AppendLine(line);
                }
                string ret = sb.ToString();
                ret = ret.Substring(0, ret.LastIndexOf("\r\n"));
                IOUtil.CreateTextFile(file.FullName, ret);
                Console.WriteLine($"{file.Name}转换完成");
            }
            Console.WriteLine($"全部转换完成");
        }

        public static void ConvertToMine()
        {
            Init();

            if (!IsNeedConvert(fileLst, regexFunctionMine))
            {
                return;
            }

            foreach (FileInfo file in fileLst)
            {
                StringBuilder sb = new StringBuilder();
                Stack<(string, int)> classStack = new(); // (类名, 前面几个tab)
                string[] content = IOUtil.GetFileTextArr(file);
                for (int iLine = 0; iLine < content.Length; iLine++)
                {
                    string line = content[iLine];

                    if (CollectClassInfo(line, classStack))
                    {
                        sb.AppendLine(line);
                        continue;
                    }


                    Match matchFunc = regexFunctionOther.Match(line);
                    if (matchFunc.Success)
                    {
                        string className = classStack.Peek().Item1;
                        string funcName = matchFunc.Groups[1].Value;
                        string symbol = line.Contains($"(self") ? ":" : ".";
                        line = line.Replace($"{funcName}", $"{className}{symbol}{funcName}");
                        if (matchFunc.Groups[2].Value == "self")
                        {
                            line = line.Replace($"{funcName}(self)", $"{funcName}()");
                        }
                        else
                        {
                            line = line.Replace($"self, ", $"");
                        }
                    }

                    sb.AppendLine(line);
                }
                string ret = sb.ToString();
                ret = ret.Substring(0, ret.LastIndexOf("\r\n"));
                IOUtil.CreateTextFile(file.FullName, ret);
                Console.WriteLine($"{file.Name}转换完成");
            }
            Console.WriteLine($"全部转换完成");
        }

        private static bool CollectClassInfo(string line, Stack<(string, int)> classStack)
        {
            #region 获得类名
            Match matchClass = regexClass.Match(line);
            if (matchClass.Success)
            {
                classStack.Push((matchClass.Groups[1].Value, (line.Length - line.Replace("    ", "").Replace("\t", "").Length) / 4));
                return true;
            }
            #endregion

            #region 判断类是否结束并从栈中移除
            if (classStack.Count == 0)
            {
                return false;
            }
            string endStrNoFormated = "";
            for (int i = 0; i < classStack.Peek().Item2; i++)
            {
                endStrNoFormated += "\t";
            }
            endStrNoFormated += "end)";
            string endStrFormated = endStrNoFormated.Replace("end", "");
            string endStrNoFormatedNoTab = endStrNoFormated.Replace("\t", "    ");
            string endStrFormatedNoTab = endStrFormated.Replace("\t", "    ");
            if (line.StartsWith(endStrNoFormated) || line.StartsWith(endStrFormated) || line.StartsWith(endStrNoFormatedNoTab) || line.StartsWith(endStrFormatedNoTab))
            {
                classStack.Pop();
                return true;
            }
            #endregion

            return false;
        }

        private static bool IsNeedConvert(List<FileInfo> fileLst, Regex errorRegex)
        {
            Console.WriteLine("开始检查是否需要转换");
            bool needConvert = true;
            foreach (FileInfo file in fileLst)
            {
                if (!_IsNeedConvert(file, errorRegex))
                {
                    Console.WriteLine($"{file.FullName}已经转换过了，接下来所有文件都不转换");
                    needConvert = false;
                }
            }
            Console.WriteLine("检查结束，开始转换");
            return needConvert;
        }

        private static bool _IsNeedConvert(FileInfo file, Regex errorRegex)
        {
            string content = IOUtil.GetFileText(file.FullName);
            Match matchError = errorRegex.Match(content);
            return !(matchError.Success && !matchError.Groups[1].Value.Contains(":"));
        }
    }
}

/*
class "DlgAdventureName" (function(_ENV)
    inherit (UEDlgPanel)

    class "TestClass" (function(_ENV)
        ----------------------------------------------
        ------------------ Property ------------------
        ----------------------------------------------
    
        __DebugArguments__{}
        function TestClass(self)
            Super(self);
        end

        __DebugArguments__{ }
        function TestFunc(self, data)
            
        end
        
        __DebugArguments__{ }
        __DebugStatic__() function TestStatic1()
            
        end
        
        __DebugArguments__{ }
        __DebugStatic__() function TestStatic2(data)
            
        end
    
        ----------------------------------------------
        ------------------- Method -------------------
        ----------------------------------------------
    
    end)
end)
 */