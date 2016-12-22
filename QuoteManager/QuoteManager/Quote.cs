namespace QuoteManager
{
    public class Quote
    {
        private string author;
        private string quote;
        private string[] references;
        public Quote(string author, string quote)
        {
            this.author = author;
            this.quote = quote;
        }
    }
}