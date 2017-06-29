using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Collections;

namespace Ubora.Web.Tests.Helper
{
    internal class TestTempDataDictionary : ITempDataDictionary
    {
        private Dictionary<string, object> _collection = new Dictionary<string, object>();
        public object this[string key]
        {
            get
            {
                return _collection[key];
            }
            set
            {
                _collection[key] = value;
            }
        }

        public ICollection<string> Keys => _collection.Keys;

        public ICollection<object> Values => _collection.Values;

        public int Count => _collection.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(string key, object value)
        {
            throw new NotImplementedException();
        }

        public void Add(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        public void Keep()
        {
            throw new NotImplementedException();
        }

        public void Keep(string key)
        {
            throw new NotImplementedException();
        }

        public void Load()
        {
            throw new NotImplementedException();
        }

        public object Peek(string key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out object value)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }
    }
}
