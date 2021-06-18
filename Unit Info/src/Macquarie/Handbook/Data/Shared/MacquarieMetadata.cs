//#define IGNORE_UNNECESSARY

using System;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Shared
{
    public class MacquarieMetadata
    {
        public DateTime DATE_DATA_RETRIEVED { get; init; } = DateTime.Now;
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("hostName")]
        public string HostName { get; set; }
        [JsonProperty("modDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ModificationDate { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("studyLevel")]
        public string StudyLevel { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("contentTypeLabel")]
        public string ContentTypeLabel { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("inode")]
        public string IndexNode { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("archived")]
#endif
        public bool Archived { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("host")]
#endif
        public string Host { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("working")]
        public bool Working { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("locked")]
        public bool Locked { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("stInode")]
#endif
        public string STIndexNode { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("contentType")]
        public string ContentType { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("live")]
        public bool Live { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("owner")]
        public string Owner { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("identifier")]
        public string Identifier { get; set; }
        #if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("studyLevelValue")]
        public string StudyLevelValue { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("languageId")]
        public UInt16 LanguageId { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("URL_MAP_FOR_CONTENT")]
        public string UrlMapForContent { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("parentAcademicOrg")]
        public string ParentAcademicOrg { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("url")]
        public string Url { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("titleImage")]
        public string TitleImage { get; set; }
        [JsonProperty("implementationYear")]
        public string ImplementationYear { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("folder")]
        public string Folder { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("hasTitleImage")]
        public bool HasTitleImage { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("sortOrder")]
        public UInt16 SortOrder { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("modUser")]
        public string ModificationUser { get; set; }
    }
}