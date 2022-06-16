using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MoreStructures.Tests.Utilities;

[TestClass]
public class ValueEnumerableTests
{
    [TestMethod]
    public void Ctor_DoesntEnumerateUnderlyingEnumerable()
    {
        var callsToEEnumerator = 0;
        var e = Enumerable.Range(0, int.MaxValue).Select(x => callsToEEnumerator++);
        var ve = new ValueEnumerable<int>(e);

        Assert.AreEqual(0, callsToEEnumerator);
    }

    [TestMethod]
    public void Equals_ByValueOnItemsOfUnderlyingEnumerable()
    {
        var ve1 = new ValueEnumerable<char>("abc".ToList());
        var ve2 = new ValueEnumerable<char>("abc".ToList());
        Assert.AreEqual(ve1, ve2);

        var ve3 = new ValueEnumerable<char>("abd".ToList());
        Assert.AreNotEqual(ve1, ve3);

        Assert.IsFalse(ve1.Equals(null));
    }

    [TestMethod]
    public void GetHashCode_ByValueOnItemsOfUnderlyingEnumerable()
    {
        var ve1 = new ValueEnumerable<char>("abc".ToList());
        var ve2 = new ValueEnumerable<char>("abc".ToList());
        Assert.AreEqual(ve1.GetHashCode(), ve2.GetHashCode());
    }

    [TestMethod]
    public void ToString_IncludesUnderlyingEnumerableToString()
    {
        var e1 = "abc";
        var ve1 = new ValueEnumerable<char>(e1);
        Assert.IsTrue(ve1.ToString().Contains(e1));
    }

    [TestMethod]
    public void GetEnumerator_WithGeneric_GivesItemsOfUnderlyingEnumerable()
    {
        var e1 = new List<(int, bool)>() { (0, true), (1, false), (2, false) };
        var ve1 = new ValueEnumerable<(int, bool)>(e1);
        Assert.IsTrue(ve1.SequenceEqual(e1));
    }

    [TestMethod]
    public void GetEnumerator_WithoutGeneric_GivesItemsOfUnderlyingEnumerable()
    {
        var e1 = new List<(int, bool)>() { (0, true), (1, false), (2, false) };
        var ve1 = new ValueEnumerable<(int, bool)>(e1);

        var e1Enumerator = (e1 as IEnumerable).GetEnumerator();
        var ve1Enumerator = (ve1 as IEnumerable).GetEnumerator();
        while (e1Enumerator.MoveNext())
        {
            Assert.IsTrue(ve1Enumerator.MoveNext());
            Assert.AreEqual(e1Enumerator.Current, ve1Enumerator.Current);
        }
        Assert.IsFalse(ve1Enumerator.MoveNext());
    }
}
