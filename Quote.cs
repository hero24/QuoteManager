using System;
// "A quote is just a tattoo on the tongue." ~ William F. DeVault
namespace QuoteManager
{
    [Serializable()]
    public class DataSegment
    {
        string name;
        public DataSegment(string reference)
        {
            name = reference;
        }
        public override string ToString()
        {
            return name;
        }
    }
    [Serializable()]
    public class Quote
    {
        public string author
        {
            get { return _author; }
        }
        private string _author;
        private string quote;
        /*
        TODO:
        maybe a serializable object for flag && reference
        create new or choose from list of existing?
        references -> what the quote is about
        flags -> any user set flags such as, already used.
        */
        private Storage<DataSegment> references;
        private Storage<DataSegment> flags;
        public Quote(string author, string quote)
        {
            this._author = author;
            this.quote = quote;
            references = new Storage<DataSegment>(10);
            flags = new Storage<DataSegment>(10);
        }
        private void updateStorage(string[] data,Storage<DataSegment> segment)
        {
            foreach(string s in data)
            {
                DataSegment ds = new DataSegment(s);
                if(!segment.Contains(ds)) 
                { 
                    segment.Add(ds);
                }   
            }
        }
        public void Update(string author, string quote, string[] refs,string[] flags)
        {
            _author = author;
            this.quote = quote;
            updateStorage(refs,references);
            updateStorage(flags,this.flags);
        }
        public string getQuote()
        {
            return quote;
        }
        public void addReference(string reference)
        {
            references.Add(new DataSegment(reference)); 
        }
        public void addFlag(string flag)
        {
            flags.Add(new DataSegment(flag));
        }
        public Storage<DataSegment> getReferences()
        {
            return references;
        }
        public Storage<DataSegment> getFlags()
        {
            return flags;
        }
        public override string ToString()
        {
            return quote + " ~ " + author;
        }
    }
}