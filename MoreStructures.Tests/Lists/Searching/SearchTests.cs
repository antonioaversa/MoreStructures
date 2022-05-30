using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreLinq;
using MoreStructures.Lists.Searching;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MoreStructures.Tests.Lists.Searching;

using static SearchTests.TestCaseId;

[TestClass]
public class SearchTests
{
    public enum TestCaseId 
    { 
        ListOfIntegers1,
        ListOfIntegers2,
        ArrayOfChars1,
        String1,
        String2,
    }

    private readonly Dictionary<TestCaseId, IEnumerable> TestCases = 
        new ()
        {
            [ListOfIntegers1] = new List<int> { 0, 0, 2, 3, 3, 3, 4, 4, 7, 8, 9, 10, 10, 10, 10, 11 },
            [ListOfIntegers2] = new List<int> { },
            [ArrayOfChars1] = "abbcccdeffgggghijkkkl".ToCharArray(),
            [String1] = "abbcccdeffgggghijkkkl",
            [String2] = "",
        };

    [DataRow(ListOfIntegers1)]
    [DataRow(ListOfIntegers2)]
    [DataTestMethod]
    public void BinarySearchMethods_OnListOfIntegers(TestCaseId testCaseId)
    {
        BinarySearchMethods_OnEnumerable((List<int>)TestCases[testCaseId]);
    }

    [DataRow(ArrayOfChars1)]
    [DataTestMethod]
    public void BinarySearchMethods_OnCharArray(TestCaseId testCaseId)
    {
        BinarySearchMethods_OnEnumerable((char[])TestCases[testCaseId]);
    }

    [DataRow(String1)]
    [DataRow(String2)]
    [DataTestMethod]
    public void BinarySearchMethods_OnString(TestCaseId testCaseId)
    {
        BinarySearchMethods_OnEnumerable((string)TestCases[testCaseId]);
    }

    [DataRow(4)]
    [DataRow(5)]
    [DataTestMethod]
    public void BinarySearchMethods_ElementNotFound(int length)
    {
        var enumerable1 = Enumerable.Range(0, length);
        Assert.IsTrue(Search.BinarySearchFirst(enumerable1, length) < 0);
        Assert.IsTrue(Search.BinarySearchLast(enumerable1, length) < 0);
        Assert.IsTrue(Search.BinarySearchInterval(enumerable1, length) is var (x, y) && x < 0 && y < 0);
        Assert.IsTrue(Search.BinarySearchNth(enumerable1, length, 0) < 0);
        Assert.IsTrue(Search.BinarySearchNth(enumerable1, length - 1, 1) < 0);
    }

    [TestMethod]
    public void BinarySearchMethods_ElementNotFound_OnStrings()
    {
        var enumerable1 = "abcde";
        Assert.IsTrue(Search.BinarySearchFirst(enumerable1, 'f') < 0);
        Assert.IsTrue(Search.BinarySearchLast(enumerable1, 'f') < 0);
        Assert.IsTrue(Search.BinarySearchInterval(enumerable1, 'f') is var (x, y) && x < 0 && y < 0);
        Assert.IsTrue(Search.BinarySearchNth(enumerable1, 'f', 0) < 0);
        Assert.IsTrue(Search.BinarySearchNth(enumerable1, 'e', 1) < 0);
    }

    [TestMethod]
    public void BinarySearchMethods_OnEmptyEnumerable()
    {
        var enumerable1 = Enumerable.Empty<double>();
        Assert.IsTrue(Search.BinarySearchFirst(enumerable1, 1.1) < 0);
        Assert.IsTrue(Search.BinarySearchLast(enumerable1, 2) < 0);
        Assert.IsTrue(Search.BinarySearchInterval(enumerable1, -1.3) is var (x, y) && x < 0 && y < 0);
        Assert.IsTrue(Search.BinarySearchNth(enumerable1, 1, 0) < 0);
    }

    [TestMethod]
    public void BinarySearchNth_RaisesExceptionOnInvalidOccurrenceRank()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => Search.BinarySearchNth(Enumerable.Empty<double>(), 1, -4));
    }

    private static void BinarySearchMethods_OnEnumerable<T>(IEnumerable<T> enumerable)
        where T : notnull
    {
        var occurrences = new Dictionary<T, int> { };
        foreach (var element in enumerable)
        {
            var first = Search.BinarySearchFirst(enumerable, element, Comparer<T>.Default, 0, enumerable.Count() - 1);
            var expectedFirst = enumerable.Index().First(e => Equals(e.Value, element)).Key;
            var last = Search.BinarySearchLast(enumerable, element, Comparer<T>.Default, 0, enumerable.Count() - 1);
            var expectedLast = enumerable.Index().Reverse().First(e => Equals(e.Value, element)).Key;
            Assert.AreEqual(expectedFirst, first);
            Assert.AreEqual(expectedLast, last);

            var (first1, last1) = Search.BinarySearchInterval(
                enumerable, element, Comparer<T>.Default, 0, enumerable.Count() - 1);
            Assert.AreEqual(expectedFirst, first1);
            Assert.AreEqual(expectedLast, last1);

            if (!occurrences.TryGetValue(element, out var currentOccurrence))
                currentOccurrence = -1;
            ++currentOccurrence;
            occurrences[element] = currentOccurrence;

            var expectedNth = enumerable.Index().Where(e => Equals(e.Value, element)).Skip(currentOccurrence).First().Key;
            var nth = Search.BinarySearchNth(
                    enumerable, element, currentOccurrence, Comparer<T>.Default, 0, enumerable.Count() - 1);
            Assert.AreEqual(expectedNth, nth);
        }
    }
}
