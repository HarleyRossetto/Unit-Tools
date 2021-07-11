using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Macquarie.Handbook.Data.Transcript.Facts;

namespace Macquarie.Handbook.Data.Transcript.Facts.Providers
{
    public class TranscriptFactIEnumerableProvider : ITranscriptFactProvider
    {
        private readonly IEnumerable<ITranscriptFact> collection;

        public TranscriptFactIEnumerableProvider(IEnumerable<ITranscriptFact> ienumerable) => collection = ienumerable;

        public IEnumerator<ITranscriptFact> GetEnumerator() {
            return collection.GetEnumerator();
        }

        public bool GetFact(string key, out ITranscriptFact result) {
            // if (collection.Where((x) => {
            //     x.
            // }))

            //TODO consider this design more.
            result = null;
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return collection.GetEnumerator();
        }
    }
}