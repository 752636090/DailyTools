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

    interface IEatable
    {
        void BeEat();
    }

    abstract class Fruit : IEatable
    {
        public virtual void BeEat()
        {
            Console.WriteLine("吃水果：");
        }
    }

    class Apple : Fruit
    {
        public override void BeEat()
        {
            base.BeEat();
            Console.WriteLine("吃苹果");
        }
    }

    class Banana : Fruit
    {
        public override void BeEat()
        {
            base.BeEat();
            Console.WriteLine("吃香蕉");
        }
    }
}
