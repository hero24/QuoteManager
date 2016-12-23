using System;
namespace QuoteManager
{
    class Program
    {
        static void Main(string[] args)
        { 
            Quote[] quotes = new Quote[100];
            int i = 0;
            quotes[i++] = new Quote("Athas","Linux printing was designed and implemented by people working to preserve the rainforest by making it utterly impossible to consume paper.");
            quotes[i++] = new Quote("Unknown","ALSA is like the emperors new clothes.It never works, but people say it’s because you’re a noob.");
            quotes[i++] = new Quote("--kfx","ruby is what happens when some kid learns java then looks at perl and thinks ‘I can fix this!’.");
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
