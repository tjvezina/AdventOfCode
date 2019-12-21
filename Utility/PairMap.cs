using System;
using System.Collections;
using System.Collections.Generic;

namespace AdventOfCode {
    public class PairMap<T> : IEnumerable<KeyValuePair<T, T>> {
        private Dictionary<T, T> _forward = new Dictionary<T, T>();
        private Dictionary<T, T> _reverse = new Dictionary<T, T>();

        public T this[T key] {
            get {
                if (_forward.ContainsKey(key)) return _forward[key];
                if (_reverse.ContainsKey(key)) return _reverse[key];
                throw new Exception($"PairMap does not contain key {key}");
            }
            set {
                if (key.Equals(value)) throw new Exception($"Cannot set a key equal to itself: {key}");
                if (_reverse.ContainsKey(key)) {
                    _forward.Remove(_reverse[key]);
                    _reverse[key] = value;
                    _forward[value] = key;
                    return;
                }
                if (_forward.ContainsKey(key)) {
                    _reverse.Remove(_forward[key]);
                }
                _forward[key] = value;
                _reverse[value] = key;
            }
        }

        public void Add(KeyValuePair<T, T> pair) => Add(pair.Key, pair.Value);
        public void Add(T a, T b) {
            _forward[a] = b;
            _reverse[b] = a;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<KeyValuePair<T, T>> GetEnumerator() => _forward.GetEnumerator();
    }

    public class PairMap<T1, T2> : IEnumerable<KeyValuePair<T1, T2>> {
        private Dictionary<T1, T2> _forward = new Dictionary<T1, T2>();
        private Dictionary<T2, T1> _reverse = new Dictionary<T2, T1>();

        public T2 this[T1 key] {
            get => _forward[key];
            set => _forward[key] = value;
        }
        public T1 this[T2 key] {
            get => _reverse[key];
            set => _reverse[key] = value;
        }

        public void Add(KeyValuePair<T1, T2> pair) => Add(pair.Key, pair.Value);
        public void Add(T1 a, T2 b) {
            _forward[a] = b;
            _reverse[b] = a;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator() => _forward.GetEnumerator();
    }
}
