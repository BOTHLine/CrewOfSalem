using System.Collections.Generic;

namespace CrewOfSalem
{
    public class DictionaryKVP<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public void Add(KeyValuePair<TKey, TValue> keyValuePair)
        {
            Add(keyValuePair.Key, keyValuePair.Value);
        }
    }
}