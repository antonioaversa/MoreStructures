using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using MoreStructures.BurrowsWheelerTransform;

namespace MoreStructures.Tests.BurrowsWheelerTransform;

[TestClass]
public class BWTCyclicRotationsTests
{
    [TestMethod]
    public void CalculateCyclicRotations_ReturnsAllRotationsOfText()
    {
        var text = new TextWithTerminator("abcabcd");
        var rotations = BWTCyclicRotations.CalculateCyclicRotations(text);

        Assert.AreEqual(text.Length, rotations.Count);
        Assert.IsTrue(rotations.All(
            rotation => string.Join(string.Empty, rotation.Split(text.Terminator).Reverse()) == text.Text));
    }

    [TestMethod]
    public void CalculateCyclicRotations_ReturnsRotationsSorted()
    {
        var text = new TextWithTerminator("abcabcd");
        var rotations = BWTCyclicRotations.CalculateCyclicRotations(text);

        Assert.IsTrue(rotations
            .Zip(rotations.Skip(1))
            .All(((string rotation, string nextRotation) pair) => 
                string.Compare(pair.rotation, pair.nextRotation) <= 0));
    }

    [TestMethod]
    public void CalculateCyclicRotations_AllRotationsAreDifferent()
    {
        var text = new TextWithTerminator("aaaaa");
        var rotations = BWTCyclicRotations.CalculateCyclicRotations(text);

        Assert.AreEqual(rotations.Count, rotations.Distinct().Count());
    }
}
