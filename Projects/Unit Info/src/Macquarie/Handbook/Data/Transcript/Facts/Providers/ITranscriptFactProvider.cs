using System.Collections.Generic;

namespace Macquarie.Handbook.Data.Transcript.Facts.Providers
{
    public interface ITranscriptFactProvider : IEnumerable<ITranscriptFact> {
        public bool GetFact(string key, out ITranscriptFact result);
    }
}