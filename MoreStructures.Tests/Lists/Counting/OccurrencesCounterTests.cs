using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.Lists.Counting;
using System.Collections.Generic;
using System.Linq;

namespace MoreStructures.Tests.Lists.Counting;

using static DictionaryBasedOccurrencesCounterTests.TestCaseId;

public abstract class OccurrencesCounterTests
{
    protected IOccurrencesCounter Counter { get; }

    protected OccurrencesCounterTests(IOccurrencesCounter counter)
    {
        Counter = counter;
    }

    public enum TestCaseId 
    { 
        EmptyEnumerable, 
        EmptyArray, 
        SingletonList, 
        SingletonArray, 
        ThreeElementsWithoutRepetitions,
        ThreeElementsWithOneRepetitions, 
        ThreeElementsWithTwoRepetitions,
        MultipleRepetedElements,
    }
    private record TestCase(IEnumerable<int> Enumerable, IEnumerable<int> ExpectedOutput);

    private static readonly Dictionary<TestCaseId, TestCase> TestCases = new()
    {
        [EmptyEnumerable] = new(Enumerable.Empty<int>(), Enumerable.Empty<int>()),
        [EmptyArray] = new(new int[] { }, new List<int> { }),
        [SingletonList] = new(new List<int> { 3 }, new List<int> { 0 }),
        [SingletonArray] = new(new int[] { 4 }, new List<int> { 0 }),
        [ThreeElementsWithoutRepetitions] = new(new List<int> { 1, 2, 3 }, new List<int> { 0, 0, 0 }),
        [ThreeElementsWithOneRepetitions] = new(new List<int> { 1, 2, 1 }, new List<int> { 0, 0, 1 }),
        [ThreeElementsWithTwoRepetitions] = new(new List<int> { 1, 1, 1 }, new List<int> { 0, 1, 2 }),
        [MultipleRepetedElements] = new(
            new List<int> { 1, 2, 3, 2, 3, 1, 3, 3, 3, 1, 4, 2 }, 
            new List<int> { 0, 0, 0, 1, 1, 1, 2, 3, 4, 2, 0, 2 }),
    };

    [DataRow(EmptyEnumerable)]
    [DataRow(EmptyArray)]
    [DataRow(SingletonList)]
    [DataRow(SingletonArray)]
    [DataRow(ThreeElementsWithoutRepetitions)]
    [DataRow(ThreeElementsWithOneRepetitions)] 
    [DataRow(ThreeElementsWithTwoRepetitions)]
    [DataRow(MultipleRepetedElements)]
    [DataTestMethod]
    public void Count_IsCorrect(TestCaseId testCaseId)
    {
        var (enumerable, expectedOutput) = TestCases[testCaseId];

        Assert.IsTrue(Counter.Count(enumerable).SequenceEqual(expectedOutput));
    }
}
