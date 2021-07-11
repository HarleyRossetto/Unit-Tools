using System.Collections.Generic;
using System.Linq;
using System.Text;
using Macquarie.Handbook.Data.Transcript.Facts.Providers;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts
{
    public abstract class ListRequirementFact : IRequirementFact
    {
        public List<IRequirementFact> Facts { get; set; }
        public abstract bool RequirementMet(ITranscriptFactProvider resultsProvider);

        public abstract override string ToString();

        protected bool ContainsFacts() {
            return Facts.Any();
        }

        protected string GetFactListAsString(string seperator) {
            StringBuilder sb = new("(");
            for (int i = 0; i < Facts.Count; i++) {
                sb.Append(Facts[i].ToString());
                if (i < Facts.Count - 1)
                    sb.Append($" {seperator} ");
            }
            sb.Append(')');
            return sb.ToString();
        }
    }
}