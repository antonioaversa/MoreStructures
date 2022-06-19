using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

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

    private static void AssertElementAtO1<T>(T expected, IEnumerable<T> enumerable, int index, bool reverse = true)
    {
        Assert.AreEqual(expected, enumerable.ElementAtO1(index));
        Assert.AreEqual(expected, enumerable.ElementAtO1(new Index(index)));
        if (reverse)
            Assert.AreEqual(expected, enumerable.Reverse().ElementAtO1(new Index(index + 1, true)));
    }

    [TestMethod]
    public void ElementAtO1_IsCorrect()
    {
        AssertElementAtO1('b', "abcd", 1);
        AssertElementAtO1('d', "abcd", 3);
        AssertElementAtO1(1, new int[] { 1, 2, 3 }, 0);
        AssertElementAtO1(3, new int[] { 1, 2, 3 }, 2);
        AssertElementAtO1(1, new List<int> { 1, 2, 3 }, 0);
        AssertElementAtO1(3, new List<int> { 1, 2, 3 }, 2);
    }

    [TestMethod]
    public void ElementAtO1_ThrowsExceptionForInvalidIndexes()
    {
        static void AssertException<T>(IEnumerable<T> enumerable, int index)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => enumerable.ElementAtO1(index));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => enumerable.ElementAtO1(new Index(index)));
        }

        AssertException("abc", -1);
        AssertException("abc", 3);
        AssertException(new int[] { 1, 2, 3 }, -1);
        AssertException(new int[] { 1, 2, 3 }, 3);
        AssertException(new List<int> { 1, 2, 3 }, -1);
        AssertException(new List<int> { 1, 2, 3 }, 3);
        AssertException(new GenericListMock(3), -1);
        AssertException(new GenericListMock(3), 3);
        AssertException(new NonGenericListMock(3), -1);
        AssertException(new NonGenericListMock(3), 3);

        static void AssertExceptionWithRanges<T>(IEnumerable<T> enumerable)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => enumerable.ElementAtO1(^0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => enumerable.ElementAtO1(^-1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => enumerable.ElementAtO1(^4));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => enumerable.ElementAtO1(^10));
        }

        AssertExceptionWithRanges("abc");
        AssertExceptionWithRanges("abc");
        AssertExceptionWithRanges("abc");
        AssertExceptionWithRanges(new int[] { 1, 2, 3 });
        AssertExceptionWithRanges(new int[] { 1, 2, 3 });
        AssertExceptionWithRanges(new int[] { 1, 2, 3 });
        AssertExceptionWithRanges(new List<int> { 1, 2, 3 });
        AssertExceptionWithRanges(new List<int> { 1, 2, 3 });
        AssertExceptionWithRanges(new GenericListMock(3));
        AssertExceptionWithRanges(new GenericListMock(3));
        AssertExceptionWithRanges(new NonGenericListMock(3));
    }

    [TestMethod]
    public void ElementAtO1_DoesntEnumerateNonGenericIListImplementers()
    {
        var nonGenericList = new NonGenericListMock(57) { ElementReturned = 123 };
        AssertElementAtO1(123, nonGenericList, 45, false);
        nonGenericList.ElementReturned = 456;
        AssertElementAtO1(456, nonGenericList, 56, false);
    }

    [TestMethod]
    public void ElementAtO1_DoesntEnumerateGenericIListImplementers()
    {
        var genericList = new GenericListMock(57) { ElementReturned = 123 };
        AssertElementAtO1(123, genericList, 45, false);
        genericList.ElementReturned = 456;
        AssertElementAtO1(456, genericList, 56, false);
    }

    [TestMethod]
    public void ElementAtO1_EnumeratesGenericEnumerables()
    {
        AssertElementAtO1(true, GetBools(), 0);
        AssertElementAtO1(false, GetBools(), 1);
        AssertElementAtO1(true, GetBools(), 2);
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
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => "abc".ElementAtO1OrDefault(-1));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new int[] { 1, 2, 3 }.ElementAtO1OrDefault(-1));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new List<int> { 1, 2, 3 }.ElementAtO1OrDefault(-1));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new GenericListMock(3).ElementAtO1OrDefault(-1));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new NonGenericListMock(3).ElementAtO1OrDefault(-1));
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
