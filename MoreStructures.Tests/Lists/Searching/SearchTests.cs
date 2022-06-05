using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreLinq;
using MoreStructures.Lists.Searching;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MoreStructures.Tests.Lists.Searching;

using static SearchTests.TestCaseId;

public abstract class SearchTests
{
    protected ISearch Search { get; }

    protected SearchTests(ISearch search)
    {
        Search = search;
    }

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
    public void SearchMethods_OnListOfIntegers(TestCaseId testCaseId)
    {
        SearchMethods_OnEnumerable((List<int>)TestCases[testCaseId]);
    }

    [DataRow(ArrayOfChars1)]
    [DataTestMethod]
    public void SearchMethods_OnCharArray(TestCaseId testCaseId)
    {
        SearchMethods_OnEnumerable((char[])TestCases[testCaseId]);
    }

    [DataRow(String1)]
    [DataRow(String2)]
    [DataTestMethod]
    public void SearchMethods_OnString(TestCaseId testCaseId)
    {
        SearchMethods_OnEnumerable((string)TestCases[testCaseId]);
    }

    [DataRow(4)]
    [DataRow(5)]
    [DataTestMethod]
    public void SearchMethods_ItemtNotFound(int length)
    {
        var enumerable1 = Enumerable.Range(0, length);
        Assert.IsTrue(Search.First(enumerable1, length) < 0);
        Assert.IsTrue(Search.Last(enumerable1, length) < 0);
        Assert.IsTrue(Search.Interval(enumerable1, length) is var (x, y) && x < 0 && y < 0);
        Assert.IsTrue(Search.Nth(enumerable1, length, 0) < 0);
        Assert.IsTrue(Search.Nth(enumerable1, length - 1, 1) < 0);
    }

    [TestMethod]
    public void SearchMethods_ItemNotFound_OnStrings()
    {
        var enumerable1 = "abcde";
        Assert.IsTrue(Search.First(enumerable1, 'f') < 0);
        Assert.IsTrue(Search.Last(enumerable1, 'f') < 0);
        Assert.IsTrue(Search.Interval(enumerable1, 'f') is var (x, y) && x < 0 && y < 0);
        Assert.IsTrue(Search.Nth(enumerable1, 'f', 0) < 0);
        Assert.IsTrue(Search.Nth(enumerable1, 'e', 1) < 0);
    }

    [TestMethod]
    public void SearchMethods_OnEmptyEnumerable()
    {
        var enumerable1 = Enumerable.Empty<double>();
        Assert.IsTrue(Search.First(enumerable1, 1.1) < 0);
        Assert.IsTrue(Search.FirstAll(enumerable1).Count == 0);
        Assert.IsTrue(Search.Last(enumerable1, 2) < 0);
        Assert.IsTrue(Search.Interval(enumerable1, -1.3) is var (x, y) && x < 0 && y < 0);
        Assert.IsTrue(Search.Nth(enumerable1, 1, 0) < 0);
    }

    [TestMethod]
    public void SearchMethods_FromIndexAndToIndexNull()
    {
        var length = 5;
        var enumerable1 = Enumerable.Range(0, length);
        Assert.AreEqual(2, Search.First(enumerable1, 2, null, null, null));
        Assert.AreEqual(2, Search.First(enumerable1, 2, null, 0, null));
        Assert.AreEqual(-1, Search.First(enumerable1, 2, null, 3, null));
        Assert.AreEqual(2, Search.First(enumerable1, 2, null, null, 2));
        Assert.AreEqual(-1, Search.First(enumerable1, 2, null, null, 1));
    }

    [TestMethod]
    public void Nth_RaisesExceptionOnInvalidOccurrenceRank()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => Search.Nth(Enumerable.Empty<double>(), 1, -4));
    }

    [TestMethod]
    public void FirstAll_OnCustomIntervals()
    {
        Assert.AreEqual(2, Search.FirstAll("abcbc", null, 2, null)['c']);
        Assert.AreEqual(4, Search.FirstAll("abcbc", null, 3, null)['c']);
        Assert.IsFalse(Search.FirstAll("abcbc", null, 3, 3).ContainsKey('c'));
        Assert.IsFalse(Search.FirstAll("abcbc", null, 4, 2).ContainsKey('c'));
    }

    private void SearchMethods_OnEnumerable<T>(IEnumerable<T> enumerable)
        where T : notnull
    {
        var occurrences = new Dictionary<T, int> { };
        foreach (var item in enumerable)
        {
            // First and Last
            var first = Search.First(enumerable, item, Comparer<T>.Default, 0, enumerable.Count() - 1);
            var expectedFirst = enumerable.Index().First(e => Equals(e.Value, item)).Key;
            var last = Search.Last(enumerable, item, Comparer<T>.Default, 0, enumerable.Count() - 1);
            var expectedLast = enumerable.Index().Reverse().First(e => Equals(e.Value, item)).Key;
            Assert.AreEqual(expectedFirst, first);
            Assert.AreEqual(expectedLast, last);

            // First all
            var firstAll = Search.FirstAll(enumerable);
            Assert.IsTrue(Enumerable.ToHashSet(firstAll.Keys).SetEquals(enumerable.Distinct()));
            var enumerableWithIndexes = enumerable.Index();
            Assert.IsTrue(
                firstAll.All(
                    i => enumerableWithIndexes.First(
                        e => Equals(e.Value, enumerable.ElementAt(i.Value))).Key == i.Value));

            // Interval
            var (first1, last1) = Search.Interval(
                enumerable, item, Comparer<T>.Default, 0, enumerable.Count() - 1);
            Assert.AreEqual(expectedFirst, first1);
            Assert.AreEqual(expectedLast, last1);

            // Nth
            if (!occurrences.TryGetValue(item, out var currentOccurrence))
                currentOccurrence = -1;
            ++currentOccurrence;
            occurrences[item] = currentOccurrence;

            var expectedNth = enumerable.Index().Where(e => Equals(e.Value, item)).Skip(currentOccurrence).First().Key;
            var nth = Search.Nth(
                enumerable, item, currentOccurrence, Comparer<T>.Default, 0, enumerable.Count() - 1);
            Assert.AreEqual(expectedNth, nth);
        }
    }
}
