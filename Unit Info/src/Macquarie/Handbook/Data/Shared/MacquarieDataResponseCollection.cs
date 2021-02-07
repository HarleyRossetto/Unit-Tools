using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Shared
{
    public class MacquarieDataResponseCollection<T> where T : MacquarieMetadata
    {
        [JsonProperty("contentlets")]
        public List<T> Collection { get; set; } 

        public T this[int index] {
            get {
                return Collection[index] != null ? Collection[index] : default(T);
            }
        }
    }
}