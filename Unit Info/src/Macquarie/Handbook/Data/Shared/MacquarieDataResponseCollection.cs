using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Shared
{
    public class MacquarieDataResponseCollection<T> : IList<T> where T : MacquarieMetadata
    {
        [JsonProperty("contentlets")]
        public List<T> Collection { get; set; }

        public T this[int index] { get => ((IList<T>)Collection)[index]; set => ((IList<T>)Collection)[index] = value; }

        public int Count => ((ICollection<T>)Collection).Count;

        public bool IsReadOnly => ((ICollection<T>)Collection).IsReadOnly;

        public void Add(T item) {
            ((ICollection<T>)Collection).Add(item);
        }

        public void Clear() {
            ((ICollection<T>)Collection).Clear();
        }

        public bool Contains(T item) {
            return ((ICollection<T>)Collection).Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            ((ICollection<T>)Collection).CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator() {
            return ((IEnumerable<T>)Collection).GetEnumerator();
        }

        public int IndexOf(T item) {
            return ((IList<T>)Collection).IndexOf(item);
        }

        public void Insert(int index, T item) {
            ((IList<T>)Collection).Insert(index, item);
        }

        public bool Remove(T item) {
            return ((ICollection<T>)Collection).Remove(item);
        }

        public void RemoveAt(int index) {
            ((IList<T>)Collection).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)Collection).GetEnumerator();
        }
    }
}