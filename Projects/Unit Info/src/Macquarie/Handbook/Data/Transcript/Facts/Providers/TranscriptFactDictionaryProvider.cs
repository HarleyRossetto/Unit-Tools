using System.Collections;
using System.Collections.Generic;
using Macquarie.Handbook.Data.Transcript.Facts;

namespace Macquarie.Handbook.Data.Unit.Transcript.Facts.Providers
{
    public class TranscriptFactDictionaryProvider : ITranscriptFactProvider
    {
        public Dictionary<string, ITranscriptFact> RequirementFacts { get; set; }

        public IEnumerator<ITranscriptFact> GetEnumerator() {
            return RequirementFacts.Values.GetEnumerator();
        }

        public bool GetFact(string key, out ITranscriptFact result) {
            result = null;

            if (RequirementFacts is null) return false;

            return RequirementFacts.TryGetValue(key, out result);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)RequirementFacts.Values).GetEnumerator();
        }
    }
}