using System;
using System.IO;
using System.Text;

namespace BooksOutpath
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo directory = new DirectoryInfo(rootPath);
            StringBuilder sb = new StringBuilder();
            foreach (FileInfo file in directory.GetFiles("*", SearchOption.AllDirectories))
            {
                if (file.Name.Contains("pdf") || file.Name.Contains("chm") || file.Name.Contains("doc"))
                {
                    sb.AppendLine(file.FullName.Replace(rootPath, ""));
                }
            }
            CreateTextFile(rootPath + @"\Info.txt", sb.ToString());
            Console.WriteLine("完成");
        }

        private static void CreateTextFile(string filePath, string content)
        {
            DeleteFile(filePath);

            using (FileStream fs = File.Create(filePath))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(content.ToString());
                }
            }
        }

        private static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
