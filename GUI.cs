using System;
using System.Windows.Forms;
namespace QuoteManager
{
    class QuoteMenu : MainMenu
    {
        public QuoteMenu(string title)
        {
            MenuItem quotes = new MenuItem(title);
            MenuItems.Add(quotes);
            MenuItem load = new MenuItem("Load");
            quotes.MenuItems.Add(load);
        }
    }
    
    public class GUI:Form
    {
        public GUI(string title)
        {
            Text = title;
            Menu = new QuoteMenu("Quotes");
        }
    }
}
