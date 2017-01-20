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
        private static int TAB = 10;
        
        private Loader loader;
        private Label currentQuote = new Label();
        private Label currentAuthor = new Label();
        
        public GUI(string title, Loader load)
        {
            Text = title;
            loader = load;
            Menu = new QuoteMenu("Quotes",load);
            Application.ApplicationExit += OnExit;
            currentAuthor.Text = loader.quotes[0].author;
            currentQuote.Text = loader.quotes[0].getQuote();
            currentAuthor.Top = currentQuote.Height;
            currentAuthor.Left = GUI.TAB * 2;
            currentAuthor.AutoSize = true;
            currentQuote.AutoSize = true;
            currentQuote.Left = GUI.TAB;
            Controls.Add(currentAuthor);
            Controls.Add(currentQuote);
        }
        private void OnExit(Object sender, EventArgs ea)
        {
            loader.saveBinaryData();
        }
    }
}
