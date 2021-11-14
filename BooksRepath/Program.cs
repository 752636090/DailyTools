using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BooksRepath
{
    class Program
    {
        static Dictionary<string, int> infos = new();

        static void Main(string[] args)
        {
            CollectInfo();
            Dictionary<string, bool> flags = new();

            DirectoryInfo directory = new(AppDomain.CurrentDomain.BaseDirectory);
            FileInfo[] files = directory.GetFiles("*", SearchOption.AllDirectories);
            Console.ForegroundColor = ConsoleColor.Cyan;
            foreach (FileInfo file in files)
            {
                if (infos[file.Name] > 0)
                {
                    string targetPath = AppDomain.CurrentDomain.BaseDirectory + file.Name;
                    string targetDirectoryPath = targetPath.Substring(0, targetPath.LastIndexOf("\\"));
                    if (!Directory.Exists(targetDirectoryPath))
                    {
                        Directory.CreateDirectory(targetDirectoryPath);
                    }
                    File.Move(file.FullName, targetPath);
                    flags[file.Name] = true;
                    break;
                }
                else
                {
                    Console.WriteLine($"本地多余：{file.FullName}");
                }
            }

            Console.ForegroundColor = ConsoleColor.Red;
            foreach (string info in infos.Keys)
            {
                if (flags[info] == false)
                {
                    Console.WriteLine("不存在：" + info);
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkRed;
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
                if (!infos.ContainsKey(line))
                {
                    infos[line] = 0;
                }
                infos[line] = infos[line] + 1;
            }
            sr.Close();
        }
    }
}
