using System.Collections.Generic;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Shared
{
    public class MacquarieDataResponseCollection<T> where T : MacquarieMetadata
    {
        private List<T> _Objects;
        [JsonProperty("contentlets")]
        public List<T> Objects { 
            get {
                return _Objects;
            }
            set {
                _Objects = value;
                DeserialiseObjectsInnerJson();
            } 
        }

        protected void DeserialiseObjectsInnerJson() {
            foreach (var unit in Objects)
            {
                unit.DeserialiseInnerJson();
            }
        }

        public T this[int index] {
            get {
                return Objects[index] != null ? Objects[index] : default(T);
            }
        }
    }
}