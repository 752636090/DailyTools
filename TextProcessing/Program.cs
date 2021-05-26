using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace TextProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            FileInfo file = new FileInfo(@"C:\Users\zjk\Desktop\temp.txt");
            StreamReader sr = file.OpenText();
            StringBuilder sb = new StringBuilder();
            int iLine = 0;
            string lastLine = string.Empty;
            while (!sr.EndOfStream)
            {
                iLine++;
                string line = sr.ReadLine();
                string curLine = line;
                //if (curLine.Contains("Section") || Regex.IsMatch(curLine, "[0-9][.][0-9]"))
                //{
                //    sb.AppendLine(curLine);
                //}
                //int index = curLine.IndexOf(@"/XYZ");
                //if (index > 0)
                //{
                //    curLine = curLine.Remove(index - 1);
                //}
                //if (line.Contains("Exercises"))
                //{
                //    sb.Append("\t");
                //}
                //if (Regex.IsMatch(curLine, "[0-9]"))
                //{
                //    sb.AppendLine(curLine);
                //}
                //int dotCount = Regex.Matches(line, "[.]").Count;
                //for (int i = 0; i < dotCount; i++)
                //{
                //    sb.Append("\t\t");
                //}
                //sb.AppendLine(curLine);
                //if (lastLine.Contains(line))
                //{
                //    sb.Replace(lastLine, line);
                //}
                //else if (line.Contains(lastLine))
                //{

                //}
                //else
                {
                    //sb.AppendLine(line);
                    sb.Append(line);
                }
                lastLine = line;
            }
            sr.Close();
            FileStream fs = file.OpenWrite();
            byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
            fs.SetLength(bytes.Length);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }
    }
}