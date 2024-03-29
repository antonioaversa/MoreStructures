﻿using MoreStructures.SuffixStructures.Builders;

namespace MoreStructures.Tests.SuffixStructures;

[TestClass] 
public class BuilderExtensionsTests
{
    [TestMethod]
    public void BuildTree_CallsTheBuilderWithAnEquivalentTextWithTerminator()
    {
        var builder = new SuffixStructureMock.Builder();
        builder.BuildTree("a string");
        Assert.IsTrue(builder.BuildTreeCalled);
        Assert.IsTrue(new[] { new TextWithTerminator("a string") }.SequenceEqual(
            builder.BuildTreeArgument ?? Array.Empty<TextWithTerminator>()));
    }
}
