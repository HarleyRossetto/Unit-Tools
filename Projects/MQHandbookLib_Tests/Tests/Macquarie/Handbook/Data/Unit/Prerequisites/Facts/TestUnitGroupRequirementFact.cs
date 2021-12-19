using System;
using System.Collections.Generic;
using Macquarie.Handbook.Data.Transcript.Facts;
using Macquarie.Handbook.Data.Transcript.Facts.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Macquarie.Handbook.Data.Unit.Prerequisites.Facts;

[TestClass]
public class TestUnitGroupRequirementFact
{
    [TestMethod]
    public void TestUnitGroupProperty() {
        //No value
        UnitGroupRequirementFact fact = new();
        Assert.AreEqual(String.Empty, fact.UnitGroup);

        //Null
        fact = new();
        Assert.AreEqual(String.Empty, fact.UnitGroup);

        //Valid
        fact = new() {
            UnitGroup = "COMP"
        };
        Assert.AreEqual("COMP", fact.UnitGroup);
    }

    [TestMethod]
    public void TestRequirementMet() {
        //Null provider
        UnitGroupRequirementFact fact = new() {
            UnitGroup = "COMP"
        };

        Assert.IsFalse(fact.RequirementMet(null));

        //No group, no provider
        fact = new();
        Assert.IsFalse(fact.RequirementMet(null));

        //No group, valid provider
        Assert.IsFalse(fact.RequirementMet(new TranscriptFactIEnumerableProvider(new List<ITranscriptFact>())));

        //No group, valid provider with valid unit
        UnitFact unit = new() {
            UnitCode = "COMP1010"
        };

        TranscriptFactIEnumerableProvider provider = new(new List<ITranscriptFact>() { unit });
        Assert.IsFalse(fact.RequirementMet(provider));

        //Valid group with valid provider and unit
        fact = new() {
            UnitGroup = "COMP"
        };
        Assert.IsTrue(fact.RequirementMet(provider));

        //Valid group with valid provider and invalid unit
        fact = new() {
            UnitGroup = "LAWS"
        };
        Assert.IsFalse(fact.RequirementMet(provider));

        //Valid group with valid provider and incorrect transcript fact type
        CourseFact course = new();
        provider = new(new List<ITranscriptFact>() { course });
        Assert.IsFalse(fact.RequirementMet(provider));

        //Valid group with valid provider and null transcript facts
        provider = new(new List<ITranscriptFact>() { null });
        Assert.IsFalse(fact.RequirementMet(provider));
    }
}
