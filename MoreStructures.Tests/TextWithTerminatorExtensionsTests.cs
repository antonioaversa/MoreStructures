namespace MoreStructures.Tests;

[TestClass]
public class TextWithTerminatorExtensionsTests
{
    [TestMethod]
    public void ToVirtuallyRotated()
    {
        var text = new TextWithTerminator("a");
        var vrtext1 = text.ToVirtuallyRotated(1);
        Assert.AreEqual('$', vrtext1[0]);

        var vrtext2 = text.ToVirtuallyRotated(2);
        Assert.AreEqual('a', vrtext2[0]);
    }

    [DataRow("", TextWithTerminator.DefaultTerminator, new string[] { }, 
        new char[] { TextWithTerminator.DefaultTerminator })]
    [DataRow("", '1', new string[] { "" }, new char[] { '1' })]
    [DataRow("1", '2', new string[] { "", "" }, new char[] { '1', '2' })]
    [DataRow("a1a", '2', new string[] { "a", "a" }, new char[] { '1', '2' })]
    [DataRow("a1b", '2', new string[] { "a", "b" }, new char[] { '1', '2' })]
    [DataRow("a1b2ab", '3', new string[] { "a", "b", "ab" }, new char[] { '1', '2', '3' })]
    [DataRow("a12ab", '3', new string[] { "a", "", "ab" }, new char[] { '1', '2', '3' })]
    [DataRow("1a2aa3babaa", '4', new string[] { "", "a", "aa", "babaa" }, new char[] { '1', '2', '3', '4'})]
    [DataTestMethod]
    public void GenerateFullText_IsCorrect(
        string expectedText, char expectedTerminator, string[] textContents, char[] terminators)
    {
        var texts = textContents
            .Zip(terminators)
            .Select(textAndTerminator => new TextWithTerminator(textAndTerminator.First, textAndTerminator.Second))
            .ToArray();

        var text = texts.GenerateFullText();
        Assert.AreEqual(expectedText, string.Concat(text.fullText.Text));
        Assert.AreEqual(expectedTerminator, text.fullText.Terminator);
        Assert.IsTrue(terminators.ToHashSet().SetEquals(text.terminators));
    }

    [TestMethod]
    public void GenerateFullText_RaiseExceptionOnRepeatedTerminators()
    {
        Assert.ThrowsException<ArgumentException>(
            () => new TextWithTerminator[] { new("a", '1'), new("b", '1') }
                .GenerateFullText());
        Assert.ThrowsException<ArgumentException>(
            () => new TextWithTerminator[] { new("a", '1'), new("b", '2'), new("c", '1') }
                .GenerateFullText());
    }
}
