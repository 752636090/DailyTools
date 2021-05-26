using System;
using System.IO;
using System.Text;

namespace PrintFilesNameInFolder
{
    class Program
    {
        static void Main(string[] args)
        {
            StringBuilder sbOut = new StringBuilder();
            //Console.WriteLine("输入文件夹路径：");
            //string folderPath = Console.ReadLine();
            string folderPath = @"F:\视频\美术\2020火星游戏特效\3.AE游戏场景课";
            char stopChar = '-';
            DirectoryInfo directory = new DirectoryInfo(folderPath);
            FileInfo[] files = directory.GetFiles();
            int fileCount = files.Length;
            FileInfoWithOrder[] myFiles = new FileInfoWithOrder[fileCount];
            for (int i = 0; i < fileCount; i++)
            {
                int numLen = 0;
                int stopIndex = -1;
                Scan(stopChar, files[i], ref stopIndex, ref numLen);
                myFiles[i] = new FileInfoWithOrder();
                myFiles[i].fileInfo = files[i];
                myFiles[i].order = int.Parse(files[i].Name.Substring(0, numLen));
                myFiles[i].realName = files[i].Name.Substring(numLen, stopIndex - numLen + 1);
            }
            Array.Sort(myFiles, (FileInfoWithOrder x, FileInfoWithOrder y) => { return x.order.CompareTo(y.order); });
            foreach (FileInfoWithOrder file in myFiles)
            {
                sbOut.Append(file.order);
                sbOut.Append(' ');
                sbOut.AppendLine(file.realName);
            }
            Console.WriteLine(sbOut.ToString());
        }

        private static void Scan(char stopChar, FileInfo file, ref int stopIndex, ref int numLen)
        {
            bool isNumEnd = false;
            foreach (char c in file.Name)
            {
                if (!isNumEnd)
                {
                    if (char.IsDigit(c))
                    {
                        numLen++;
                    }
                    else
                    {
                        isNumEnd = true;
                    }
                }
                if (c != stopChar)
                {
                    stopIndex++;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
