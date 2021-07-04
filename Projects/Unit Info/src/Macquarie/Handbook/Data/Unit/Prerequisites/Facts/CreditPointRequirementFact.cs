using System;
using System.Linq;
using Macquarie.Handbook.Data.Transcript;
using Macquarie.Handbook.Data.Transcript.Facts;
using Macquarie.Handbook.Data.Unit.Transcript.Facts;
using Macquarie.Handbook.Helpers.Extensions;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts
{
    public class CreditPointRequirementFact : IRequirementFact
    {
        private int _creditPoints;
        public int CreditPoints {
            get => _creditPoints;
            init => _creditPoints = value.Clamp(0, Int32.MaxValue); //TODO Revise maximum clamp value.
        }

        public StudyLevelRequirement StudyLevelRequirement { get; init; }

        public IRequirementFact IncludingFacts { get; init; } = null;

        public CreditPointRequirementFact(StudyLevelRequirement studyLevelRequirement) {
            StudyLevelRequirement = studyLevelRequirement;
        }

        public bool RequirementMet(ITranscriptFactProvider resultsProvider) {
            // Determine which comparison function we are going to use to decide if a passed unit fulfils the 
            // requirment. 
            StudyLevelRequirement.StudyLevelComparison compareFunc = StudyLevelRequirement.OrAbove ? StudyLevelRequirement.StudyLevelEqualOrGreaterComparison : StudyLevelRequirement.StudyLevelEqualComparison;
            // Get a list of all non-null, UnitFact instances then count all UnitFacts
            // which meet the comparison function and exceed the a PASS grade.
            // Might need to include way of considering specific grades on units, i.e. COMP1000 (D)
            //
            // Check 'including' conditions
            var creditPointAttained = (from fact in resultsProvider
                                       where fact is not null && fact is UnitFact
                                       select fact).Count(f =>
                                       {
                                           var fact = f as UnitFact;

                                            // Check if study level matches exactly, or matches or is greater than.
                                           if (compareFunc(fact.StudyLevel)) {

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

            //         StudyLevelRequirement.StudyLevelComparison compareFunc = StudyLevelRequirement.OrAbove ? StudyLevelRequirement.StudyLevelEqualOrGreaterComparison : StudyLevelRequirement.StudyLevelEqualComparison;

            //         if (compareFunc(fact.StudyLevel)) {
            //             return fact.Grade >= EnumGrade.Pass;
            //         }

            //     }
            //     return false;
            // });
#endregion

            return creditPointAttained >= CreditPoints;
        }
    }

    public class StudyLevelRequirement
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

        public StudyLevelRequirement(EnumStudyLevel studyLevel, bool orAbove = false) {
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