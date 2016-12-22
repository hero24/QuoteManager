using System;
namespace QuoteManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("hello");
            Quote q = new Quote("Cezary Pazura", "Kobieta u mojego boku musi byc swiatowym topem");
            //Console.Write(q.quote);
            Console.Read();
        }
    }
}
