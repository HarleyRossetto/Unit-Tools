using System;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Shared
{
    public class MacquarieMetadata
    {
        [JsonProperty("hostName")]
        public string HostName { get; set; }
        [JsonProperty("modDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ModificationDate { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }

        protected string _InnerJsonData;

        [JsonProperty("studyLevel")]
        public string StudyLevel { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("contentTypeLabel")]
        public string ContentTypeLabel { get; set; }
        [JsonProperty("inode")]
        public string IndexNode { get; set; }
        [JsonProperty("archived")]
        public bool Archived { get; set; }
        [JsonProperty("host")]
        public string Host { get; set; }
        [JsonProperty("working")]
        public bool Working { get; set; }
        [JsonProperty("locked")]
        public bool Locked { get; set; }
        [JsonProperty("stInode")]
        public string STIndexNode { get; set; }
        [JsonProperty("contentType")]
        public string ContentType { get; set; }
        [JsonProperty("live")]
        public bool Live { get; set; }
        [JsonProperty("owner")]
        public string Owner { get; set; }
        [JsonProperty("identifier")]
        public string Identifier { get; set; }
        [JsonProperty("studyLevelValue")]
        public string StudyLevelValue { get; set; }
        [JsonProperty("languageId")]
        public UInt16 LanguageId { get; set; }
        [JsonProperty("URL_MAP_FOR_CONTENT")]
        public string UrlMapForContent { get; set; }
        [JsonProperty("parentAcademicOrg")]
        public string ParentAcademicOrg { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("titleImage")]
        public string TitleImage { get; set; }
        [JsonProperty("implementationYear")]
        public string ImplementationYear { get; set; }
        [JsonProperty("folder")]
        public string Folder { get; set; }
        [JsonProperty("hasTitleImage")]
        public bool HasTitleImage { get; set; }
        [JsonProperty("sortOrder")]
        public UInt16 SortOrder { get; set; }
        [JsonProperty("modUser")]
        public string ModificationUser { get; set; }

        protected T DeserialiseInnerJson<T>(ref string json)
        {
            return MacquarieHandbook.DeserialiseJsonObject<T>(json).Result;
        }
    }
}