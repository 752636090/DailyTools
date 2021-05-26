using System;
using System.IO;

namespace PrintAllFilesName
{
    class Program
    {
        static void Main(string[] args)
        {
            string folderPath = @"F:\书";
            DirectoryInfo directory = new DirectoryInfo(folderPath);
            FileInfo[] files = directory.GetFiles();
            foreach (FileInfo file in files)
            {
                Console.WriteLine(file.Name);
            }
            Console.WriteLine("完成");
        }
    }
}
