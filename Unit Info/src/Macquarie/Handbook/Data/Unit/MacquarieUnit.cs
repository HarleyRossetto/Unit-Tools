//#define IGNORE_UNNECESSARY

using System;
using Macquarie.Handbook.Data.Shared;
using Macquarie.Handbook.Data.Unit;
using Macquarie.Handbook.Converters;
using Newtonsoft.Json;
using Macquarie.Handbook.Helpers;

namespace Macquarie.Handbook.Data
{
    public class MacquarieUnit : MacquarieMetadata
    {
        [JsonProperty("data")]
        [JsonConverter(typeof(MacquarieEmbeddedJsonConverter<MacquarieUnitData>))]
        public MacquarieUnitData UnitData { get; set; }
        [JsonProperty("creditPoints")]
        public UInt16 CreditPoints { get; set; }

        [JsonProperty("description")]
        [JsonConverter(typeof(MacquarieHtmlStripperConverter))]
        public string Description { get; set; }
        [JsonProperty("level")]
        public string Level { get; set; }
        [JsonProperty("publishedInHandbook")]
        public UInt16 PublishedInHandbook { get; set; }

        [JsonProperty("levelDisplay")]
        public string LevelDisplay { get; set; }
        [JsonProperty("effectiveDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? EffectiveDate { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("sysId")]
        public string SysId { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("location")]
        public string Location { get; set; }

        
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("version")]
        public string Version { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("teachingPeriod")]
        public string TeachingPeriod { get; set; }
        
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("active")]
        public string Active { get; set; }

        #if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("type")]
        public string Type { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("baseType")]
        public string BaseType { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("mode")]
        public string Mode { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("academicOrg")]
        public string AcademicOrg { get; set; }
    }
}