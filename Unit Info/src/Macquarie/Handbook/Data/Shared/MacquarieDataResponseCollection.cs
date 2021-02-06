using System.Collections.Generic;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Shared
{
    public class MacquarieDataResponseCollection<T>
    {
        [JsonProperty("contentlets")]
        public List<T> Units { get; set; }

        public T this[int index] {
            get {
                return Units[index] != null ? Units[index] : default(T);
            }
        }
    }
}