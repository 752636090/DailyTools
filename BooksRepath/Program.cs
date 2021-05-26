using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BooksRepath
{
    class Program
    {
        static List<string> infos = new List<string>();

        static void Main(string[] args)
        {
            CollectInfo();
            int infoLen = infos.Count;
            bool[] flags = new bool[infoLen];

            DirectoryInfo directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            FileInfo[] files = directory.GetFiles("*", SearchOption.AllDirectories);
            foreach (FileInfo file in files)
            {
                for (int i = 0; i < infoLen; i++)
                {
                    string info = infos[i];
                    if (info.Contains(file.Name))
                    {
                        string targetPath = AppDomain.CurrentDomain.BaseDirectory + info;
                        string targetDirectoryPath = targetPath.Substring(0, targetPath.LastIndexOf("\\"));
                        if (!Directory.Exists(targetDirectoryPath))
                        {
                            Directory.CreateDirectory(targetDirectoryPath);
                        }
                        File.Move(file.FullName, targetPath);
                        flags[i] = true;
                        break;
                    }
                }
            }

            for (int i = 0; i < infoLen; i++)
            {
                if (flags[i] == false)
                {
                    Console.WriteLine("不存在：" + infos[i]);
                }
            }

            Console.WriteLine("完成");
            Console.ReadKey();
        }

        private static void CollectInfo()
        {
            FileInfo file = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\Info.txt");
            StreamReader sr = file.OpenText();
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                infos.Add(line);
            }
            sr.Close();
        }
    }
}
