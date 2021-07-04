using System.Collections.Generic;

namespace Macquarie.Handbook.Data.Transcript.Facts
{
    //Marker interface
    public interface ITranscriptFact
    {
        
    }

    
    public interface ITranscriptFactProvider : IEnumerable<ITranscriptFact> {
        public bool GetFact(string key, out ITranscriptFact result);
    }
}