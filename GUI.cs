using System;
using System.Windows.Forms;
// "Ask any racer. Any real racer. It don't matter if you win by an inch or a mile. Winning's winning." ~ Dom Toretto. @ Fast and Furious
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
