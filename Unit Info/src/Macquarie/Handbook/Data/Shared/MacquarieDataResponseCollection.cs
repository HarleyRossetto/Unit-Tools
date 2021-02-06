using System.Collections.Generic;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Shared
{
    public class MacquarieDataResponseCollection<T> where T : MacquarieMetadata
    {
        private List<T> _Units;
        [JsonProperty("contentlets")]
        public List<T> Units { 
            get {
                return _Units;
            }
            set {
                _Units = value;
                DeserialiseObjectsInnerJson();
            } 
        }

        protected void DeserialiseObjectsInnerJson() {
            foreach (var unit in Units)
            {
                unit.DeserialiseInnerJson();
            }
        }

        public T this[int index] {
            get {
                return Units[index] != null ? Units[index] : default(T);
            }
        }
    }
}