using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;

namespace CodeStyleConverter
{
    class Program
    {
        private static List<FileInfo> fileLst;
        private static List<string> localDefineLst;
        private static List<MyDic> dic2MineLst;
        private static List<MyDic> dic2OtherLst;
        private static List<string> singletonLst;

        static void Main(string[] args)
        {
            Init();
            Console.WriteLine("初始化成功");
            while (true)
            {
                Thread.Sleep(20);
                string key = Console.ReadLine();
                if (key == "0")
                {
                    Init();
                    Console.WriteLine("初始化成功");
                }
                else if (key == "1")
                {
                    Console.WriteLine("开始转换成别人代码");
                    foreach (FileInfo file in fileLst)
                    {
                        ProcessFileToOther(file);
                    }
                    Console.WriteLine("转换成别人代码成功");
                }
                else if (key == "2")
                {
                    Console.WriteLine("开始转换成自己代码");
                    foreach (FileInfo file in fileLst)
                    {
                        ProcessFileToMine(file);
                    }
                    Console.WriteLine("转换成自己代码成功");
                }
            }
        }

        private static void Init()
        {
            fileLst = new List<FileInfo>();
            localDefineLst = new List<string>();
            dic2OtherLst = new List<MyDic>();
            dic2MineLst = new List<MyDic>();
            singletonLst = new List<string>();

            string configPath = AppDomain.CurrentDomain.BaseDirectory + "Configs\\Config.xml";
            XDocument doc = XDocument.Load(configPath);

            IEnumerable<XElement> fileElems = doc.Root.Element("Files").Elements("Item");
            foreach (XElement item in fileElems)
            {
                string path = item.Attribute("Path").Value;
                FileInfo file = new(path);
                fileLst.Add(file);
            }

            IEnumerable<XElement> folderElems = doc.Root.Element("Folders").Elements("Item");
            foreach (XElement item in folderElems)
            {
                string path = item.Attribute("Path").Value;
                DirectoryInfo directory = new(path);
                FileInfo[] files = directory.GetFiles("*.lua", SearchOption.AllDirectories);
                fileLst.AddRange(files);
            }

            IEnumerable<XElement> codeElems = doc.Root.Element("LocalDefines").Elements("Item");
            foreach (XElement item in codeElems)
            {
                string value = item.Attribute("CodeLine").Value;
                localDefineLst.Add(value);
            }

            IEnumerable<XElement> dic2MineElems = doc.Root.Element("Dic2Mine").Elements("Item");
            foreach (XElement item in dic2MineElems)
            {
                dic2MineLst.Add(new MyDic()
                {
                    Other = item.Attribute("Other").Value,
                    Mine = item.Attribute("Mine").Value,
                    IsRegex = item.Attribute("IsRegex").Value == "1",
                    NotContainsStrs = item.Attribute("NotContains")?.Value.Split("|OR|"),
                });
            }

            IEnumerable<XElement> dic2OtherElems = doc.Root.Element("Dic2Other").Elements("Item");
            foreach (XElement item in dic2OtherElems)
            {
                dic2OtherLst.Add(new MyDic()
                {
                    Other = item.Attribute("Other").Value,
                    Mine = item.Attribute("Mine").Value,
                    IsRegex = item.Attribute("IsRegex").Value == "1",
                    NotContainsStrs = item.Attribute("NotContains")?.Value.Split("|OR|"),
                });
            }

            IEnumerable<XElement> singletonElems = doc.Root.Element("Singletons").Elements("Item");
            foreach (XElement item in singletonElems)
            {
                string value = item.Attribute("Item").Value;
                singletonLst.Add(value);
            }
        }

