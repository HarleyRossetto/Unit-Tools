using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Macquarie.Handbook.Data.Shared;
using Macquarie.Handbook.Data.Unit;
using Macquarie.Handbook.Data.Unit.Prerequisites;
using MQHandbookAPI.Models.Macquarie.Handbook.Data.Shared;
using MQHandbookAPI.Models.Macquarie.Handbook.Data.Unit.Prerequisites;

namespace MQHandbookAPI.Models.Macquarie.Handbook.Data.Unit
{
    public class UnitDataDTO : MaterialMetadataDTO
    {
        public string GradingSchema { get; init; }
        public string StudyLevel { get; init; }
        public string QuoteEnrolmentRequirements { get; init; }
        public DateTime? StartDate { get; init; }
        public string Exclusions { get; init; }
        public string Level { get; init; }
        public string UACCode { get; init; }
        public string SpecialRequirements { get; init; }
        public List<string> SpecialUnitType { get; init; }
        public string VersionStatus { get; init; }
        public DateTime? EndDate { get; init; }
        public string LearningMaterials { get; init; }
        public bool SpecialTopic { get; init; }
        public string AscedBroad { get; init; }
        public string PlacementProportion { get; init; }
        public List<string> UnitDescription { get; init; }
        public List<LearningOutcomeDTO> UnitLearningOutcomes { get; init; }
        public List<LearningActivityDTO> NonScheduledLearningActivities { get; init; }
        public List<LearningActivityDTO> ScheduledLearningActivites { get; init; }
        private List<EnrolmentRuleDTO> _enrolmentRules;
        public List<EnrolmentRuleDTO> EnrolmentRules {
            get {
                return _enrolmentRules;
            }
            init {
                if (value is not null) {
                    _enrolmentRules = value;

                    //Original Implementation
                    //PrerequisiteParserOld.ParsePrerequisites(value, Code);

                    //Testing implementations
                    //EnrolmentRules.ForEach(x => PrerequisiteParserOld.ParsePrerequisiteString(x.Description));
                }
            }
        }
        public List<AssessmentDTO> Assessments { get; init; }
        public List<Requisite> Requisites { get; init; }
        public List<UnitOfferingDTO> UnitOffering { get; init; }
        public string UnitOfferingText { get; init; }
        public string SubjectSearchTitle { get; init; }
        public bool PublishTuitionFees { get; init; }
        public bool D_gov_cohort_year { get; init; }
        public string DurationFullTimeMax { get; init; }
        public string DurationPartTimeMax { get; init; }
        public string DurationPartTimeStandard { get; init; }
        public string DurationPartTimeMinimum { get; init; }
        public string DurationPartTimePeriod { get; init; }
    }
}