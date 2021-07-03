using System.Collections.Generic;
using Macquarie.Handbook.Data.Transcript.Facts;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts
{
    public abstract class ListRequirementFact : IRequirementFact
    {
        public List<IRequirementFact> Facts { get; set; }
        public abstract bool RequirementMet(ITranscriptFactProvider resultsProvider);
    }
}