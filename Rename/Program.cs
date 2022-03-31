using System;
using System.IO;

namespace Rename
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = Console.ReadLine().Split('|');
            string folderPath = input[0]; //@"F:\视频\unity游戏开发\傅老师OpenGL\第3章";
            string beReplace = input[1]; // "(傅老師-OpenGL教學 第三章) (01-08晚間更新) OpenGL自製3D遊戲引擎(已更畢) (";
            string toReplace = input[2];// "Chapter 3-";
            bool isReplace = false;
            if (input.Length > 3)
            {
                isReplace = bool.Parse(input[3]);
            }
            DirectoryInfo directory = new DirectoryInfo(folderPath);
            FileInfo[] files = directory.GetFiles("*.*", SearchOption.AllDirectories);
            int fileCount = files.Length;
            foreach (FileInfo file in files)
            {
                //Console.Write(file.FullName + "\n" + " ==> " + "\n");
                if (isReplace)
                {
                    File.Move(file.FullName, file.FullName.Replace(beReplace, toReplace));
                    if (file.FullName.Contains(beReplace))
                    {
                        Console.WriteLine(file.FullName); 
                    }
                }
                if (file.FullName.Contains(beReplace))
                {
                    Console.WriteLine(file.FullName);
                }
            }
            Console.WriteLine("完成");
            Console.ReadLine();
        }


    }
}
