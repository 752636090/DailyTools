using System;
using System.IO;

namespace Rename
{
    class Program
    {
        static void Main(string[] args)
        {
            string folderPath = @"F:\视频\unity游戏开发\傅老师OpenGL\第3章";
            string beReplace = "(傅老師-OpenGL教學 第三章) (01-08晚間更新) OpenGL自製3D遊戲引擎(已更畢) (";
            string toReplace = "Chapter 3-";
            DirectoryInfo directory = new DirectoryInfo(folderPath);
            FileInfo[] files = directory.GetFiles();
            int fileCount = files.Length;
            foreach (FileInfo file in files)
            {
                //Console.Write(file.FullName + "\n" + " ==> " + "\n");
                File.Move(file.FullName, file.FullName.Replace(beReplace, toReplace));
                Console.WriteLine(file.FullName + "\n");
            }
            Console.WriteLine("完成");
        }


    }
}
