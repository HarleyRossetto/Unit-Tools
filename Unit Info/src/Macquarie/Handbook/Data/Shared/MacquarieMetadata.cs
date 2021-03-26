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
#else
        [JsonProperty("hostName")]
#endif
        public string HostName { get; set; }
        [JsonProperty("modDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ModificationDate { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("studyLevel")]
#endif
        public string StudyLevel { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("contentTypeLabel")]
#endif
        public string ContentTypeLabel { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("inode")]
#endif
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
#else
        [JsonProperty("working")]
#endif
        public bool Working { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("locked")]
#endif
        public bool Locked { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("stInode")]
#endif
        public string STIndexNode { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("contentType")]
#endif
        public string ContentType { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("live")]
#endif
        public bool Live { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("owner")]
#endif
        public string Owner { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("identifier")]
#endif
        public string Identifier { get; set; }
        #if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("studyLevelValue")]
#endif
        public string StudyLevelValue { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("languageId")]
#endif
        public UInt16 LanguageId { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("URL_MAP_FOR_CONTENT")]
#endif
        public string UrlMapForContent { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("parentAcademicOrg")]
#endif
        public string ParentAcademicOrg { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("url")]
#endif
        public string Url { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("titleImage")]
#endif
        public string TitleImage { get; set; }
        [JsonProperty("implementationYear")]
        public string ImplementationYear { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("folder")]
#endif
        public string Folder { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("hasTitleImage")]
#endif
        public bool HasTitleImage { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("sortOrder")]
#endif
        public UInt16 SortOrder { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("modUser")]
#endif
        public string ModificationUser { get; set; }
    }
}