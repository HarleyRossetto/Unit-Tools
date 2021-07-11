using System.Collections;
using System.Collections.Generic;
using Macquarie.Handbook.Data.Transcript.Facts;
using Macquarie.Handbook.Data.Transcript.Facts.Providers;

namespace Unit_Info.src.Macquarie.Handbook.Data.Transcript.Facts.Providers
{
    public class TranscriptFactSingleUnitProvider : ITranscriptFactProvider
    {
        private UnitFact Fact { get; init; }

        public TranscriptFactSingleUnitProvider(UnitFact unit) => Fact = unit;

        public IEnumerator<ITranscriptFact> GetEnumerator() {
            yield return Fact;
        }
        public bool GetFact(string key, out ITranscriptFact result) {
            if (Fact.UnitCode == key) {
                result = Fact;
                return true;
            } else {
                result = null;
                return false;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            yield return Fact;
        }
    }
}