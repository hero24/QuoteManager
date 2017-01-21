using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
// "As for Guns N' Roses, I don't think there's ever a chance of a reunion." ~ Slash
namespace QuoteManager
{
    public delegate void ErrorReport(string error,int code);
    public class Loader
    {
        Storage<Quote> quotes;
        static string savedData = "quotes.bin";
        ErrorReport error;
        
        public Loader(Storage<Quote> storage, ErrorReport errorOut)
        {
            error = errorOut;
            quotes = storage;
            FileInfo binaryData = new FileInfo(Loader.savedData);
            if (binaryData.Exists)
            {
                loadBinaryData();
            }
        }
        ~Loader()
        {
            saveBinaryData();
        }
        
        public void loadFromFile(string filename)
        {
            try
                {
                    FileInfo quotesText = new FileInfo(filename);
                    StreamReader quoteStream = quotesText.OpenText();
                    String line;
                    while ((line = quoteStream.ReadLine()) != null)
                    {
                        String[] line_ = line.Split('~');
                        quotes.Add(new Quote(line_[1], line_[0]));
                    }
                }
                catch (Exception e)
                {
                    error("Loading error, problem with reading from file",1);
                }            
        }
        
        public void loadBinaryData()
        {
            try
                {
                    Stream quotesBinary = File.Open(Loader.savedData, FileMode.Open);
                    BinaryFormatter deserialize = new BinaryFormatter();
                    quotes.Add((Storage<Quote>)deserialize.Deserialize(quotesBinary));
                    quotesBinary.Close();
                }
                catch(Exception e)
                {
                    error("Loading error, problem with reading saved quotes",2);
                }
        }
        
        public void saveBinaryData()
        {
            Stream stream = File.Open(Loader.savedData, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, quotes);
            stream.Close();
        }
        
        public Storage<Quote> getStorage()
        {
            return quotes;
        }
    }
}
