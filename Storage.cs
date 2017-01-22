using System;
namespace QuoteManager
{
    //"Why is the rum always gone" ~ Jack Sparrow
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
        public void RemoveAt(int index)
        {
            for(int j = index + 1,k=index;j < i; j++,k++)
            {
                storage[k] = storage[j];
            }
            i--;
        }
        public void Remove(T data)
        {
            for(int j = 0; j < i;j++)
            {
                Console.WriteLine("{0} , {1} ",storage[j].Equals(data), data.GetType());
                if(storage[j].Equals(data))
                {
                    RemoveAt(j);
                    break;
                }
            }
        }
    }
}