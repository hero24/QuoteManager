using System;
using System.Drawing;
using System.Windows.Forms;
// "Ask any racer. Any real racer. It don't matter if you win by an inch or a mile. Winning's winning." ~ Dom Toretto. @ Fast and Furious
namespace QuoteManager
{
    class StaticGUI
    {
        public static int Width = 800;
        public static int TAB = 10;
        public static void ErrorMsg(string error, int code)
        {
            MessageBox.Show(error,"Error");
        }
    }
    class QuoteMenu : MainMenu
    {
        private static string PROGRAM_TITLE = "Quotes";
        private static string CURRENT_TITLE = "Current";
        Loader loader;
        public QuoteMenu(Loader loader)
        {
            this.loader = loader;
            programMenu();
            currentMenu();            
        }
        private void currentMenu()
        {
            MenuItem current = new MenuItem(QuoteMenu.CURRENT_TITLE);
            MenuItem references = new MenuItem("Add reference");
            MenuItem flags = new MenuItem("Add flag");
            current.MenuItems.Add(references);
            current.MenuItems.Add(flags);
            MenuItems.Add(current);
        }
        private void programMenu()
        {
            MenuItem quotes = new MenuItem(QuoteMenu.PROGRAM_TITLE);
            MenuItem add_ = new MenuItem("Add");
            MenuItem save = new MenuItem("Save");
            MenuItem exit = new MenuItem("Exit");
            exit.Click += OnExit;
            save.Click += OnLoad;
            add_.Click += OnAdd;
            quotes.MenuItems.Add(add_);
            quotes.MenuItems.Add(save);
            quotes.MenuItems.Add(exit);
            MenuItems.Add(quotes);
        }
        private void OnExit(object sender, EventArgs ea)
        {
            Application.Exit();
        }
        private void OnLoad(object sender, EventArgs ea)
        {
            loader.saveBinaryData();
        }
        private void OnAdd(object sender, EventArgs ea)
        {
            AddQuote addWindow = new AddQuote();

        }
    }
    public class AddQuote:Form
    {
        public AddQuote()
        {
            TextBox quote = new TextBox();
            TextBox author = new TextBox();
            Button AddButton = new Button();
            AddButton.Text = "Add";
            Text = "Add Quote";
            Width = StaticGUI.Width;
            Controls.Add(quote);
            Controls.Add(author);
            Controls.Add(AddButton);
            Closing += OnClose;
            Show();
        }
        private void OnClose(object sender, EventArgs ae)
        {
            StaticGUI.ErrorMsg("Are you sure, you will loose unsaved changes",4);
        }
    }
    public class GUI:Form
    {        
        private Loader loader;
        private int i = 0;
        private Label currentQuote = new Label();
        private Label currentAuthor = new Label();
        private Button prev = new Button();
        private Button next = new Button();
        private Quote[] quotes;
        
        public GUI(string title, Loader load, Quote[] storage)
        {
            Width = StaticGUI.Width;
            Text = title;
            loader = load;
            quotes = storage;
            Menu = new QuoteMenu(load);
            Application.ApplicationExit += OnExit;
            currentAuthor.Text = storage[i].author;
            currentQuote.Text = storage[i].getQuote();
            next.Left = ClientSize.Width - next.Width - StaticGUI.TAB;
            currentQuote.MaximumSize = new Size(ClientSize.Width - prev.Width - next.Width - (StaticGUI.TAB * 3),0);
            currentAuthor.AutoSize = true;
            currentQuote.AutoSize = true;
            Controls.Add(currentAuthor);
            Controls.Add(currentQuote);
            prev.Click += OnPrevClick;
            next.Click += OnNextClick;
            prev.Text = "Previous";
            prev.Left = StaticGUI.TAB;
            next.Text = "Next";
            Controls.Add(prev);
            Controls.Add(next);
            resizeLabels();
        }
        private void resizeLabels()
        {
            currentAuthor.Left = (StaticGUI.TAB * 2) + prev.Width;
            currentQuote.Left = (StaticGUI.TAB * 2) + prev.Width;
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
                currentAuthor.Text = quotes[++i].author;
                currentQuote.Text = quotes[i].getQuote();
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
                currentAuthor.Text = quotes[--i].author;
                currentQuote.Text = quotes[i].getQuote();
                resizeLabels();
            } else
            {
                StaticGUI.ErrorMsg("No more quotes to display",3);
            }
        }
    }
}

