using System.Windows.Forms;
using System;
// "I live my life a quarter mile at a time."  Dom Toretto @ Fast and Furious
namespace QuoteManager
{
    class Program
    {
        public static void ErrorReporter(string error,int code)
        {
            // Legacy code
            Console.WriteLine("Error code {0}: {1}",code,error);
        }
        [STAThreadAttribute()]
        public static void Main(string[] args)
        {
            Storage<Quote> quotes = new Storage<Quote>();
            Loader load = new Loader(quotes,StaticGUI.ErrorMsg);
            Application.Run(new GUI("QuoteManager",load,quotes));
        }
    }
}