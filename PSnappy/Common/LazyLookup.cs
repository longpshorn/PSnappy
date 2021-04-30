using System;
using System.Collections.Generic;
using System.Linq;

namespace PSnappy
{
    public class LazyLookup<T1, T2>
    {
        private bool _iscached = false;
        private readonly IEnumerable<T1> _items;
        private readonly Func<T1, T2> _keyselector;
        private Dictionary<T2, T1> _dic = new Dictionary<T2, T1>();

        public LazyLookup(IEnumerable<T1> items, Func<T1, T2> keyselector)
        {
            Clear();
            _items = items;
            _keyselector = keyselector;
        }

        public void Clear()
        {
            _dic.Clear();
            _iscached = false;
        }

        public T1 GetValueOrDefault(T2 key)
        {
            if (!_iscached)
            {
                var hasduplicates = _items.HasDuplicates(_keyselector);
                if (!hasduplicates) _dic = _items.ToDictionary(_keyselector);
                _iscached = true;
            }

            if (_dic.Any())
            {
                if (_dic.TryGetValue(key, out T1 value))
                {
                    return value;
                }
                return (T1)default;
            }
            else
            {
                return _items.FirstOrDefault(e => key.Equals(_keyselector(e)));
            }
        }
    }
}
