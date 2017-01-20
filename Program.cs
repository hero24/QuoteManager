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
            Loader load = new Loader(StaticGUI.ErrorMsg,"quotes.txt.txt");
            Application.Run(new GUI("QuoteManager"));
        }
    }
}