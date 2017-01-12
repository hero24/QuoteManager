using System;
using System.IO;

namespace QuoteManager
{
    class Program
    {
        static void Main(string[] args)
        { 
            Quote[] quotes = new Quote[100];
            int i = 0;
            try
            {
                FileInfo quotesText = new FileInfo("quotes.txt");
                StreamReader quoteStream = quotesText.OpenText();
                String line;
                while((line = quoteStream.ReadLine()) != null)
                {
                    String[] line_ = line.Split('~');
                    quotes[i++] = new Quote(line_[1],line_[0]); 
                }
            } catch(Exception e)
            {
                Console.Write(e);
            }
            Console.Read();
            for (int j = 0; j < i; j++)
            {
                Console.WriteLine(quotes[j].getQuote());
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(quotes[j].author);
                Console.ResetColor();
                Console.Read();
            }
            
        }
    }
}
