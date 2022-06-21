using MoreStructures.BurrowsWheelerTransform;
using MoreStructures.Utilities;

namespace MoreStructures.Tests.BurrowsWheelerTransform;

[TestClass]
public class BWTransformTests
{
    [TestMethod]
    public void Length_IsTheSameOfLengthOfText()
    {
        var transform = new BWTransform(new("test"), new("ttes$"));
        Assert.AreEqual(transform.Text.Length, transform.Length);
    }

    private class MockCharComparer1 : IComparer<char>
    {
        private static readonly IDictionary<char, int> _priorities = new Dictionary<char, int>
        {
            ['s'] = 0, ['t'] = 1, ['$'] = 2, ['e'] = 3,
        };

        public int Compare(char x, char y) => _priorities[x] - _priorities[y];
    }

    private class MockCharComparer2 : IComparer<char>
    {
        private static readonly IDictionary<char, int> _priorities = new Dictionary<char, int>
        {
            ['e'] = 0,
            ['t'] = 1,
            ['$'] = 2,
            ['s'] = 3,
        };

        public int Compare(char x, char y) => _priorities[x] - _priorities[y];
    }
    [TestMethod]
    public void Quicksort_WithDefaultComparer()
    {
        Assert.AreEqual(
            string.Concat(BWTransform.QuickSort(new("test$"))),
            string.Concat(BWTransform.QuickSort(new("test$"), CharOrTerminatorComparer.Build('$'))));
    }

    [TestMethod]
    public void Quicksort_WithCustomComparer()
    {
        Assert.AreEqual("stt$e", 
            string.Concat(BWTransform.QuickSort(new("test$"), new MockCharComparer1()).sortedText.RotatedText));
        Assert.AreEqual("ett$s",
            string.Concat(BWTransform.QuickSort(new("test$"), new MockCharComparer2()).sortedText.RotatedText));
    }

    [TestMethod]
    public void Quicksort_IndexesMappingIsCorrect()
    {
        var indexesMapping = BWTransform.QuickSort(new("test$")).indexesMapping;
        Assert.IsTrue(indexesMapping.SequenceEqual(new List<int> { 4, 1, 2, 0, 3 }));
        indexesMapping = BWTransform.QuickSort(new("test$"), new MockCharComparer1()).indexesMapping;
        Assert.IsTrue(indexesMapping.SequenceEqual(new List<int> { 2, 0, 3, 4, 1 }));
        indexesMapping = BWTransform.QuickSort(new("test$"), new MockCharComparer2()).indexesMapping;
        Assert.IsTrue(indexesMapping.SequenceEqual(new List<int> { 1, 0, 3, 4, 2 }));
    }
}
