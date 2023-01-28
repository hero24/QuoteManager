using System;
using System.Drawing;
using System.Windows.Forms;
// "Ask any racer. Any real racer. It don't matter if you win by an inch or a mile. Winning's winning." ~ Dom Toretto. @ Fast and Furious
namespace QuoteManager
{
    public delegate void ComboAction();
    public class GUIParent : Form
    {
        public GUIParent()
        {
            this.MaximizeBox = false;
            //this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }
    }
    public class Prompt : GUIParent
    {
        PlaceholderedBox input;
        Button accept;
        Button cancel;
        private Storage<DataSegment> ds;
        Action action;
       
        public Prompt(string text, Storage<DataSegment> ds, Action action)
        {
            input = new PlaceholderedBox(text);
            accept = new Button();
            cancel = new Button();
            accept.Text = "OK";
            cancel.Text = "Cancel";
            input.Top = StaticGUI.TAB;
            cancel.Top = input.Top + input.Height + StaticGUI.TAB;
            accept.Top = input.Top + input.Height + StaticGUI.TAB;
            cancel.Left = accept.Width + accept.Left + StaticGUI.TAB;
            Controls.Add(input);
            Controls.Add(accept);
            Controls.Add(cancel);
            accept.Click += OnClick;
            cancel.Click += OnCancel;
            Width = cancel.Left + cancel.Width + StaticGUI.TAB;
            input.Width = Width - (StaticGUI.TAB * 2);
            input.Left = StaticGUI.TAB;
            Height = input.Height + (StaticGUI.TAB * 6) + accept.Height;
            this.MinimizeBox = false;  
            this.ds = ds;
            this.action = action;     
            Show();
        }
        protected internal void OnCancel(object sender, EventArgs ea)
        {
            this.Close();
        }

