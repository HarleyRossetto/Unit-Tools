using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Shared
{
    public class MacquarieDataCollection<T> where T : MacquarieMetadata
    {
        [JsonProperty("contentlets")]
        public List<T> Collection { get; set; }
        
        public T this[int index] { get => Collection[index]; set => Collection[index] = value; }

        public int Count { get => Collection.Count; }

        public IEnumerator<T> GetEnumerator() => Collection.GetEnumerator();
    }
}