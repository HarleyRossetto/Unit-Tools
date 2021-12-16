using Macquarie.Handbook.Data.Shared;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace MQHandbookAPI.Models.Macquarie.Handbook.Data.Unit;

public class UnitOfferingDTO
{
        public string Publish { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string TeachingPeriod { get; set; }
        public string AttendanceMode { get; set; }
        public string QuotaNumber { get; set; }
        public string StudyLevel { get; set; }
        public string AcademicItem { get; set; }
        public string ClarificationToAppearInHandbook { get; set; }
        public string SelfEnrol { get; set; }
        public string Order { get; set; }
        public string FeesCommonwealth { get; set; }
        public string FeesInternational { get; set; }
        public string CourseRestrictions { get; set; }
        public string QuotaLimit { get; set; }
        public string FeesDomestic { get; set; }
        public string Location { get; set; }

    public override string ToString() => DisplayName;
}
