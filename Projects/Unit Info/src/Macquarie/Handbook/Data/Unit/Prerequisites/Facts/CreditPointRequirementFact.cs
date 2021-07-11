using System.Collections.Generic;
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

        public StudyLevelDescriptor StudyLevelRequirement { get; init; } = new StudyLevelDescriptor(EnumStudyLevel.Level1000, true);

        public IRequirementFact IncludingFact { get; init; } = null;

        public CreditPointRequirementFact(CreditPoint credits, StudyLevelDescriptor studyLevelRequirement = null) {
            CreditPoints = credits;

            if (studyLevelRequirement is not null)
                StudyLevelRequirement = studyLevelRequirement;
        }

        public bool RequirementMet(ITranscriptFactProvider resultsProvider) {
            bool creditPointRequirementMet = IsCreditPointRequirementMet(resultsProvider);

            if (creditPointRequirementMet) {
                bool includingFactMet = IsIncludingFactMet(resultsProvider);
                return IncludingFact is null || includingFactMet;
            }

            return false;
        }

        private bool IsCreditPointRequirementMet(ITranscriptFactProvider resultsProvider) {
            return TotalCreditPointsAttained(resultsProvider) >= CreditPoints.Value;
        }

        /// <summary>
        /// Calculates the total number of credit points that have been attained based on the 
        /// number of units whose grade were a pass or higher.
        /// </summary>
        /// <param name="resultsProvider">Provider interface for accessing transcription facts</param>
        /// <returns></returns>
        private int TotalCreditPointsAttained(ITranscriptFactProvider resultsProvider) {
            return (from fact in resultsProvider
                    where fact is not null && fact is UnitFact
                    let unit = fact as UnitFact
                    where unit.Grade >= EnumGrade.Pass && StudyLevelRequirement.Comparator(unit.StudyLevel)
                    select fact).Count() * CREDIT_POINT_VALUE_2020;
        }

        private bool IsIncludingFactMet(ITranscriptFactProvider resultsProvider) {
            bool includingFactMet = false;

            if (IncludingFact is not null)
                includingFactMet = IncludingFact.RequirementMet(resultsProvider);

            return includingFactMet;
        }

        public IEnumerable<ITranscriptFact> GetAwardedTranscriptFacts(ITranscriptFactProvider resultsProvider) {
            return from fact in resultsProvider
                   where fact is not null && fact is UnitFact
                   let unit = fact as UnitFact
                   where unit.Grade >= EnumGrade.Pass && StudyLevelRequirement.Comparator(unit.StudyLevel)
                   select fact;
        }

        private bool StudyLevelRequirementMet(IEnumerable<ITranscriptFact> passedUnits) {
            // Determine which comparison function we are going to use to decide if a passed unit fulfils the 
            // requirment. 
            if (StudyLevelRequirement is not null) {
                var pointsAttainedAtLevel = (from unit in passedUnits
                                             where StudyLevelRequirement.Comparator((unit as UnitFact).StudyLevel)
                                             select unit).Count() * CREDIT_POINT_VALUE_2020;

                return pointsAttainedAtLevel >= CreditPoints.Value;

            }
            return false;
        }

        private int ValidCreditPointAttained(ITranscriptFactProvider resultsProvider) {

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
                                           // Study level is 'No Level' or #000 level.
                                           // Check if study level matches exactly, or matches or is greater than.
                                           // Ensure subject is at least pass.
                                           return (fact.Grade >= EnumGrade.Pass) && StudyLevelRequirement.Comparator(fact.StudyLevel);
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
}