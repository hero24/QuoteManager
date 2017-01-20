using System;
using System.Windows.Forms;
// "Ask any racer. Any real racer. It don't matter if you win by an inch or a mile. Winning's winning." ~ Dom Toretto. @ Fast and Furious
namespace QuoteManager
{
    class StaticGUI
    {
        public static void ErrorMsg(string error, int code)
        {
            MessageBox.Show(error,"Error");
        }
    }
    class QuoteMenu : MainMenu
    {
        Loader loader;
        public QuoteMenu(string title, Loader loader)
        {
            this.loader = loader;
            MenuItem quotes = new MenuItem(title);
            MenuItems.Add(quotes);
            MenuItem save = new MenuItem("Save");
            quotes.MenuItems.Add(save);
            MenuItem exit = new MenuItem("Exit");
            exit.Click += new EventHandler(OnExit);
            save.Click += OnLoad;
            quotes.MenuItems.Add(exit);
        }
        private void OnExit(object sender, EventArgs ea)
        {
            Application.Exit();
        }
        private void OnLoad(object sender, EventArgs ea)
        {
            loader.saveBinaryData();
        }
    }
    
    public class GUI:Form
    {
        Loader loader;
        public GUI(string title, Loader load)
        {
            Text = title;
            Menu = new QuoteMenu("Quotes",load);
            loader = load;
            Application.ApplicationExit += OnExit; 
        }
        private void OnExit(Object sender, EventArgs ea)
        {
            loader.saveBinaryData();
        }
    }
}
