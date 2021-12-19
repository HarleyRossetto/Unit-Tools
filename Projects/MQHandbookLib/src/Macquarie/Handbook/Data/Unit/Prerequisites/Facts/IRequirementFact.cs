using Macquarie.Handbook.Data.Transcript.Facts.Providers;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts
{
    public interface IRequirementFact
    {
        public bool RequirementMet(ITranscriptFactProvider resultsProvider);
        public abstract string ToString();
    }

}