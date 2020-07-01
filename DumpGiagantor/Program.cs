using System;
using Loaders.Apps.Android;

namespace DumpGiagantor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            AndroidWhatsApp parser = new AndroidWhatsApp();
            parser.Init();
            parser.Parse();
        }
    }
}
