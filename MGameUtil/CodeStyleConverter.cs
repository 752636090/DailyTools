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
        private static Regex regexFunctionOther = new(@"function (.*)\(self(.*)\)");

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
                        line = line.Replace($"{matchFunc.Groups[1].Value}:", "");
                        if (matchFunc.Groups[3].Value == "")
                        {
                            line = line.Replace($"{matchFunc.Groups[2].Value}()", $"{matchFunc.Groups[2].Value}(self)");
                        }
                        else
                        {
                            line = line.Replace($"{matchFunc.Groups[3].Value}", $"self, {matchFunc.Groups[3].Value}");
                        }
                    }

                    sb.AppendLine(line);
                }
                string ret = sb.ToString();
                ret = ret.Substring(0, ret.LastIndexOf("\r\n"));
                IOUtil.CreateTextFile(file.FullName, ret);
            }
        }

        public static void ConvertToMine()
        {
            Init();
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
                        line = line.Replace($"{matchFunc.Groups[1].Value}", $"{classStack.Peek().Item1}:{matchFunc.Groups[1].Value}");
                        if (matchFunc.Groups[2].Value == "")
                        {
                            line = line.Replace($"self", $"");
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
            }
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
    }
}

/*
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
    
        ----------------------------------------------
        ------------------- Method -------------------
        ----------------------------------------------
    
    end)
 */