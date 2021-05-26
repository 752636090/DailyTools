using System;
using System.IO;

namespace FileAddPrefix
{
    class Program
    {
        static void Main(string[] args)
        {
            string folderPath = @"F:\视频\美术\sakimichan\122";
            string prefix = "122-";
            DirectoryInfo directory = new DirectoryInfo(folderPath);
            FileInfo[] files = directory.GetFiles("*.mp4", SearchOption.AllDirectories);
            int fileCount = files.Length;
            foreach (FileInfo file in files)
            {
                //Console.Write(file.FullName + "\n" + " ==> " + "\n");
                File.Move(file.FullName, file.FullName.Replace(file.Name, prefix + file.Name));
                Console.WriteLine(file.Name);
            }
            Console.WriteLine("完成");
        }
    }
}
