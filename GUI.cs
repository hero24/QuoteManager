using System;
using System.Drawing;
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
        private int i = 0;
        private Label currentQuote = new Label();
        private Label currentAuthor = new Label();
        private Button prev = new Button();
        private Button next = new Button();
        
        public GUI(string title, Loader load)
        {
            Text = title;
            loader = load;
            Width = 800;
            Menu = new QuoteMenu("Quotes",load);
            Application.ApplicationExit += OnExit;
            currentAuthor.Text = loader.quotes[i].author;
            currentQuote.Text = loader.quotes[i++].getQuote();
            next.Left = ClientSize.Width - next.Width - GUI.TAB;
            currentAuthor.AutoSize = true;
            currentQuote.MaximumSize = new Size(ClientSize.Width - prev.Width - next.Width - (GUI.TAB * 3),0);
            currentQuote.AutoSize = true;
            Controls.Add(currentAuthor);
            Controls.Add(currentQuote);
            prev.Click += OnPrevClick;
            next.Click += OnNextClick;
            prev.Text = "Previous";
            prev.Left = GUI.TAB;
            next.Text = "Next";
            Controls.Add(prev);
            Controls.Add(next);
            resizeLabels();
        }
        private void resizeLabels()
        {
            currentAuthor.Left = (GUI.TAB * 2) + prev.Width;
            currentQuote.Left = (GUI.TAB * 2) + prev.Width;
            currentAuthor.Top = currentQuote.Height;            
        }
        private void OnExit(Object sender, EventArgs ea)
        {
            loader.saveBinaryData();
        }
        private void OnNextClick(object sender, EventArgs ae)
        {
            if(i < loader.count-1)
            {
                currentAuthor.Text = loader.quotes[++i].author;
                currentQuote.Text = loader.quotes[i].getQuote();
                resizeLabels();
            } else
            {
                StaticGUI.ErrorMsg("No more quotes to display",3);
            }
        }
        private void OnPrevClick(object sender, EventArgs ae)
        {
            if(i > 0 && i < loader.count)
            {
                currentAuthor.Text = loader.quotes[--i].author;
                currentQuote.Text = loader.quotes[i].getQuote();
                resizeLabels();
            } else
            {
                StaticGUI.ErrorMsg("No more quotes to display",3);
            }
        }
    }
}
