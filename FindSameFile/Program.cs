using System;
using System.Collections.Generic;
using System.IO;

namespace FindSameFile
{
    class Program
    {
        static void Main(string[] args)
        {
            string folderPath = @"F:\视频\美术\sakimichan";
            string extensionName = ".mp4";
            DirectoryInfo directory = new DirectoryInfo(folderPath);
            FileInfo[] files = directory.GetFiles("*" + extensionName, SearchOption.AllDirectories);
            HashSet<string> nameSet = new HashSet<string>();
            HashSet<string> outSet = new HashSet<string>();
            foreach (FileInfo file in files)
            {
                if (nameSet.Contains(file.Name))
                {
                    if (!outSet.Contains(file.Name))
                    {
                        outSet.Add(file.Name);
                    }
                }
                else
                {
                    nameSet.Add(file.Name);
                }
            }
            foreach (string name in outSet)
            {
                Console.WriteLine(name);
            }
            Console.WriteLine("完成");
        }
    }
}
