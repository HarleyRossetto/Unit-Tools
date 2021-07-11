using System.Collections;
using System.Collections.Generic;
using Macquarie.Handbook.Data.Transcript.Facts;
using Macquarie.Handbook.Data.Transcript.Facts.Providers;

namespace Macquarie.Handbook.Data.Unit.Transcript.Facts.Providers
{
    public class TranscriptFactDictionaryProvider : ITranscriptFactProvider
    {
        public Dictionary<string, ITranscriptFact> transcriptFacts;

        public TranscriptFactDictionaryProvider(Dictionary<string, ITranscriptFact> dictionary) {
            transcriptFacts = dictionary;
        }

        public IEnumerator<ITranscriptFact> GetEnumerator() {
            return transcriptFacts.Values.GetEnumerator();
        }

        public bool GetFact(string key, out ITranscriptFact result) {
            result = null;

            if (transcriptFacts is null) return false;

            return transcriptFacts.TryGetValue(key, out result);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)transcriptFacts.Values).GetEnumerator();
        }
    }
}