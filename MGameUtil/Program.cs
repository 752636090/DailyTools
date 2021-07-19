using System;
using System.Threading;

namespace MGameUtil
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(args);
            if (args != null && args.Length > 0)
            {
                foreach (string item in args)
                {
                    Thread.Sleep(20);
                    string[] inputs = item.Split('|');
                    Handle(inputs[0], inputs[1]);
                }
                return;
            }

            while (true)
            {
                Thread.Sleep(20);
                string key = Console.ReadLine();
                Handle(key);
            }
        }

        private static void Handle(string key, string param = null)
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
                CodeStyleConverter.ConvertToOther(param != null, param);
            }
            else if (key == "6")
            {
                CodeStyleConverter.ConvertToMine(param != null, param);
            }
        }
    }
}
