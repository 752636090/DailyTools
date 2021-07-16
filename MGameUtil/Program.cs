using System;
using System.Threading;

namespace MGameUtil
{
    class Program
    {
        static void Main(string[] args)
        {
            string key = "";

            foreach (string item in args)
            {
                Thread.Sleep(20);
                Handle(item);
            }

            while (true)
            {
                Thread.Sleep(20);
                key = Console.ReadLine();
                Handle(key);
            }
        }

        private static void Handle(string key)
        {
            if (key == "1")
            {
                UEC2SConverter.Convert();
            }
            else if (key == "2")
            {
                AssignListConverter.Convert();
            }
            else if (key == "3")
            {
                HandleSet.Handle();
            }
            else if (key == "4")
            {
                GenZGlobal.Gen();
            }
            else if (key == "5")
            {
                CodeStyleConverter.ConvertToOther();
            }
            else if (key == "6")
            {
                CodeStyleConverter.ConvertToMine();
            }
        }
    }
}
