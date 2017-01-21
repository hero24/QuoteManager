using System;
namespace QuoteManager
{
    [Serializable()]
    public class Storage<T>
    {
        private static int INITIAL = 100;
        private static int RESIZE = 10;
        private T[] storage;
        private int i;
        public int Length
        {
            get
            {
                return i;
            }
        }
        public Storage()
        {
            storage = new T[Storage<T>.INITIAL];
        }
        public Storage(int initial)
        {
            storage = new T[initial];
        }
        public void Add(Storage<T> items)
        {
            for(int j = 0;j < items.Length;j++)
            {
                Add(items.Get(j));
            }
        }
        public void Add(T item)
        {
            if(i > storage.Length)
            {
                T[] tempStorage = new T[storage.Length + Storage<T>.RESIZE];
                for(int j=0; j < storage.Length;j++)
                {
                    tempStorage[j] = storage[j];
                }
                storage = tempStorage;
                
            }
            storage[i++] = item;
        }
        public T Get(int index)
        {
            if(index < i)
            {
                return storage[index];
            }
            throw new IndexOutOfRangeException();
        }
        
        
    }
}