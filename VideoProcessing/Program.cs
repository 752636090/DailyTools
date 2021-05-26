using System;
using MediaInfoDotNet;
using MediaInfoDotNet.Models;

namespace VideoProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            MediaFile media = new MediaFile(@"C:\Users\zjk\Desktop\毕设会议1.mp4");
            AudioStream videoStream = media.Audio[0];
        }
    }
}
