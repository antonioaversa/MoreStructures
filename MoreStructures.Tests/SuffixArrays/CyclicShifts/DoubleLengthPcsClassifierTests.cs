using MoreStructures.SuffixArrays.CyclicShifts;

namespace MoreStructures.Tests.SuffixArrays.CyclicShifts;

public abstract class DoubleLengthPcsClassifierTests
{
    public sealed record ClassifierBuilderInput(
        string Input, int PcsLength, int[] Order, int[] EqClassesPcsHalfLength, bool Terminator);

    protected Func<ClassifierBuilderInput, IDoubleLengthPcsClassifier> ClassifierBuilder { get; }

    protected DoubleLengthPcsClassifierTests(
        Func<ClassifierBuilderInput, IDoubleLengthPcsClassifier> classifierBuilder)
    {
        ClassifierBuilder = classifierBuilder;
    }

    // PcsLength 2 and no terminator
    [DataRow("ab", 2, new[] { 0, 1 }, new[] { 0, 1 }, false,
        new[] { 0, 1 })]
    [DataRow("aab", 2, new[] { 0, 1, 2 }, new[] { 0, 0, 1 }, false,
        new[] { 0, 1, 2 })]
    [DataRow("aba", 2, new[] { 2, 0, 1 }, new[] { 0, 1, 0 }, false,
        new[] { 1, 2, 0 })]
    [DataRow("abaa", 2, new[] { 2, 3, 0, 1 }, new[] { 0, 1, 0, 0 }, false,
        new[] { 1, 2, 0, 0 })]
    [DataRow("abac", 2, new[] { 0, 2, 1, 3 }, new[] { 0, 1, 0, 2 }, false,
        new[] { 0, 2, 1, 3 })]
    [DataRow("abacbba", 2, new[] { 6, 0, 2, 1, 5, 4, 3 }, new[] {0, 1, 0, 2, 1, 1, 0 }, false,
        new[] { 1, 3, 2, 5, 4, 3, 0 })]
    [DataRow("babbaab", 2, new[] { 4, 1, 5, 0, 3, 2, 6 }, new[] { 1, 0, 1, 1, 0, 0, 1 }, false,
        new[] { 2, 1, 3, 2, 0, 1, 3 })]
    // PcsLength 2 and terminator
    [DataRow("ab", 2, new[] { 1, 0 }, new[] { 1, 0 }, true,
        new[] { 1, 0 })]
    [DataRow("aab", 2, new[] { 2, 1, 0 }, new[] { 1, 1, 0 }, true,
        new[] { 2, 1, 0 })]
    [DataRow("aba", 2, new[] { 2, 0, 1 }, new[] { 0, 1, 0 }, true,
        new[] { 1, 2, 0 })]
    [DataRow("abaa", 2, new[] { 2, 3, 0, 1 }, new[] { 0, 1, 0, 0 }, true,
        new[] { 1, 2, 0, 0 })]
    // PcsLength 4
    [DataRow("abaa", 4, new[] { 2, 3, 0, 1 }, new[] { 1, 2, 0, 0 }, false,
        new[] { 2, 3, 0, 1 })]
    [DataRow("abac", 4, new[] { 0, 2, 1, 3 }, new[] { 0, 2, 1, 3 }, false,
        new[] { 0, 2, 1, 3 })]
    [DataRow("abacbba", 4, new[] { 6, 0, 2, 5, 1, 4, 3 }, new[] { 1, 3, 2, 5, 4, 3, 0 }, false,
        new[] { 1, 4, 2, 6, 5, 3, 0 })]
    [DataRow("babbaab", 4, new[] { 4, 1, 5, 3, 0, 2, 6 }, new[] { 2, 1, 3, 2, 0, 1, 3 }, false,
        new[] { 3, 1, 4, 2, 0, 1, 5 })]
    // PcsLength 4 and terminator 
    [DataRow("abaa", 4, new[] { 2, 3, 0, 1 }, new[] { 1, 2, 0, 0 }, true,
        new[] { 2, 3, 0, 1 })]
    [DataRow("abac", 4, new[] { 3, 2, 0, 1 }, new[] { 2, 3, 1, 0 }, true,
        new[] { 2, 3, 1, 0 })]
    [DataRow("abacbba", 4, new[] { 6, 0, 2, 5, 1, 4, 3 }, new[] { 1, 3, 2, 5, 4, 3, 0 }, true,
        new[] { 1, 4, 2, 6, 5, 3, 0 })]
    [DataRow("babbaab", 4, new[] { 6, 2, 0, 3, 1, 5, 4 }, new[] { 1, 2, 0, 1, 3, 2, 0 }, true,
        new[] { 2, 4, 1, 3, 5, 4, 0 })]
    // PcsLength 6
    [DataRow("abacbba", 6, new[] { 6, 0, 2, 5, 1, 4, 3 }, new[] { 6, 0, 2, 5, 1, 4, 3 }, false,
        new[] { 1, 4, 2, 6, 5, 3, 0 })]
    [DataRow("babbaab", 6, new[] { 4, 1, 5, 3, 0, 2, 6 }, new[] { 3, 1, 4, 2, 0, 1, 4 }, false,
        new[] { 4, 1, 5, 3, 0, 2, 6 })]
    // PcsLength 6 and terminator
    [DataRow("abacbba", 6, new[] { 6, 0, 2, 5, 1, 4, 3 }, new[] { 6, 0, 2, 5, 1, 4, 3 }, true,
        new[] { 1, 4, 2, 6, 5, 3, 0 })]
    [DataRow("babbaab", 6, new[] { 6, 2, 0, 3, 5, 1, 4 }, new[] { 1, 3, 0, 2, 4, 3, 0 }, true,
        new[] { 2, 5, 1, 3, 6, 4, 0 })]
    [DataTestMethod]
    public void Classify_IsCorrect(
        string input, int pcsLength, int[] order, int[] eqClassesPcsHalfLength, bool terminator, int[] expectedResult)
    {
        var classifierBuilderInput = new ClassifierBuilderInput(
            input, pcsLength, order, eqClassesPcsHalfLength, terminator);
        var classifier = ClassifierBuilder(classifierBuilderInput);
        var result = classifier.Classify();
        Assert.IsTrue(
            expectedResult.SequenceEqual(result),
            $"Expected: [{string.Join(", ", expectedResult)}], Actual: [{string.Join(", ", result)}]");
    }


}
