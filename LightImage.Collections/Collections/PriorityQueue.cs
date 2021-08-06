using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace LightImage.Collections
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private readonly List<T> _data = new List<T>();

        public PriorityQueue(params T[] items)
        {
            foreach (T item in items)
                Enqueue(item);
        }

        public int Count => _data.Count;

        public T Dequeue()
        {
            var li = _data.Count - 1;
            T frontItem = _data[0];
            _data[0] = _data[li];
            _data.RemoveAt(li);

            --li;
            var pi = 0;
            while (true)
            {
                var ci = pi * 2 + 1;
                if (ci > li) break;
                var rc = ci + 1;
                if (rc <= li && _data[rc].CompareTo(_data[ci]) < 0)
                    ci = rc;
                if (_data[pi].CompareTo(_data[ci]) <= 0) break;
                T tmp = _data[pi];
                _data[pi] = _data[ci];
                _data[ci] = tmp;
                pi = ci;
            }
            return frontItem;
        }

        public void Enqueue(T item)
        {
            _data.Add(item);
            var ci = _data.Count - 1;
            while (ci > 0)
            {
                var pi = (ci - 1) / 2;
                if (_data[ci].CompareTo(_data[pi]) >= 0)
                    break;
                T tmp = _data[ci];
                _data[ci] = _data[pi];
                _data[pi] = tmp;
                ci = pi;
            }
        }

        public T Peek()
        {
            return _data[0];
        }

        public void RemoveAll(Func<T, bool> predicate)
        {
            var items = _data.ToArray();
            _data.Clear();
            foreach (var item in items.Where(x => !predicate(x)))
                Enqueue(item);
        }
    }
}