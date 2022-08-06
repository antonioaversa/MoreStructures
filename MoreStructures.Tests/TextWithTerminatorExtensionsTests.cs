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
        new[] { TextWithTerminator.DefaultTerminator })]
    [DataRow("", '1', new[] { "" }, new[] { '1' })]
    [DataRow("1", '2', new[] { "", "" }, new[] { '1', '2' })]
    [DataRow("a1a", '2', new[] { "a", "a" }, new[] { '1', '2' })]
    [DataRow("a1b", '2', new[] { "a", "b" }, new[] { '1', '2' })]
    [DataRow("a1b2ab", '3', new[] { "a", "b", "ab" }, new[] { '1', '2', '3' })]
    [DataRow("a12ab", '3', new[] { "a", "", "ab" }, new[] { '1', '2', '3' })]
    [DataRow("1a2aa3babaa", '4', new[] { "", "a", "aa", "babaa" }, new[] { '1', '2', '3', '4'})]
    [DataTestMethod]
    public void GenerateFullText_IsCorrect(
        string expectedText, char expectedTerminator, string[] textContents, char[] terminators)
    {
        var texts = textContents
            .Zip(terminators)
            .Select(textAndTerminator => new TextWithTerminator(textAndTerminator.First, textAndTerminator.Second))
            .ToArray();

        var fullText = texts.GenerateFullText();
        Assert.AreEqual(expectedText, string.Concat(fullText.fullText.Text));
        Assert.AreEqual(expectedTerminator, fullText.fullText.Terminator);
        Assert.IsTrue(terminators.ToHashSet().SetEquals(fullText.terminators));
        Assert.IsTrue(texts.All(text => terminators.Count(terminator => text.Terminator == terminator) == 1));
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

    [DataRow(new[] { "1" }, new[] { 1 })]
    [DataRow(new[] { "1", "2" }, new[] { 1, 2 })]
    [DataRow(new[] { "a1", "a2" }, new[] { 0, 1, 1, 2 })]
    [DataRow(new[] { "a1", "b2" }, new[] { 0, 1, 1, 2 })]
    [DataRow(new[] { "a1", "b2", "ab3" }, new[] { 0, 1, 1, 2, 2, 2, 3 })]
    [DataRow(new[] { "a1", "2", "ab3" }, new[] { 0, 1, 2, 2, 2, 3 })]
    [DataRow(new[] { "1", "a2", "aa3", "babaa4" }, new[] { 1, 1, 2, 2, 2, 3, 3, 3, 3, 3, 3, 4 })]
    [DataTestMethod]
    public void BuildTerminatorsCDF_IsCorrect(string[] textContentsWithTerminator, int[] expectedResult)
    {
        var texts = textContentsWithTerminator
            .Select(textWithTerminator => new TextWithTerminator(textWithTerminator[..^1], textWithTerminator[^1]))
            .ToArray();
        var (fullText, terminators) = texts.GenerateFullText();

        var result = TextWithTerminatorExtensions.BuildTerminatorsCDF(fullText, terminators);
        Assert.IsTrue(expectedResult.SequenceEqual(result),
            $"Expected [{string.Join(", ", expectedResult)}], Got: [{string.Join(", ", result)}]");
    }
}
