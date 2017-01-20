using System;
// "A quote is just a tattoo on the tongue." ~ William F. DeVault
namespace QuoteManager
{
    [Serializable()]
    public class Quote
    {
        public string author
        {
            get { return _author; }
        }
        private string _author;
        private string quote;
        private int i;
        /*
        TODO:
        references -> what the quote is about
        flags -> any user set flags such as, already used.
        */
        private string[] references;
        private string[] flags;
        public Quote(string author, string quote)
        {
            this._author = author;
            this.quote = quote;
            references = new string[10];
            flags = new string[10];
            i = 0;
        }
        public string getQuote()
        {
            return quote;
        }
        public void addReference(string reference)
        {
            references[i++] = reference; 
        }
        public override string ToString()
        {
            return quote;
        }
    }
}