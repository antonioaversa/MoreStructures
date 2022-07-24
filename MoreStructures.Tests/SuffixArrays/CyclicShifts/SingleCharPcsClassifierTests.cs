using MoreStructures.SuffixArrays.CyclicShifts;

namespace MoreStructures.Tests.SuffixArrays.CyclicShifts;

public abstract class SingleCharPcsClassifierTests
{
    public sealed record ClassifierBuilderInput(string Input, bool InputWithTerminator);

    protected Func<ClassifierBuilderInput, ISingleCharPcsClassifier> ClassifierBuilder { get; }

    protected SingleCharPcsClassifierTests(Func<ClassifierBuilderInput, ISingleCharPcsClassifier> classifierBuilder)
    {
        ClassifierBuilder = classifierBuilder;
    }

    // Without terminator
    [DataRow("", false, new int[] { })]
    [DataRow("a", false, new int[] { 0 })]
    [DataRow("aa", false, new int[] { 0, 0 })]
    [DataRow("aba", false, new int[] { 0, 1, 0 })]
    [DataRow("cabab", false, new int[] { 2, 0, 1, 0, 1 })]
    [DataRow("ccaebfaeeb", false, new int[] { 2, 2, 0, 3, 1, 4, 0, 3, 3, 1 })]
    // With terminator
    [DataRow("", true, new int[] { })]
    [DataRow("a", true, new int[] { 0 })]
    [DataRow("aa", true, new int[] { 0, 0 })]
    [DataRow("aba", true, new int[] { 0, 1, 0 })]
    [DataRow("cabab", true, new int[] { 2, 1, 0, 1, 0 })]
    [DataRow("ccaebfaeeb", true, new int[] { 2, 2, 1, 3, 0, 4, 1, 3, 3, 0 })]
    [DataTestMethod]
    public void Classify_IsCorrect(string input, bool inputWithTerminator, int[] expectedResult)
    {
        var classifierBuilderInput = new ClassifierBuilderInput(input, inputWithTerminator);
        var classifier = ClassifierBuilder(classifierBuilderInput);
        var result = classifier.Classify();
        Assert.IsTrue(
            expectedResult.SequenceEqual(result), 
            $"Expected: [{string.Join(",", expectedResult)}], Actual: [{string.Join(", ", result)}]");
    }
}
