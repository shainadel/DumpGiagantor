using System;
using Loaders.Apps.Android;

namespace DumpGiagantor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Dump Giagantor";
            Console.WriteLine("Get Ready to Increase you PA Application Dump");

            AndroidWhatsApp parser = new AndroidWhatsApp();
            //AndroidICQ parser = new AndroidICQ();
            //AndroidTwitter parser = new AndroidTwitter();
            //AndroidTango parser = new AndroidTango();
            //AndroidWeChat parser = new AndroidWeChat();
            //AndroidFacebookMessanger parser = new AndroidFacebookMessanger();
            parser.Init();
            parser.Parse();
        }
    }
}