using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGameUtil
{
    class ReplaceUtil
    {
        public static void ReplaceText(string folderPath, string srcText, string targetText, string extName)
        {
            string[] filePathes = Directory.GetFiles(folderPath, $"*{extName}", SearchOption.AllDirectories);
            for (int i = 0; i < filePathes.Length; i++)
            {
                string content = IOUtil.GetFileText(filePathes[i]);
                if (content.Contains(srcText))
                {
                    File.SetAttributes(filePathes[i], FileAttributes.Normal);
                    IOUtil.CreateTextFile(filePathes[i], content.Replace(srcText, targetText));
                }
                Console.WriteLine($"完成{i + 1}/{filePathes.Length}");
            }
        }
    }
}
