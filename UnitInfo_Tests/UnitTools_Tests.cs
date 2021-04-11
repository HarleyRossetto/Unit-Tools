using Microsoft.VisualStudio.TestTools.UnitTesting;

using Macquarie.Handbook.Data.Unit.Prerequisites;
using Macquarie.Handbook.Helpers;

namespace UnitTools_Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestPreRequisiteParsing() {
            string input = "(Admission to BEd(Prim) and (EDUC258 or EDUC2580) and (EDUC260 or EDUC2600) and (EDUC267 or EDUC2670)) or (130cp including (EDUC258 or EDUC2580) and (EDUC260 or EDUC2600) and (EDUC267 or EDUC2670) and (EDTE353 or EDTE3530))";

            PrerequisiteParser.ParsePrerequisites(new EnrolmentRule[] { new EnrolmentRule() { Description = input, Type = new Macquarie.Handbook.Data.Shared.LabelledValue() {Value  = "prerequisite"}} }, "EDTU3010");
        }
    }
}
