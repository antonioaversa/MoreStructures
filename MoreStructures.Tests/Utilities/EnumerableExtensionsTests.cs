using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.Utilities;
using System;
using System.Collections.Generic;

namespace MoreStructures.Tests.Utilities;

[TestClass]
public class EnumerableExtensionsTests
{
    [TestMethod]
    public void CountO1_IsCorrectWithEmptyEnumerables()
    {
        Assert.AreEqual(0, "".CountO1());
        Assert.AreEqual(0, Array.Empty<char>().CountO1());
        Assert.AreEqual(0, new List<char> { }.CountO1());
        Assert.AreEqual(0, new Dictionary<int, char> { }.CountO1());
        Assert.AreEqual(0, new HashSet<double> { }.CountO1());
    }

    [TestMethod]
    public void CountO1_IsCorrectWithSingletons()
    {
        Assert.AreEqual(1, "a".CountO1());
        Assert.AreEqual(1, new char[] { 'a' }.CountO1());
        Assert.AreEqual(1, new List<char> { 'a' }.CountO1());
        Assert.AreEqual(1, new Dictionary<int, char> { [0] = 'a' }.CountO1());
        Assert.AreEqual(1, new HashSet<double> { 'a', 'a', 'a' }.CountO1());
    }

    [TestMethod]
    public void CountO1_IsCorrectWithMultipleValues()
    {
        Assert.AreEqual(3, "abc".CountO1());
        Assert.AreEqual(3, new char[] { 'a', 'b', 'c' }.CountO1());
        Assert.AreEqual(3, new List<char> { 'a', 'b', 'c' }.CountO1());
        Assert.AreEqual(3, new Dictionary<int, char> { [0] = 'a', [1] = 'b', [2] = 'c' }.CountO1());
        Assert.AreEqual(3, new HashSet<double> { 'a', 'a', 'a', 'c', 'b', 'b', 'c' }.CountO1());
    }

    [TestMethod]
    public void CountO1_DoesntEnumerateNonGenericIListImplementers()
    {
        var list = new NonGenericListMock(4) as IEnumerable<int>;
        Assert.AreEqual(4, list.CountO1());
    }

    [TestMethod]
    public void CountO1_DoesntEnumerateGenericIListImplementers()
    {
        var list = new GenericListMock(4) as IList<int>;
        Assert.AreEqual(4, list.CountO1());
    }

    [TestMethod]
    public void CountO1_EnumeratesGenericEnumerables()
    {
        var enumerable = GetBools();
        Assert.AreEqual(3, enumerable.CountO1());
    }

    [TestMethod]
    public void ElementAtO1_IsCorrect()
    {
        Assert.AreEqual('b', "abcd".ElementAtO1(1));
        Assert.AreEqual('d', "abcd".ElementAtO1(3));
        Assert.AreEqual(1, new int[] { 1, 2, 3 }.ElementAtO1(0));
        Assert.AreEqual(3, new int[] { 1, 2, 3 }.ElementAtO1(2));
        Assert.AreEqual(1, new List<int> { 1, 2, 3 }.ElementAtO1(0));
        Assert.AreEqual(3, new List<int> { 1, 2, 3 }.ElementAtO1(2));
    }

    [TestMethod]
    public void ElementAtO1_ThrowsExceptionForInvalidIndexes()
    {
        Assert.ThrowsException<IndexOutOfRangeException>(() => "abc".ElementAtO1(-1));
        Assert.ThrowsException<IndexOutOfRangeException>(() => "abc".ElementAtO1(3));
        Assert.ThrowsException<IndexOutOfRangeException>(() => new int[] { 1, 2, 3 }.ElementAtO1(-1));
        Assert.ThrowsException<IndexOutOfRangeException>(() => new int[] { 1, 2, 3 }.ElementAtO1(3));
        Assert.ThrowsException<IndexOutOfRangeException>(() => new List<int> { 1, 2, 3 }.ElementAtO1(-1));
        Assert.ThrowsException<IndexOutOfRangeException>(() => new List<int> { 1, 2, 3 }.ElementAtO1(3));
        Assert.ThrowsException<IndexOutOfRangeException>(() => new GenericListMock(3).ElementAtO1(-1));
        Assert.ThrowsException<IndexOutOfRangeException>(() => new GenericListMock(3).ElementAtO1(3));
        Assert.ThrowsException<IndexOutOfRangeException>(() => new NonGenericListMock(3).ElementAtO1(-1));
        Assert.ThrowsException<IndexOutOfRangeException>(() => new NonGenericListMock(3).ElementAtO1(3));
    }

