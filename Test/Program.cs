using System;
using System.Diagnostics;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Process.Start(@"E:\DailyTools\MGameUtil\bin\Debug\net5.0\MGameUtil.exe", new string[] { @"5|C:\Users\Administrator\Desktop\Test\DlgAdventureName.lua" });
        }
    }
}
