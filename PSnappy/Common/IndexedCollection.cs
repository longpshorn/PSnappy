using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PSnappy
{
    public static class IndexedCollectionExtensions
    {
        public static IndexedCollection<TKey, TValue> ToIndexedCollection<TKey, TValue>(this IEnumerable<TValue> items, Func<TValue, TKey> keyselector)
        {
            return new IndexedCollection<TKey, TValue>(items, keyselector);
        }
    }

    public class IndexedCollection<TKey, TValue> : ICollection<TValue>
    {
        private readonly Func<TValue, TKey> _keyselector;
        private readonly Dictionary<TKey, ICollection<TValue>> _indexedlists = new Dictionary<TKey, ICollection<TValue>>();
        private readonly ICollection<TValue> _nullkeyeditems;

        private static ICollection<TValue> DefaultCollectionFactory() => new List<TValue>();

        public IndexedCollection(Func<TValue, TKey> keyselector, Func<ICollection<TValue>> collectionFactory = null) : base()
        {
            _keyselector = keyselector;
            collectionFactory ??= DefaultCollectionFactory;
            _nullkeyeditems = collectionFactory();
        }

        public IndexedCollection(IEnumerable<TValue> items, Func<TValue, TKey> keyselector)
        {
            _nullkeyeditems = DefaultCollectionFactory();
            _keyselector = keyselector;
            foreach (var e in items)
            {
                Add(e);
            }
        }

        public IEnumerable<TKey> GetKeys()
        {
            return _indexedlists.Keys;
        }

        public bool HasNullKeyedValues
        {
            get { return _nullkeyeditems.Any(); }
        }

        private IEnumerator<TValue> GetMyEnumerator()
        {
            var items = _indexedlists.Values.SelectMany(e => e);

            if (_nullkeyeditems.Any())
            {
                items = items.Concat(_nullkeyeditems);
            }

            return items.GetEnumerator();
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return GetMyEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetMyEnumerator();
        }

        public IEnumerable<TValue> GetSubset(TKey key)
        {
            if (key != null)
            {
                return _indexedlists.TryGetValue(key, out var result) ? result : Enumerable.Empty<TValue>();
            }
            return _nullkeyeditems;
        }

        public virtual void Add(TValue item)
        {
            var key = _keyselector(item);
            if (key != null)
            {
                if (!_indexedlists.ContainsKey(key))
                {
                    _indexedlists.Add(key, new List<TValue>());
                }
                _indexedlists[key].Add(item);
            }
            else
            {
                _nullkeyeditems.Add(item);
            }
        }

        public virtual void AddRange(IEnumerable<TValue> items)
        {
            var groups = items.GroupBy(_keyselector);
            foreach (var g in groups)
            {
                AddRange(g.Key, g);
            }
        }

        private void AddRange(TKey key, IEnumerable<TValue> items)
        {
            if (key != null)
            {
                if (!_indexedlists.ContainsKey(key))
                {
                    _indexedlists.Add(key, new List<TValue>());
                }
                _indexedlists[key].AddRange(items);
            }
            else
            {
                _nullkeyeditems.AddRange(items);
            }
        }

        public void Clear()
        {
            _indexedlists.Clear();
            _nullkeyeditems.Clear();
        }

        public bool Contains(TValue item)
        {
            var key = _keyselector(item);
            if (key != null)
            {
                return _indexedlists.ContainsKey(key) && _indexedlists[key].Contains(item);
            }
            else
            {
                return _nullkeyeditems.Contains(item);
            }
        }

        public bool ContainsKey(TKey key)
        {
            return _indexedlists.ContainsKey(key);
        }

        public void CopyTo(TValue[] array, int arrayIndex)
        {
            var index = arrayIndex;
            foreach (var e in this)
            {
                array[index] = e;
                index++;
            }
        }

        public int Count
        {
            get { return _indexedlists.Sum(e => e.Value.Count) + _nullkeyeditems.Count; }
        }

        public virtual bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(TValue item)
        {
            var result = false;

            var key = _keyselector(item);

            if (key != null)
            {
                if (_indexedlists.ContainsKey(key))
                {
                    result = _indexedlists[key].Remove(item);
                }
            }
            else
            {
                result = _nullkeyeditems.Remove(item);
            }

            return result;
        }

        public void TrimExcess()
        {
            foreach (var e in _indexedlists)
            {
                if (e.Value is List<TValue> list)
                {
                    list.TrimExcess();
                }
            }
        }
    }
}
