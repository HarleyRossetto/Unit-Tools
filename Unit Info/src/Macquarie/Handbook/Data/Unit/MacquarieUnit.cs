//#define IGNORE_UNNECESSARY

using System;
using Macquarie.Handbook.Data.Shared;
using Macquarie.Handbook.Data.Unit;
using Macquarie.Handbook.Data.Converters;
using Newtonsoft.Json;
using Macquarie.Handbook.Data.Helpers;

namespace Macquarie.Handbook.Data
{
    public class MacquarieUnit : MacquarieMetadata
    {
        private string description;

        [JsonProperty("data")]
        [JsonConverter(typeof(MacquarieEmbeddedJsonConverter<MacquarieUnitData>))]
        public MacquarieUnitData UnitData { get; set; }
        [JsonProperty("creditPoints")]
        public UInt16 CreditPoints { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("sysId")]
#endif
        public string SysId { get; set; }
        [JsonProperty("description")]
        public string Description { get => description; set => description = HTMLTagStripper.StripHtmlTags(value); }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("type")]
#endif
        public string Type { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("baseType")]
#endif
        public string BaseType { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("mode")]
#endif
        public string Mode { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("academicOrg")]
#endif
        public string AcademicOrg { get; set; }
        [JsonProperty("level")]
        public string Level { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("active")]
#endif
        public string Active { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("teachingPeriod")]
#endif
        public string TeachingPeriod { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("version")]
#endif
        public string Version { get; set; }
        [JsonProperty("publishedInHandbook")]
        public UInt16 PublishedInHandbook { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("location")]
#endif
        public string Location { get; set; }
        [JsonProperty("levelDisplay")]
        public string LevelDisplay { get; set; }
        [JsonProperty("effectiveDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? EffectiveDate { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}