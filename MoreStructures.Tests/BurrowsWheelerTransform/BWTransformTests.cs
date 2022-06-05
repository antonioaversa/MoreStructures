using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.BurrowsWheelerTransform;
using MoreStructures.Utilities;
using System.Collections.Generic;

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
            string.Concat(BWTransform.QuickSort(new("test$"), new MockCharComparer1()).RotatedText));
        Assert.AreEqual("ett$s",
            string.Concat(BWTransform.QuickSort(new("test$"), new MockCharComparer2()).RotatedText));
    }
}
