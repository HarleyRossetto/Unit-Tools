using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Macquarie.Handbook.Data.Transcript.Facts.Providers;

public class TranscriptFactIEnumerableProvider : ITranscriptFactProvider
{
    private readonly IEnumerable<ITranscriptFact> collection;

    public TranscriptFactIEnumerableProvider(IEnumerable<ITranscriptFact> ienumerable) => collection = ienumerable;

    public IEnumerator<ITranscriptFact> GetEnumerator() {
        return collection.GetEnumerator();
    }

    public bool GetFact(string key, out ITranscriptFact result) {
        result = collection.First((fact) => {
            return fact.GetKey() == key;
        });

        return result is not null;
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return collection.GetEnumerator();
    }
}
