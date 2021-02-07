using System;
using Macquarie.Handbook.Data.Course;
using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data
{
    public class MacquarieCourse : MacquarieMetadata
    {
        //Must be populated after object deserialised
        public MacquarieCourseData CourseData { get; set; }
        public MacquarieCurriculumStructureData CurriculumData { get; set; }

        [JsonProperty("urlYear")]
        public string UrlYear { get; set; }

        private string _CurriculumStructureJson;
        [JsonProperty("CurriculumStructure")]
        public string CurriculumStructureJson { 
            get {
                return _CurriculumStructureJson;
            } 
            set {
                _CurriculumStructureJson = value;
                CurriculumData = DeserialiseInnerJson<MacquarieCurriculumStructureData>(ref _CurriculumStructureJson);
            } 
        }

        [JsonProperty("data")]
        public string InnerJsonData { 
            get {
                return _InnerJsonData;
            }
            set {
                _InnerJsonData = value;
                CourseData = DeserialiseInnerJson<MacquarieCourseData>(ref _InnerJsonData);
            } 
        }


        [JsonProperty("generic")]
        public string Generic { get; set; }
        [JsonProperty("modUserName")]
        public string ModUserName { get; set; }
        [JsonProperty("urlMap")]
        public string UrlMap { get; set; }
    }
}