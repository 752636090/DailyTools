using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using MediaInfoDotNet;
using MediaInfoDotNet.Models;
using MediaInfoLib;

namespace VideoDuration
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("输入视频根路径按回车");
                string folderPath = Console.ReadLine();
                //string folderPath = @"F:\视频\unity游戏开发\数值策划";
                //DirectoryInfo directory = new DirectoryInfo(folderPath);
                //FileInfo[] files = directory.GetFiles("*", SearchOption.AllDirectories);
                List<FileInfo> files = new List<FileInfo>();
                GetFiles(files, folderPath);
                int TotalTimeBySeconds = 0;
                int len = files.Count;
                for (int i = 0; i < len; i++)
                {
                    MediaFile media = new MediaFile(files[i].FullName);
                    try
                    {
                        int TimeBySeconds = media.Video[0].Duration / 1000;
                        TotalTimeBySeconds += TimeBySeconds;
                        Console.WriteLine(i + 1 + "/" + len);
                        //int seconds = TimeBySeconds % 60;
                        //int hours = TimeBySeconds / 3600;
                        //int mins = TimeBySeconds % 3600 / 60;
                        //Console.WriteLine(files[i].Name + " = " + i + 1 + "\t" + hours + ":" + mins + ":" + seconds);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("×××××××××××××××××××××××××××××");
                        Console.WriteLine(e);
                        Console.WriteLine(files[i].FullName);
                        Console.WriteLine("×××××××××××××××××××××××××××××");
                    }
                }
                int TotalSeconds = TotalTimeBySeconds % 60;
                int TotalHours = TotalTimeBySeconds / 3600;
                int TotalMins = TotalTimeBySeconds % 3600 / 60;
                Console.WriteLine("完成，共 " + TotalHours + ":" + TotalMins + ":" + TotalSeconds);

                using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"\History.txt", true))
                {
                    sw.WriteLine(folderPath + " - " + TotalHours + ":" + TotalMins + ":" + TotalSeconds);
                } 
            }
        }

        static void GetFiles(List<FileInfo> files, string path)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo directory = new DirectoryInfo(path);
                foreach (FileInfo file in directory.GetFiles())
                {
                    GetFiles(files, file.FullName);
                }
                foreach (DirectoryInfo dir in directory.GetDirectories())
                {
                    if (!(
                        dir.Name.Contains("直播")
                        || dir.Name.Contains("答疑")
                        || dir.Name.Contains("选修")
                        || dir.Name.Contains("虚幻4")
                        || dir.Name.Contains("U3D二次元")
                        || dir.Name.Contains("资料")
                        ))
                    {
                        Console.WriteLine("进入文件夹" + dir.Name);
                        GetFiles(files, dir.FullName);
                    }
                }
            }
            else
            {
                FileInfo file = new FileInfo(path);
                //if (!(
                //    file.Name.Contains("建模案例分析.hxsd-.mkv")
                //    ))
                {
                    files.Add(file); 
                }
            }
        }
    }
}

//using System;
//using System.IO;
//using System.Xml.XPath;
//using NReco.VideoInfo;

//namespace VideoDuration
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            string folderPath = @"F:\视频\unity游戏开发\图形学games101";
//            DirectoryInfo directory = new DirectoryInfo(folderPath);
//            FileInfo[] files = directory.GetFiles("*", searchOption:SearchOption.AllDirectories);
//            foreach (FileInfo file in files)
//            {
//                FFProbe ffProbe = new FFProbe();
//                MediaInfo videoInfo = ffProbe.GetMediaInfo(file.FullName);
//                Console.WriteLine(videoInfo.Duration);
//            }
//            Console.WriteLine("完成");

//        }
//    }
//}
