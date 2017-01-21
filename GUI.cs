using System;
using System.Drawing;
using System.Windows.Forms;
// "Ask any racer. Any real racer. It don't matter if you win by an inch or a mile. Winning's winning." ~ Dom Toretto. @ Fast and Furious
namespace QuoteManager
{
    public class GUIParent : Form
    {
        public GUIParent()
        {
            this.MaximizeBox = false;
            //this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }
    }
    class StaticGUI
    {
        public static int Width = 800;
        public static int TAB = 10;
        public static string ActionDelete = "Delete";
        public static void ErrorMsg(string error, int code)
        {
            MessageBox.Show(error,"Error",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
        }
        public static bool ErrorMsgYesNo(string error,int code)
        {
            DialogResult result = MessageBox.Show(error,"Error",MessageBoxButtons.OKCancel,MessageBoxIcon.Exclamation);
            if (result == DialogResult.OK) return true;
            else return false;
        }
    }
    class ProgramMenu : MainMenu
    {
        /*
        TODO:
        private void aboutMenu();
        private void Quotes -> Refresh();
        */
        private static string PROGRAM_TITLE = "Quotes";
        private static string CURRENT_TITLE = "Current";
        Loader loader;
        GUI parent;
        public ProgramMenu(Loader loader, GUI parent)
        {
            this.loader = loader;
            this.parent = parent;
            programMenu();
            currentMenu();            
        }
        private void currentMenu()
        {
            MenuItem current = new MenuItem(ProgramMenu.CURRENT_TITLE);
            MenuItem references = new MenuItem("Add reference");
            MenuItem flags = new MenuItem("Add flag");
            MenuItem delete = new MenuItem(StaticGUI.ActionDelete);
            MenuItem copy = new MenuItem("Copy");
            current.MenuItems.Add(references);
            current.MenuItems.Add(flags);
            current.MenuItems.Add(copy);
            current.MenuItems.Add(delete);
            delete.Click += OnDelete;
            copy.Click += OnCopy;
            MenuItems.Add(current);
        }
        private void programMenu()
        {
            MenuItem quotes = new MenuItem(ProgramMenu.PROGRAM_TITLE);
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
        private void OnCopy(object sender, EventArgs ea)
        {
            Clipboard.SetText(parent.getCurrentQuote().ToString());
        }
        private void OnDelete(object sender, EventArgs ea)
        {
            if(StaticGUI.ErrorMsgYesNo("Are you sure you want to delete this quote",6))
            {
                parent.removeCurrent();
            }
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
            AddQuote addWindow = new AddQuote(loader.getStorage(),parent);
        }
    }
    public class PlaceholderedBox : TextBox
    {
        string placeholder;
        public PlaceholderedBox(string Text)
        {
            placeholder = Text;
            this.Text = Text;
            GotFocus += RemoveText;
            LostFocus += AddText;
        }
        private void RemoveText(object sender, EventArgs ae)
        {
            if (Text == placeholder) Text = "";
        }
        private void AddText(object sender, EventArgs ae)
        {
            if(String.IsNullOrWhiteSpace(Text))
            {
                Text = placeholder;
            }
        }
    }
    public class AddQuote:GUIParent
    {
        Storage<Quote> storage;
        TextBox quote;
        TextBox author;
        GUI main;
        public AddQuote(Storage<Quote> quotes, GUI main)
        {
            int labelWidth = StaticGUI.Width - (StaticGUI.TAB * 4);
            storage = quotes;
            quote = new PlaceholderedBox("Quote");
            author = new PlaceholderedBox("Author");
            author.Top = quote.Height;
            quote.Left = StaticGUI.TAB;
            author.Left = StaticGUI.TAB;
            quote.Width = labelWidth;
            author.Width = labelWidth;
            Button AddButton = new Button();
            AddButton.Top = quote.Height + author.Height;
            AddButton.Text = "Add";
            AddButton.Left = StaticGUI.TAB;
            Text = "Add Quote";
            Width = StaticGUI.Width;
            Controls.Add(quote);
            Controls.Add(author);
            Controls.Add(AddButton);
            AddButton.Click += OnAdd;
            Closing += OnClose;
            this.main = main;
            Show();
        }
        private void OnClose(object sender, EventArgs ae)
        {
            if(!StaticGUI.ErrorMsgYesNo("Are you sure, you will loose unsaved changes",4))
            {
                FormClosingEventArgs e = (FormClosingEventArgs) ae;
                e.Cancel = true;
            }
        }
        private void OnAdd(object sender, EventArgs ae)
        {
            if(quote.Text == "Quote" || author.Text == "Author")
            {
                StaticGUI.ErrorMsg("Quote and Author are required",5);
                return;
            }
            Quote newQuote = new Quote(author.Text,quote.Text);
            storage.Add(newQuote);
            Closing -= OnClose;
            main.refresh();
            Close();
        }
    }
    public class DataList:ComboBox
    {
        Button actionButton;
        public DataList(string buttonText)
        {
            actionButton = new Button();
            actionButton.Text = buttonText;
        }
    }
    public class GUI:GUIParent
    {        
        private Loader loader;
        private int i = 0;
        private Label currentQuote = new Label();
        private Label currentAuthor = new Label();
        private Button prev = new Button();
        private Button next = new Button();
        private Storage<Quote> quotes;
        private DataList refs;
        private DataList flags;
        public int currentQuoteId
        {
            get 
            {
                return i;
            }
        }
        public GUI(string title, Loader load, Storage<Quote> storage)
        {
            Width = StaticGUI.Width;
            Text = title;
            loader = load;
            quotes = storage;
            Menu = new ProgramMenu(load,this);
            Application.ApplicationExit += OnExit;
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
            ComboBoxes();
            refresh();
        }
        private void ComboBoxes()
        {
            refs = new DataList(StaticGUI.ActionDelete);
            flags = new DataList(StaticGUI.ActionDelete);
            refs.Top = 150;
            flags.Top = 200;
            Controls.Add(refs);
            Controls.Add(flags);
        }
        public Quote getCurrentQuote()
        {
            return quotes.Get(i);
        }
        public void removeCurrent()
        {
            quotes.RemoveAt(i);
            refresh();
        }
        public void refresh()
        {
            if(quotes.Length > 0)
            {
                currentQuote.Text = quotes.Get(i).getQuote();
                currentAuthor.Text = quotes.Get(i).author;
            }
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
            if(i < quotes.Length-1)
            {
                currentAuthor.Text = quotes.Get(++i).author;
                currentQuote.Text = quotes.Get(i).getQuote();
                resizeLabels();
            } else
            {
                StaticGUI.ErrorMsg("No more quotes to display",3);
            }
        }
        private void OnPrevClick(object sender, EventArgs ae)
        {
            if(i > 0 && i < quotes.Length)
            {
                currentAuthor.Text = quotes.Get(--i).author;
                currentQuote.Text = quotes.Get(i).getQuote();
                resizeLabels();
            } else
            {
                StaticGUI.ErrorMsg("No more quotes to display",3);
            }
        }
    }
}

