using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BooksRepath
{
    class Program
    {
        static Dictionary<string, string> infos = new();

        static void Main(string[] args)
        {
            CollectInfo();
            Dictionary<string, bool> flags = new();

            DirectoryInfo directory = new(AppDomain.CurrentDomain.BaseDirectory);
            FileInfo[] files = directory.GetFiles("*", SearchOption.AllDirectories);
            Console.ForegroundColor = ConsoleColor.Cyan;
            foreach (FileInfo file in files)
            {
                if (infos.ContainsKey(file.Name))
                {
                    string targetPath = AppDomain.CurrentDomain.BaseDirectory + infos[file.Name];
                    string targetDirectoryPath = targetPath[..targetPath.LastIndexOf("\\")];
                    if (!Directory.Exists(targetDirectoryPath))
                    {
                        Directory.CreateDirectory(targetDirectoryPath);
                    }
                    File.Move(file.FullName, targetPath);
                    flags[file.Name] = true;
                }
                else if (file.Extension == ".pdf")
                {
                    Console.WriteLine($"本地多余：{file.Name}");
                }
            }

            Console.ForegroundColor = ConsoleColor.Red;
            foreach (string info in infos.Keys)
            {
                if (!flags.ContainsKey(info) || flags[info] == false)
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
                string name = line[(line.LastIndexOf(@"\") + 1)..];
                infos[name] = line;
            }
            sr.Close();
        }
    }
}
