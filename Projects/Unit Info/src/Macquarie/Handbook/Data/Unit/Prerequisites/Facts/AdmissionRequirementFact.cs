using System;
using Macquarie.Handbook.Data.Transcript.Facts;
using Macquarie.Handbook.Data.Transcript.Facts.Providers;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts
{
    public class AdmissionRequirementFact : IRequirementFact
    {
        public  ICourseRequirement Fact { get; init; }

        public bool RequirementMet(ITranscriptFactProvider resultsProvider) {
            return Fact is not null && Fact.RequirementMet(resultsProvider);
        }

        public override string ToString() {
            return $"Admission to {Fact.ToString()}";
        }
    }
}