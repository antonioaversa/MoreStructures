using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.SuffixStructures.Builders;

namespace StringAlgorithms.Tests.SuffixStructures;

[TestClass] 
public class BuilderExtensionsTests
{
    [TestMethod]
    public void BuildTree_CallsTheBuilderWithAnEquivalentTextWithTerminator()
    {
        var builder = new SuffixStructureMock.Builder();
        builder.BuildTree("a string");
        Assert.IsTrue(builder.BuildTreeCalled);
        Assert.AreEqual(new TextWithTerminator("a string"), builder.BuildTreeArgument);
    }
}