        private static void ProcessFileToOther(FileInfo file)
        {
            List<MyDic> runDicLst = CreateRunDicLst(file, true);

            StringBuilder sb = new();
            string[] lines = IOUtil.GetFileTextArr(file);
            for (int iLine = 0; iLine < lines.Length; iLine++)
            {
                string line = lines[iLine];
                foreach (string item in localDefineLst)
                {
                    if (line.Contains(item))
                    {
                        string tmp = line;
                        string spaces = "";
                        while (tmp[0] == ' ')
                        {
                            tmp = tmp[1..];
                            spaces += " ";
                        }
                        if (!(tmp[0] == '-' && tmp[1] == '-' && tmp[2] == ' '))
                        {
                            line = spaces + "-- " + tmp;
                        }
                        if (item.Contains("local "))
                        {
                            string comment = " -- 此变量用于代码提示，没有任何功能性作用";
                            if (!line.Contains(comment))
                            {
                                line += comment;
                            }
                        }
                        break;
                    }
                }
                foreach (MyDic dic in runDicLst)
                {
                    bool canContinue = true;
                    //line = line.Replace(dic.Mine, dic.Other);
                    if (dic.NotContainsStrs != null)
                    {
                        for (int i = 0; i < dic.NotContainsStrs.Length; i++)
                        {
                            if (line.Contains(dic.NotContainsStrs[i]))
                            {
                                canContinue = false;
                                break;
                            }
                        }
                    }
                    if (canContinue)
                    {
                        if (dic.IsRegex)
                        {
                            Regex regex = new(dic.Mine);
                            line = regex.Replace(line, dic.Other);
                        }
                        else
                        {
                            line = line.Replace(dic.Mine, dic.Other);
                        }
                    }
                }
                foreach (string item in singletonLst)
                {
                    if (!file.Name.Contains(item) && line.Contains(item) && !line.Contains($"{item}:GetInstance()"))
                    {
                        line = line.Replace(item, $"{item}:GetInstance()");
                        line = line.Replace($"local {item}:GetInstance()", $"local {item}");
                        break;
                    }
                }
                sb.AppendLine(line);
            }
            string ret = sb.ToString();
            ret = ret.Substring(0, ret.LastIndexOf("\r\n"));
            IOUtil.CreateTextFile(file.FullName, ret);
        }

        private static List<MyDic> CreateRunDicLst(FileInfo file, bool is2Other)
        {
            List<MyDic> runDicLst;
            if (is2Other)
            {
                runDicLst = DeepCopyUtil.Clone(dic2OtherLst);
            }
            else
            {
                runDicLst = DeepCopyUtil.Clone(dic2MineLst);
            }
            //StringUtil.DeepReplace(runDicLst, "$(FileName)", file.Name.Substring(0, file.Name.IndexOf('.')));
            string fileName = file.Name.Substring(0, file.Name.IndexOf('.'));
            foreach (MyDic item in runDicLst)
            {
                item.Mine = item.Mine.Replace("$(FileName)", fileName);
                item.Other = item.Other.Replace("$(FileName)", fileName);
                if (item.NotContainsStrs != null)
                {
                    for (int i = 0; i < item.NotContainsStrs.Length; i++)
                    {
                        string s = item.NotContainsStrs[i];
                        item.NotContainsStrs[i] = s.Replace("$(FileName)", fileName);
                    }
                }
            }

            return runDicLst;
        }

        private static void ProcessFileToMine(FileInfo file)
        {
            List<MyDic> runDicLst = CreateRunDicLst(file, false);

            StringBuilder sb = new();
            string[] lines = IOUtil.GetFileTextArr(file);
            for (int iLine = 0; iLine < lines.Length; iLine++)
            {
                string line = lines[iLine];
                foreach (string item in localDefineLst)
                {
                    if (line.Contains(item))
                    {
                        string tmp = line;
                        while (tmp[0] == ' ')
                        {
                            tmp = tmp[1..];
                        }
                        if (tmp[0] == '-' && tmp[1] == '-' && tmp[2] == ' ')
                        {
                            line = line.Remove(line.IndexOf("-- "), 3);
                        }
                        break;
                    }
                }
                foreach (MyDic dic in runDicLst)
                {
                    bool canContinue = true;
                    //line = line.Replace(dic.Mine, dic.Other);
                    if (dic.NotContainsStrs != null)
                    {
                        for (int i = 0; i < dic.NotContainsStrs.Length; i++)
                        {
                            if (line.Contains(dic.NotContainsStrs[i]))
                            {
                                canContinue = false;
                                break;
                            }
                        }
                    }
                    if (canContinue)
                    {
                        if (dic.IsRegex)
                        {
                            Regex regex = new(dic.Other);
                            line = regex.Replace(line, dic.Mine);
                        }
                        else
                        {
                            line = line.Replace(dic.Other, dic.Mine);
                        }
                    }
                }
                foreach (string item in singletonLst)
                {
                    if (!file.Name.Contains(item) && line.Contains(item) && line.Contains($"{item}:GetInstance()"))
                    {
                        line = line.Replace($"{item}:GetInstance()", item);
                        break;
                    }
                }
                sb.AppendLine(line);
            }
            string ret = sb.ToString();
            ret = ret.Substring(0, ret.LastIndexOf("\r\n"));
            IOUtil.CreateTextFile(file.FullName, ret);
        }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(MyDic))]
    class MyDic
    {
        public string Other;
        public string Mine;
        public bool IsRegex;
        public string[] NotContainsStrs;
    }
}
