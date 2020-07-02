using System;
using Loaders.Apps.Android;

namespace DumpGiagantor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //AndroidWhatsApp parser = new AndroidWhatsApp();
            //AndroidICQ parser = new AndroidICQ();
            AndroidTwitter parser = new AndroidTwitter();
            parser.Init();
            parser.Parse();
        }
    }
}