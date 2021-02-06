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
        [JsonProperty("CurriculumStructure")]
        public string CurriculumStructureJson { get; set; }
        [JsonProperty("generic")]
        public string Generic { get; set; }
        [JsonProperty("modUserName")]
        public string ModUserName { get; set; }
        [JsonProperty("urlMap")]
        public string UrlMap { get; set; }

        public override void DeserialiseInnerJson()
        {
            if (this.InnerJsonData != null) {
                this.CourseData = JsonConvert.DeserializeObject<MacquarieCourseData>(this.InnerJsonData);
            } else {
                System.Diagnostics.Debug.WriteLine("Unable to deserialise inner Course json data.");
            }

            if (this.CurriculumStructureJson != null)             {
                this.CurriculumData = JsonConvert.DeserializeObject<MacquarieCurriculumStructureData>(this.CurriculumStructureJson);
            } else {
                System.Diagnostics.Debug.WriteLine("Unable to deserialise Curriculum Structure json data.");
            }
        }
    }
}