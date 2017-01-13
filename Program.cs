using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace QuoteManager
{
    class Program
    {
        static void Main(string[] args)
        { 
            Quote[] quotes = new Quote[100];
            int i = 0;
            FileInfo binaryData = new FileInfo("quotes.bin");
            if (!binaryData.Exists)
            {
                try
                {
                    FileInfo quotesText = new FileInfo("quotes.txt");
                    StreamReader quoteStream = quotesText.OpenText();
                    String line;
                    while ((line = quoteStream.ReadLine()) != null)
                    {
                        String[] line_ = line.Split('~');
                        quotes[i++] = new Quote(line_[1], line_[0]);
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e);
                }
            } else
            {
                try
                {
                    Stream quotesBinary = File.Open("quotes.bin", FileMode.Open);
                    BinaryFormatter deserialize = new BinaryFormatter();
                    quotes = (Quote[])deserialize.Deserialize(quotesBinary);
                    quotesBinary.Close();
                    while (quotes[i] != null) i++;
                }
                catch(Exception e)
                {
                    Console.Write(e);
                }
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
            Stream stream = File.Open("quotes.bin", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, quotes);
            stream.Close();
        }
    }
}
