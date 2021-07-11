using System;
using System.Linq;
using System.Text;
using Macquarie.Handbook.Data.Transcript;
using Macquarie.Handbook.Data.Transcript.Facts;
using Macquarie.Handbook.Data.Unit.Transcript.Facts;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts
{
    public class CreditPointRequirementFact : IRequirementFact
    {
        private const int CREDIT_POINT_VALUE_2020 = 10;

        public CreditPoint CreditPoints { get; init; }

        public StudyLevelDescriptor StudyLevelRequirement { get; init; }

        public IRequirementFact IncludingFact { get; init; } = null;

        public CreditPointRequirementFact(CreditPoint credits, StudyLevelDescriptor studyLevelRequirement = null) {
            CreditPoints = credits;
            StudyLevelRequirement = studyLevelRequirement;
        }

        public bool RequirementMet(ITranscriptFactProvider resultsProvider) {
            bool creditPointRequirementMet = IsCreditPointRequirementMet(resultsProvider);
            bool includingFactMet = IsIncludingFactMet(resultsProvider);

            return creditPointRequirementMet && (IncludingFact is null || includingFactMet);
        }

        private bool IsIncludingFactMet(ITranscriptFactProvider resultsProvider) {
            bool includingFactMet = false;

            if (IncludingFact is not null)
                includingFactMet = IncludingFact.RequirementMet(resultsProvider);

            return includingFactMet;
        }

        private bool IsCreditPointRequirementMet(ITranscriptFactProvider resultsProvider) {
            int creditPointAttained = ValidCreditPointAttained(resultsProvider);
            var creditPointRequirementMet = creditPointAttained >= CreditPoints.Value;
            return creditPointRequirementMet;
        }

        private int ValidCreditPointAttained(ITranscriptFactProvider resultsProvider) {
            // Determine which comparison function we are going to use to decide if a passed unit fulfils the 
            // requirment. 
            StudyLevelDescriptor.StudyLevelComparison comparisonFunc = null;

            if (StudyLevelRequirement is not null) {
                comparisonFunc = (bool)(StudyLevelRequirement?.OrAbove) ? StudyLevelRequirement.StudyLevelEqualOrGreaterComparison
                                                                        : StudyLevelRequirement.StudyLevelEqualComparison;
            }
            // Get a list of all non-null, UnitFact instances then count all UnitFacts
            // which meet the comparison function and exceed the a PASS grade.
            // Might need to include way of considering specific grades on units, i.e. COMP1000 (D)
            //
            // Check 'including' conditions
            // Need to verify ABCD units rules, seperate rule object?
            var creditPointAttained = (from fact in resultsProvider
                                       where fact is not null && fact is UnitFact
                                       select fact).Count(f => {
                                           var fact = f as UnitFact;

                                           bool gradeMet = false;
                                           // Study level is 'No Level' or #000 level.
                                           // Check if study level matches exactly, or matches or is greater than.
                                           if (comparisonFunc is null || comparisonFunc(fact.StudyLevel)) {
                                               // Ensure subject is at least pass.
                                               gradeMet = fact.Grade >= EnumGrade.Pass;
                                           }

                                           return gradeMet;
                                       }) * CREDIT_POINT_VALUE_2020;
            return creditPointAttained;
        }

        public override string ToString() {
            var sb = new StringBuilder().Append(CreditPoints).Append("cp");

            if (StudyLevelRequirement is not null) {
                sb.AppendFormat(" at {0} level", StudyLevelConverter.ToInt(StudyLevelRequirement.StudyLevel));
                if (StudyLevelRequirement.OrAbove) {
                    sb.Append(" or above");
                }
            }
            if (IncludingFact is not null) {
                sb.AppendFormat(" including {0}", IncludingFact.ToString());
            }
            return sb.ToString();
        }
    }

    public class StudyLevelDescriptor
    {

        private EnumStudyLevel _studyLevel;
        public EnumStudyLevel StudyLevel {
            get => _studyLevel;
            init {
                if ((int)value >= 0 && (int)value <= 7 && (int)value != 5) {
                    _studyLevel = value;
                } else {
                    throw new ArgumentOutOfRangeException($"Study Level value {value} is invalid.");
                }
            }
        }

        public bool OrAbove { get; init; }

        public StudyLevelDescriptor(EnumStudyLevel studyLevel, bool orAbove = false) {
            StudyLevel = studyLevel;
            OrAbove = orAbove;
        }

        public delegate bool StudyLevelComparison(EnumStudyLevel otherLevel);

        public bool StudyLevelEqualComparison(EnumStudyLevel otherLevel) {
            return StudyLevel == otherLevel;
        }

        public bool StudyLevelEqualOrGreaterComparison(EnumStudyLevel otherRequirement) {
            return StudyLevel >= otherRequirement;
        }
    }
}