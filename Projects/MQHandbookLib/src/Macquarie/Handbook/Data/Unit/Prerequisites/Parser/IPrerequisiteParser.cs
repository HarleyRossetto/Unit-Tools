namespace Macquarie.Handbook.Data.Unit.Prerequisites.Parser;

using Macquarie.Handbook.Data.Unit.Prerequisites.Facts;


public interface IPrerequisiteParser
{
    public IRequirementFact Parse(string sourceText);
}
