using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nekoyume.Model.State;

namespace Nekoyume.Model
{
    public class CollectionMap : IState, IDictionary<int, int>
    {
        private Dictionary<int, int> _dictionary;

        public CollectionMap()
        {
            _dictionary = new Dictionary<int, int>();
        }

        public void Add(KeyValuePair<int, int> item)
        {
            if (_dictionary.ContainsKey(item.Key))
            {
                _dictionary[item.Key] += item.Value;
            }
            else
            {
                _dictionary[item.Key] = item.Value;
            }
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool Remove(KeyValuePair<int, int> item)
        {
            return _dictionary.Remove(item.Key);
        }

        public int Count => _dictionary.Count;
        public bool IsReadOnly => false;

        public ICollection<int> Keys => throw new NotImplementedException();

        public ICollection<int> Values => throw new NotImplementedException();

        public IEnumerator<KeyValuePair<int, int>> GetEnumerator()
        {
            return _dictionary.OrderBy(kv => kv.Key).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(int key, int value)
        {
            Add(new KeyValuePair<int, int>(key, value));
        }

        public bool TryGetValue(int key, out int value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public int this[int key]
        {
            get => _dictionary[key];
            set => _dictionary[key] = value;
        }

        public bool Contains(KeyValuePair<int, int> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<int, int>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(int key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int key)
        {
            throw new NotImplementedException();
        }
    }
}
