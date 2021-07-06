using System;
using System.Linq;
using Macquarie.Handbook.Data.Transcript;
using Macquarie.Handbook.Data.Transcript.Facts;
using Macquarie.Handbook.Data.Unit.Transcript.Facts;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts
{
    public class CreditPointRequirementFact : IRequirementFact
    {
        public CreditPoint CreditPoints { get; init; }

        public StudyLevelDescriptor StudyLevelRequirement { get; init; }

        public IRequirementFact IncludingFacts { get; init; } = null;

        public CreditPointRequirementFact(StudyLevelDescriptor studyLevelRequirement) {
            StudyLevelRequirement = studyLevelRequirement;
        }

        public bool RequirementMet(ITranscriptFactProvider resultsProvider) {
            // Determine which comparison function we are going to use to decide if a passed unit fulfils the 
            // requirment. 
            StudyLevelDescriptor.StudyLevelComparison comparisonFunc = StudyLevelRequirement.OrAbove ? StudyLevelRequirement.StudyLevelEqualOrGreaterComparison : StudyLevelRequirement.StudyLevelEqualComparison;
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
                                           if (comparisonFunc(fact.StudyLevel)) {

                                               // Ensure subject is at least pass.
                                               return fact.Grade >= EnumGrade.Pass;
                                           }

                                           return false;
                                       });
            #region V1 Code
            // creditPointAttained = resultsProvider.Count((ITranscriptFact f) =>
            // {
            //     if (f is not null && f is UnitFact) {
            //         var fact = f as UnitFact;

            //         StudyLevelCreditPointRequirementFactDecorator.StudyLevelComparison comparisonFunc = StudyLevelCreditPointRequirementFactDecorator.OrAbove ? StudyLevelCreditPointRequirementFactDecorator.StudyLevelEqualOrGreaterComparison : StudyLevelCreditPointRequirementFactDecorator.StudyLevelEqualComparison;

            //         if (comparisonFunc(fact.StudyLevelDescriptor)) {
            //             return fact.Grade >= EnumGrade.Pass;
            //         }

            //     }
            //     return false;
            // });
            #endregion

            return creditPointAttained >= CreditPoints.Value;
        }
    }

    public class StudyLevelDescriptor
    {

        private EnumStudyLevel _studyLevel;
        public EnumStudyLevel StudyLevel {
            get => _studyLevel;
            init {
                if ((int)value >= 1 && (int)value <= 7 && (int)value != 5) {
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