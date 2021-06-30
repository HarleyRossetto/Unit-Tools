using Microsoft.VisualStudio.TestTools.UnitTesting;

using Macquarie.Handbook.Helpers.Prerequisites.Parser;

namespace UnitTools_Tests
{
    [TestClass]
    public class PrerequisiteParserTests
    {
        [TestMethod]
        public void TestParse() {
            string inputOne = "(Admission to BEd(Prim) and (EDUC258 or EDUC2580) and (EDUC260 or EDUC2600) and (EDUC267 or EDUC2670)) or (130cp including (EDUC258 or EDUC2580) and (EDUC260 or EDUC2600) and (EDUC267 or EDUC2670) and (EDTE353 or EDTE3530))";
            PrerequisiteParser.Parse(inputOne);


            string inputTwo = "Admission to BEd(Prim) and (EDUC258 or EDUC2580) and (EDUC260 or EDUC2600) and (EDUC267 or EDUC2670)";
            PrerequisiteParser.Parse(inputTwo);
        }
    }
}