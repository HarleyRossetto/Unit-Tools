using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Shared
{
    public record LabelledValue
    {
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        
        public override string ToString()
        {
            return Label;
        }
    }
}