using Microsoft.VisualStudio.TestTools.UnitTesting;

using Macquarie.Handbook.Helpers.Prerequisites;
using System.Collections.Generic;

namespace UnitTools_Tests
{
    [TestClass]
    public class PrerequisiteSanitiserTests 
    {
        [TestMethod]
        public void TestBracketedQualifiers() {
            string inputOne = "Admission to MCrWrit (OUA)";
            inputOne = ParenthesesSanitiser.ReplaceSquareBrackets(inputOne);
            const string resultOneExpected = "Admission to MCrWrit [OUA]";
            var resultOne = ParenthesesSanitiser.ReplaceBracketedQualifiers(inputOne);
            Assert.AreEqual(resultOneExpected, resultOne);

            string inputTwo = "20cp at 2000 level including PSY234(P) or PSY2234(P)";
            inputTwo = ParenthesesSanitiser.ReplaceSquareBrackets(inputTwo);
            const string resultTwoExpected = "20cp at 2000 level including PSY234[P] or PSY2234[P]";
            var resultTwo = ParenthesesSanitiser.ReplaceBracketedQualifiers(inputTwo);
            Assert.AreEqual(resultTwoExpected, resultTwo);

            string inputThree = "Admission to MAppFin(Beijing) and ECFL866 or AFCL8003";
            inputThree = ParenthesesSanitiser.ReplaceSquareBrackets(inputThree);
            const string resultThreeExpected = "Admission to MAppFin[Beijing] and ECFL866 or AFCL8003";
            var resultThree = ParenthesesSanitiser.ReplaceBracketedQualifiers(inputThree);
            Assert.AreEqual(resultThreeExpected, resultThree);
        }

        [TestMethod]
        public void TestSquareBracketsRemoval(){
            string inputOne = "[Admission to (MTeach(0-5) or GradCertEChild) and (ECED600 or ECHE6000)] or [admission to MEChild or MEd or MEdLead or MIndigenousEd or MSpecEd or GradCertEdS]";
            var resultsOne = ParenthesesSanitiser.ReplaceSquareBrackets(inputOne);
            Assert.IsFalse(resultsOne.Contains('['));
            Assert.IsFalse(resultsOne.Contains(']'));
        }
    }
}
