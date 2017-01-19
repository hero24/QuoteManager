using System.Windows.Forms;
using System;
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
            Loader load = new Loader(ErrorReporter,"quotes.txt.txt");
            Application.Run(new GUI("QuoteManager"));
        }
    }
}