    [TestMethod]
    public void ElementAtO1_DoesntEnumerateNonGenericIListImplementers()
    {
        var nonGenericList = new NonGenericListMock(57) { ElementReturned = 123 };
        Assert.AreEqual(123, nonGenericList.ElementAtO1(45));
        nonGenericList.ElementReturned = 456;
        Assert.AreEqual(456, nonGenericList.ElementAtO1(56));
    }

    [TestMethod]
    public void ElementAtO1_DoesntEnumerateGenericIListImplementers()
    {
        var genericList = new GenericListMock(57) { ElementReturned = 123 };
        Assert.AreEqual(123, genericList.ElementAtO1(45));
        genericList.ElementReturned = 456;
        Assert.AreEqual(456, genericList.ElementAtO1(56));
    }

    [TestMethod]
    public void ElementAtO1_EnumeratesGenericEnumerables()
    {
        Assert.AreEqual(true, GetBools().ElementAtO1(0));
        Assert.AreEqual(false, GetBools().ElementAtO1(1));
        Assert.AreEqual(true, GetBools().ElementAtO1(2));
    }

    [TestMethod]
    public void ElementAtO1OrDefault_IsCorrect()
    {
        Assert.AreEqual(default, "abcd".ElementAtO1OrDefault(10));
        Assert.AreEqual('b', "abcd".ElementAtO1OrDefault(1));
        Assert.AreEqual('d', "abcd".ElementAtO1OrDefault(3));
        Assert.AreEqual(1, new int[] { 1, 2, 3 }.ElementAtO1OrDefault(0));
        Assert.AreEqual(3, new int[] { 1, 2, 3 }.ElementAtO1OrDefault(2));
        Assert.AreEqual(default, new int[] { 1, 2, 3 }.ElementAtO1OrDefault(10));
        Assert.AreEqual(1, new List<int> { 1, 2, 3 }.ElementAtO1OrDefault(0));
        Assert.AreEqual(3, new List<int> { 1, 2, 3 }.ElementAtO1OrDefault(2));
        Assert.AreEqual(default, new List<int> { 1, 2, 3 }.ElementAtO1OrDefault(10));
    }

    [TestMethod]
    public void ElementAtO1OrDefault_ThrowsExceptionForNegativeIndexes()
    {
        Assert.ThrowsException<IndexOutOfRangeException>(() => "abc".ElementAtO1OrDefault(-1));
        Assert.ThrowsException<IndexOutOfRangeException>(() => new int[] { 1, 2, 3 }.ElementAtO1OrDefault(-1));
        Assert.ThrowsException<IndexOutOfRangeException>(() => new List<int> { 1, 2, 3 }.ElementAtO1OrDefault(-1));
        Assert.ThrowsException<IndexOutOfRangeException>(() => new GenericListMock(3).ElementAtO1OrDefault(-1));
        Assert.ThrowsException<IndexOutOfRangeException>(() => new NonGenericListMock(3).ElementAtO1OrDefault(-1));
    }

    [TestMethod]
    public void ElementAtO1OrDefault_ReturnsDefaultForBigIndexes()
    {
        Assert.AreEqual(default, "abc".ElementAtO1OrDefault(3));
        Assert.AreEqual(default, new int[] { 1, 2, 3 }.ElementAtO1OrDefault(3));
        Assert.AreEqual(default, new List<int> { 1, 2, 3 }.ElementAtO1OrDefault(3));
        Assert.AreEqual(default, new GenericListMock(3).ElementAtO1OrDefault(3));
        Assert.AreEqual(default, new NonGenericListMock(3).ElementAtO1OrDefault(3));
    }

    [TestMethod]
    public void ElementAtO1OrDefault_DoesntEnumerateNonGenericIListImplementers()
    {
        var nonGenericList = new NonGenericListMock(57) { ElementReturned = 123 };
        Assert.AreEqual(123, nonGenericList.ElementAtO1OrDefault(45));
        nonGenericList.ElementReturned = 456;
        Assert.AreEqual(456, nonGenericList.ElementAtO1OrDefault(56));
    }

    [TestMethod]
    public void ElementAtO1OrDefault_DoesntEnumerateGenericIListImplementers()
    {
        var genericList = new GenericListMock(57) { ElementReturned = 123 };
        Assert.AreEqual(123, genericList.ElementAtO1OrDefault(45));
        genericList.ElementReturned = 456;
        Assert.AreEqual(456, genericList.ElementAtO1OrDefault(56));
    }

    [TestMethod]
    public void ElementAtO1OrDefault_EnumeratesGenericEnumerables()
    {
        Assert.AreEqual(true, GetBools().ElementAtO1OrDefault(0));
        Assert.AreEqual(false, GetBools().ElementAtO1OrDefault(1));
        Assert.AreEqual(true, GetBools().ElementAtO1OrDefault(2));
    }

    private static IEnumerable<bool> GetBools()
    {
        yield return true;
        yield return false;
        yield return true;
    }
}
