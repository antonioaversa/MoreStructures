using MoreStructures.SuffixArrays.CyclicShifts;

namespace MoreStructures.Tests.SuffixArrays.CyclicShifts;

public abstract class SingleCharPcsClassifierTests
{
    protected Func<string, ISingleCharPCSClassifier> ClassifierBuilder { get; }

    protected SingleCharPcsClassifierTests(Func<string, ISingleCharPCSClassifier> classifierBuilder)
    {
        ClassifierBuilder = classifierBuilder;
    }

    [DataRow("", new int[] { })]
    [DataRow("a", new int[] { 0 })]
    [DataRow("aa", new int[] { 0, 0 })]
    [DataRow("aba", new int[] { 0, 1, 0 })]
    [DataRow("cabab", new int[] { 2, 0, 1, 0, 1 })]
    [DataRow("ccaebfaeeb", new int[] { 2, 2, 0, 3, 1, 4, 0, 3, 3, 1 })]
    [DataTestMethod]
    public void Classify_IsCorrect(string input, int[] expectedResult)
    {
        var classifier = ClassifierBuilder(input);
        var result = classifier.Classify();
        Assert.IsTrue(
            expectedResult.SequenceEqual(result), 
            $"Expected: [{string.Join(",", expectedResult)}]. Got: [{string.Join(", ", result)}]");
    }
}
