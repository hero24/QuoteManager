using System.Windows.Forms;
namespace QuoteManager
{
    class Program
    {
        public static void Main(string[] args)
        {
            Loader load = new Loader();
            load.load("quotes.txt.txt");
            Application.Run(new GUI("QuoteManager"));
        }
    }
}