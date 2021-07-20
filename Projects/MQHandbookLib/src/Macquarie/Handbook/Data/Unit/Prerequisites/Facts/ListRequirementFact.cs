using System.Collections.Generic;
using System.Linq;
using System.Text;
using Macquarie.Handbook.Data.Transcript.Facts.Providers;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts
{
    public abstract class ListRequirementFact<T> : IRequirementFact, ICourseRequirement, IUnitRequirement where T : IRequirementFact
    {
        public List<T> Facts { get; set; }
        public abstract bool RequirementMet(ITranscriptFactProvider resultsProvider);

        public abstract override string ToString();

        protected bool ContainsFacts() {
            return Facts is not null && Facts.Any();
        }

        protected string GetFactListAsString(string seperator) {
            StringBuilder sb = new();
            if (Facts.Count > 1) sb.Append('('); //If multiple items, encase in parenthese
            for (int i = 0; i < Facts.Count; i++) {
                sb.Append(Facts[i].ToString());
                if (i < Facts.Count - 1) //If not the last element, append the seperator
                    sb.Append($" {seperator} ");
            }
            if (Facts.Count > 1) sb.Append(')'); //If multiple items, encase in parenthese
            return sb.ToString();
        }
    }
}