using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.SuffixStructures;

namespace StringAlgorithms.Tests.SuffixStructures;

[TestClass] 
public class SuffixStructuresBuilderExtensionsTests
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
