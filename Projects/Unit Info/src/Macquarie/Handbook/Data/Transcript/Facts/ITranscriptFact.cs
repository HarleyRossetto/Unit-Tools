namespace Macquarie.Handbook.Data.Transcript.Facts
{
    //Marker interface
    public interface ITranscriptFact
    {
        
    }

    
    public interface ITranscriptFactProvider {
        public bool GetFact(string key, out ITranscriptFact result);
    }
}