using System.Windows.Forms;
using System;
// "I live my life a quarter mile at a time."  Dom Toretto @ Fast and Furious
namespace QuoteManager
{
    class Program
    {
        public static void ErrorReporter(string error,int code)
        {
            Console.WriteLine("Error code {0}: {1}",code,error);
        }
        public static void Main(string[] args)
        {
            Quote[] quotes = new Quote[100];
            Loader load = new Loader(quotes,StaticGUI.ErrorMsg,"quotes.txt.txt");
            quotes = load.storage;
            Application.Run(new GUI("QuoteManager",load,quotes));
        }
    }
}