        protected internal void OnClick(object sender, EventArgs ea)
        {
            ds.Add(new DataSegment(input.Text));
            this.action();
            this.Close();
        }
    }
    class StaticGUI
    {
        public const int Width = 800;
        public const int TAB = 10;
        public const string ActionDelete = "Delete";
        public const string FLAG = "Flags : ";
        public const string REFS = "References : ";
        public static void ErrorMsg(string error, int code)
        {
            MessageBox.Show(error,"Error",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
        }
        public static void Msg(string msg, string title)
        {
            MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static bool ErrorMsgYesNo(string error,int code)
        {
            DialogResult result = MessageBox.Show(error,"Error",MessageBoxButtons.OKCancel,MessageBoxIcon.Exclamation);
            if (result == DialogResult.OK) return true;
            else return false;
        }
    }
    class AboutMenu : MenuItem
    {
        protected const string TITLE = "About";
        private const string ABOUT = "QuoteManager v1 by hero24";
        public AboutMenu() : base(AboutMenu.TITLE)
        {
            var about = new MenuItem(TITLE);
            this.MenuItems.Add(about);
            about.Click += OnAbout;
        }
        private void OnAbout(object sender, EventArgs ea)
        {
            StaticGUI.Msg(ABOUT, TITLE);
        }
    }
    class CurrentMenu : MenuItem
    {
        protected const string TITLE = "Current";
        private const string REFERENCE = "Add reference";
        private const string FLAG = "Add flag";
        private const string EDIT = "Edit";
        private const string COPY = "Copy";
        private const string REFRESH = "Refresh";
        GUI parent;
        public CurrentMenu(GUI parent) : base(CurrentMenu.TITLE)
        {
            var references = new MenuItem(REFERENCE);
            var flags = new MenuItem(FLAG);
            var edit = new MenuItem(EDIT);
            var delete = new MenuItem(StaticGUI.ActionDelete);
            var copy = new MenuItem(COPY);
            var refresh = new MenuItem(REFRESH);
            this.MenuItems.Add(references);
            this.MenuItems.Add(flags);
            this.MenuItems.Add(copy);
            this.MenuItems.Add(edit);
            this.MenuItems.Add(delete);
            this.MenuItems.Add(refresh);
            references.Click += OnReferences;
            flags.Click += OnFlags;
            delete.Click += OnDelete;
            copy.Click += OnCopy;
            edit.Click += OnEdit;
            refresh.Click += OnRefresh;
            this.parent = parent;
        }
                private void OnRefresh(object sender, EventArgs ea)
        {
            parent.refresh();
        }
        private void OnReferences(object sender, EventArgs ea)
        {
            Prompt p = new Prompt("References", parent.getCurrentQuote().getReferences(), parent.changeCombos);
        }
        private void OnFlags(object sender, EventArgs ea)
        {
            Prompt p = new Prompt("Flags", parent.getCurrentQuote().getFlags(), parent.changeCombos);
        }
        private void OnEdit(object sender, EventArgs ea)
        {
            QuoteGUI quote = new QuoteGUI(parent.getCurrentQuote(), parent);
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
                parent.refresh();
            }
        }
    }
    class QuotesMenu:MenuItem
    {
        protected const string TITLE = "Quotes";
        private const string ADD = "Add";
        private const string SAVE = "Save";
        private const string EXIT = "Exit";
        Loader loader;
        GUI parent;
        public QuotesMenu(Loader loader, GUI parent) : base(QuotesMenu.TITLE)
        {
            var add_ = new MenuItem(ADD);
            var save = new MenuItem(SAVE);
            var exit = new MenuItem(EXIT);
            exit.Click += OnExit;
            save.Click += OnLoad;
            add_.Click += OnAdd;
            this.MenuItems.Add(add_);
            this.MenuItems.Add(save);
            this.MenuItems.Add(exit);
            this.loader = loader;
            this.parent = parent;
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
            QuoteGUI addWindow = new QuoteGUI(loader.getStorage(),parent);
        }
    }
    class ProgramMenu : MainMenu
    {
        Loader loader;
        GUI parent;
        public ProgramMenu(Loader loader, GUI parent)
        {
            this.loader = loader;
            this.parent = parent;
            var quotes = new QuotesMenu(loader, parent);
            var current = new CurrentMenu(parent);
            var about = new AboutMenu();
            MenuItems.Add(quotes);
            MenuItems.Add(current);
            MenuItems.Add(about);
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
    public class QuoteGUI:GUIParent
    {
        const string quotePlaceholder = "Quote";
        const string authorPlaceholder = "Author";
        const string referencePlaceholder = "References, separate by commas";
        const string flagsPlaceholder = "Flags, separate by commas";
        
        Storage<Quote> storage;
        Quote q;
        PlaceholderedBox quote;
        PlaceholderedBox author;
        PlaceholderedBox references;
        PlaceholderedBox flags;
        GUI main;
        public QuoteGUI(Quote quote, GUI main)
        {
            q = quote;
            setup(main);
            this.quote.Text = q.getQuote();
            author.Text = q.author;
            Button EditButton = new Button();
            EditButton.Top = flags.Top + flags.Height +StaticGUI.TAB;
            EditButton.Text = "Edit";
            EditButton.Left = StaticGUI.TAB;
            Text = "Edit Quote";
            Width = StaticGUI.Width;
            Controls.Add(EditButton);
            loadFlagsRefs();
            EditButton.Click += OnEdit;
            Show();
        }
        public QuoteGUI(Storage<Quote> quotes, GUI main)
        {
            setup(main);
            storage = quotes;
            Button AddButton = new Button();
            AddButton.Top = flags.Top + flags.Height + StaticGUI.TAB;
            AddButton.Text = "Add";
            AddButton.Left = StaticGUI.TAB;
            Text = "Add Quote";
            Width = StaticGUI.Width;
            Controls.Add(AddButton);
            AddButton.Click += OnAdd;
            Show();
        }
        private void loadFlagsRefs()
        {
            
            if (q.getReferences().Length > 0)
            {
                string refs = "";
                for(int i=0;i<q.getReferences().Length;i++)
                {
                    refs += q.getReferences().Get(i).ToString() + ','; 
                }
                references.Text = refs;   
            } 
            if(q.getFlags().Length > 0)
            {
                string flag = "";
                for(int i=0;i<q.getFlags().Length;i++)
                {
                    flag += q.getFlags().Get(i).ToString() + ','; 
                }
                this.flags.Text = flag;   
            }
        }
        private void setup(GUI main)
        {
            int labelWidth = StaticGUI.Width - (StaticGUI.TAB * 4);
            quote = new PlaceholderedBox(QuoteGUI.quotePlaceholder);
            author = new PlaceholderedBox(QuoteGUI.authorPlaceholder);
            references = new PlaceholderedBox(QuoteGUI.referencePlaceholder);
            flags = new PlaceholderedBox(QuoteGUI.flagsPlaceholder);
            author.Top = quote.Height;
            quote.Left = StaticGUI.TAB;
            author.Left = StaticGUI.TAB;
            quote.Width = labelWidth;
            author.Width = labelWidth;
            references.Width = labelWidth;
            flags.Width = labelWidth;
            references.Left = StaticGUI.TAB;
            flags.Left = StaticGUI.TAB;
            references.Top = author.Top + author.Height + StaticGUI.TAB;
            flags.Top = references.Top + references.Height + StaticGUI.TAB;
            Controls.Add(quote);
            Controls.Add(author);
            Controls.Add(references);
            Controls.Add(flags);
            this.main = main;
            Closing += OnClose;
        }
        private void addData(PlaceholderedBox box, string placeholder, Quote quote_, ADD Add)
        {
            if(box.Text != placeholder)
            {
                string[] refs = box.Text.Split(',');
                foreach(string ref_ in refs)
                {
                    Add(ref_.Trim(), quote_);
                }
            }
        }
        private string[] Extract(string Text)
        {
            string[] refs = Text.Split(',');
            for(int i = 0;i < refs.Length ;i++)
            {
                refs[i] = refs[i].Trim();
            }   
            return refs;
        }
        private delegate void ADD(string s, Quote q);
        private void addRef(string s, Quote q)
        {
            q.addReference(s);
        }
        private void addFlag(string s, Quote q)
        {
            q.addFlag(s);
        }
        private void OnEdit(object sender, EventArgs ae)
        {
            string[] refs;
            string[] flags;
            if(references.Text != referencePlaceholder) 
                refs = Extract(references.Text);
            else
                refs = new string[0];
            if(this.flags.Text != flagsPlaceholder)
                flags = Extract(this.flags.Text);
            else
                flags = new string[0];
            q.Update(author.Text,quote.Text,refs,flags);        
            main.refresh();
            Closing -= OnClose;
            Close();
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
            if(quote.Text == QuoteGUI.quotePlaceholder || author.Text == QuoteGUI.authorPlaceholder)
            {
                StaticGUI.ErrorMsg("Quote and Author are required",5);
                return;
            }
            Quote newQuote = new Quote(author.Text,quote.Text);
            addData(references,QuoteGUI.referencePlaceholder, newQuote, addRef);
            addData(flags,QuoteGUI.flagsPlaceholder, newQuote, addFlag);
            storage.Add(newQuote);
            Closing -= OnClose;
            main.refresh();
            Close();
        }
    }
    public class DataList:ComboBox
    {
        Button actionButton;
        GUI parent;
        Storage<DataSegment> storage;
        ComboAction delDS;
        Label desc;
        public DataList(Storage<DataSegment> data,string buttonText, string Desc, GUI parent,int top, int left, ComboAction delDS)
        {
            this.parent = parent;
            DropDownStyle = ComboBoxStyle.DropDownList;
            desc = new Label();
            desc.AutoSize = true;
            desc.Text = Desc;
            desc.Top = top;
            desc.Left = left;
            Top = desc.Top;
            Left = StaticGUI.TAB + desc.Left + desc.Width;
            storage = data;
            actionButton = new Button();
            actionButton.Text = buttonText;
            actionButton.Left = Left + StaticGUI.TAB + Width;
            actionButton.Top = Top;
            actionButton.Click += OnClick;
            for(int i = 0; i < storage.Length; i++)
            {
                Items.Add(storage.Get(i));
            }
            AutoChoice();
            this.delDS = delDS;
            parent.Controls.Add(desc);
            parent.Controls.Add(actionButton);
        }
        protected internal void OnClick(object sender, EventArgs ae)
        {
            delDS();
        }
        public void AutoChoice()
        {
            if (storage.Length > 0) 
            {
                SelectedIndex = 0;
            }
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
            //ComboBoxes();
            refresh();
        }
        private void ComboBoxes()
        {
            refs = new DataList(quotes.Get(i).getReferences(),StaticGUI.ActionDelete,StaticGUI.REFS,this,150,StaticGUI.TAB,delRefs);
            flags = new DataList(quotes.Get(i).getFlags(),StaticGUI.ActionDelete,StaticGUI.FLAG,this,200,StaticGUI.TAB,delFlags);
            Controls.Add(refs);
            Controls.Add(flags);
        }
        public void changeCombos()
        {
            Controls.Remove(refs);
            Controls.Remove(flags);
            ComboBoxes();
        }
        private void delRefs()
        {
            if(refs.SelectedItem != null)
            {
                quotes.Get(i).getReferences().Remove(((DataSegment) refs.SelectedItem));
                refs.Items.Remove(refs.SelectedItem);
                refs.AutoChoice();
                refs.Refresh();
            }
        }
        private void delFlags()
        {
            if(refs.SelectedItem != null)
            {
                quotes.Get(i).getFlags().Remove(((DataSegment) flags.SelectedItem));
                flags.Items.Remove(flags.SelectedItem);
                refs.AutoChoice();
                flags.Refresh();
            }
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
        public void clear()
        {
            currentQuote.Text = "";
            currentAuthor.Text = "";
            if(refs != null && flags != null)
            {
                refs.Items.Clear();
                flags.Items.Clear();
            }
        }
        public void refresh()
        {
            if(quotes.Length > 0 && i < quotes.Length)
            {
                currentQuote.Text = quotes.Get(i).getQuote();
                currentAuthor.Text = quotes.Get(i).author;
                changeCombos();
                resizeLabels();
            }
            else if(i > 0)
            {
                i--;
                refresh();
            }else
            {
                clear();
                //i++;
            }
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
                changeCombos();
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
                changeCombos();
                resizeLabels();
            } else
            {
                StaticGUI.ErrorMsg("No more quotes to display",3);
            }
        }
    }
}
