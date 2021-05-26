using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace CodeStyleConverter
{
    class Program
    {
        private static List<FileInfo> fileLst;
        private static List<string> localDefineLst;
        private static List<MyDic> myDicLst;

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
            myDicLst = new List<MyDic>();

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
                string path = item.Attribute("CodeLine").Value;
                localDefineLst.Add(path);
            }

            IEnumerable<XElement> dicElems = doc.Root.Element("Dic").Elements("Item");
            foreach (XElement item in dicElems)
            {
                myDicLst.Add(new MyDic() { Other = item.Attribute("Other").Value,
                    Mine = item.Attribute("Mine").Value
                });
            }
        }

        private static void ProcessFileToOther(FileInfo file)
        {
            StreamReader sr = file.OpenText();
            StringBuilder sb = new();
            int iLine = 0;
            while (!sr.EndOfStream)
            {
                iLine++;
                string line = sr.ReadLine();
                foreach (string item in localDefineLst)
                {
                    if (line.Contains(item))
                    {
                        string tmp = line;
                        string spaces = "";
                        while(tmp[0] == ' ')
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
                    else
                    {
                        foreach (MyDic dic in myDicLst)
                        {
                            line = line.Replace(dic.Mine, dic.Other);
                        }
                    }
                }
                sb.AppendLine(line);
            }
            sr.Close();
            FileStream fs = file.OpenWrite();
            byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
            fs.SetLength(bytes.Length);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }

        private static void ProcessFileToMine(FileInfo file)
        {
            StreamReader sr = file.OpenText();
            StringBuilder sb = new();
            int iLine = 0;
            while (!sr.EndOfStream)
            {
                iLine++;
                string line = sr.ReadLine();
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
                    else
                    {
                        foreach (MyDic dic in myDicLst)
                        {
                            line = line.Replace(dic.Other, dic.Mine);
                        }
                    }
                }
                sb.AppendLine(line);
            }
            sr.Close();
            FileStream fs = file.OpenWrite();
            byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
            fs.SetLength(bytes.Length);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }
    }

    class MyDic
    {
        public string Other;
        public string Mine;
    }
}
