using MoreStructures.Lists.Counting;

namespace MoreStructures.Tests.Lists.Counting;

using static MoreStructures.Tests.Lists.Counting.OccurrencesCounterTests.TestCaseId;

using IntList = List<int>;
using DIntItemCounts = Dictionary<int, int>;
using IntCounts = IDictionary<int, IDictionary<int, int>>;
using DIntCounts = Dictionary<int, IDictionary<int, int>>;

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
    private record TestCase(IEnumerable<int> Enumerable, DIntCounts ExpectedOutput);

    private static readonly Dictionary<TestCaseId, TestCase> TestCases = new()
    {
        [EmptyEnumerable] = new(
            Enumerable.Empty<int>(), 
            new DIntCounts { }),

        [EmptyArray] = new(
            Array.Empty<int>(), 
            new DIntCounts { }),

        [SingletonList] = new(
            new IntList { 3 }, 
            new DIntCounts
            { 
                [3] = new DIntItemCounts { [0] = 1 }
            }),
        [SingletonArray] = new(
            new int[] { 4 }, 
            new DIntCounts
            { 
                [4] = new DIntItemCounts { [0] = 1 } 
            }),
        [ThreeElementsWithoutRepetitions] = new(
            new IntList { 1, 2, 3 }, 
            new DIntCounts
            { 
                [1] = new DIntItemCounts { [0] = 1 } 
            }),
        [ThreeElementsWithOneRepetitions] = new(
            new IntList { 1, 2, 1 }, 
            new DIntCounts
            { 
                [1] = new DIntItemCounts { [0] = 1, [1] = 1, [2] = 2 }, 
                [2] = new DIntItemCounts { [0] = 0, [1] = 1, [2] = 1 }, 
            }),
        [ThreeElementsWithTwoRepetitions] = new(
            new IntList { 1, 1, 1 }, 
            new DIntCounts
            {
                [1] = new DIntItemCounts { [0] = 1, [1] = 2, [2] = 3 },
            }),
        [MultipleRepetedElements] = new(
            new IntList { 1, 2, 3, 2, 3, 1, 3, 3, 3, 1, 4 }, 
            new DIntCounts
            {
                [1] = new DIntItemCounts
                { 
                    [0] = 1, [1] = 1, [2] = 1, [3] = 1, [4] = 1, [5] = 2, [6] = 2, [7] = 2, [8] = 2, [9] = 3, [10] = 3,
                },
                [2] = new DIntItemCounts
                { 
                    [0] = 0, [1] = 1, [2] = 1, [3] = 2, [4] = 2, [5] = 2, [6] = 2, [7] = 2, [8] = 2, [9] = 2, [10] = 2,
                },
                [3] = new DIntItemCounts
                { 
                    [0] = 0, [1] = 0, [2] = 1, [3] = 1, [4] = 2, [5] = 2, [6] = 3, [7] = 4, [8] = 5, [9] = 5, [10] = 5,
                },
                [4] = new DIntItemCounts
                { 
                    [0] = 0, [1] = 0, [2] = 0, [3] = 0, [4] = 0, [5] = 0, [6] = 0, [7] = 0, [8] = 0, [9] = 0, [10] = 1,
                },
            }),
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

        Assert.IsTrue(AreEqual(expectedOutput, Counter.Count(enumerable)));
    }

    private static bool AreEqual(IntCounts counts1, IntCounts counts2) =>
        counts1.All(kvp => 
            counts2.ContainsKey(kvp.Key) && 
            kvp.Value.All(kvp2 => 
                counts2[kvp.Key].ContainsKey(kvp2.Key) && 
                kvp2.Value == counts2[kvp.Key][kvp2.Key]));
}
