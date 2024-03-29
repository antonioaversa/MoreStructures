﻿using MoreStructures.SuffixArrays.CyclicShifts;

namespace MoreStructures.Tests.SuffixArrays.CyclicShifts;

[TestClass]
public class NaiveDoubleLengthPcsClassifierTests : DoubleLengthPcsClassifierTests
{
    public NaiveDoubleLengthPcsClassifierTests() 
        : base(cbi => new NaiveDoubleLengthPcsClassifier(cbi.Input, cbi.PcsLength, cbi.InputWithTerminator))
    {
    }

    [TestMethod]
    public void Ctor_RaisesExceptionWithInvalidPcsLength()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new NaiveDoubleLengthPcsClassifier("a", -1, false));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new NaiveDoubleLengthPcsClassifier("a", 0, false));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new NaiveDoubleLengthPcsClassifier("a", 2, false));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new NaiveDoubleLengthPcsClassifier("", 1, false));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new NaiveDoubleLengthPcsClassifier("abc", 4, false));
    }
}
