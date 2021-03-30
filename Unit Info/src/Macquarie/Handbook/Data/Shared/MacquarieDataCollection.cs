using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Shared
{
    public class MacquarieDataCollection<T> where T : MacquarieMetadata
    {
        public MacquarieDataCollection(int capacity) {
            Collection = new List<T>(capacity);
        }

        public MacquarieDataCollection(IEnumerable<T> tempCollection) {
            Collection = new List<T>(tempCollection);
        }

        [JsonProperty("contentlets")]
        public List<T> Collection { get; set; }
        
        public T this[int index] { get => Collection[index]; set => Collection[index] = value; }

        public int Count { get => Collection.Count; }

        public IEnumerator<T> GetEnumerator() => Collection.GetEnumerator();

        public IEnumerable<T> AsEnumerable() {
            return Collection.AsEnumerable();
        }
    }
}