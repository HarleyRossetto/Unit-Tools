using System.Collections.Generic;
using Macquarie.Handbook.Data.Transcript.Facts;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts.Providers
{
    public class TranscriptFactDictionaryProvider : ITranscriptFactProvider
    {
        public Dictionary<string, ITranscriptFact> RequirementFacts { get; set; }
        public bool GetFact(string key, out ITranscriptFact result) {
            result = null;

            if (RequirementFacts is null) return false;

            return RequirementFacts.TryGetValue(key, out result);
        }
    }
}