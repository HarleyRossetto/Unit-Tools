using System;
using Macquarie.Handbook.Data.Shared;
using Macquarie.Handbook.Data.Unit;
using Newtonsoft.Json;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Macquarie.Handbook.Data
{
    public class MacquarieUnit : MacquarieMetadata
    {
        //Must be populated after object deserialised
        public MacquarieUnitData UnitData { get; set; }

        [JsonProperty("data")]
        public string InnerJsonData {
            get {
                return _InnerJsonData;
            }
            set {
                //Let's not worry storing the json string again, we don't need it.
                //_InnerJsonData = value;

                //Deserialise the inner json.
                //UnitData = DeserialiseInnerJson<MacquarieUnitData>(ref _InnerJsonData);
                UnitData = DeserialiseInnerJson<MacquarieUnitData>(ref value);
            }
        }

        [JsonProperty("creditPoints")]
        public UInt16 CreditPoints { get; set; }
        [JsonProperty("sysId")]
        public string SysId { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("baseType")]
        public string BaseType { get; set; }
        [JsonProperty("mode")]
        public string Mode { get; set; }
        [JsonProperty("academicOrg")]
        public string AcademicOrg { get; set; }
        [JsonProperty("level")]
        public string Level { get; set; }
        [JsonProperty("active")]
        public string Active { get; set; }
        [JsonProperty("teachingPeriod")]
        public string TeachingPeriod { get; set; }
        [JsonProperty("version")]
        public string Version { get; set; }
        [JsonProperty("publishedInHandbook")]
        public UInt16 PublishedInHandbook { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
        [JsonProperty("levelDisplay")]
        public string LevelDisplay { get; set; }
        [JsonProperty("effectiveDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? EffectiveDate { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}