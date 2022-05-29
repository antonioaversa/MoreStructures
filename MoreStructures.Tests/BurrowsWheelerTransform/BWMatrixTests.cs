using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using MoreStructures.BurrowsWheelerTransform;
using System.Collections.Generic;
using MoreStructures.BurrowsWheelerTransform.Builders;

namespace MoreStructures.Tests.BurrowsWheelerTransform;

[TestClass]
public class BWMatrixTests
{
    private IBuilder Builder { get; } = new NaiveBuilder();

    [TestMethod]
    public void Ctor_AssignsContentSpecified()
    {
        var text = new TextWithTerminator("ac");
        var t = text.Terminator;
        var incorrectMatrixContent = new string[] { $"{t}ab", $"ab{t}", $"b{t}a" };
        var matrix = new BWMatrix(text, incorrectMatrixContent);
        Assert.IsTrue(matrix.Content.SequenceEqual(incorrectMatrixContent));
    }

    [TestMethod]
    public void Content_ReturnsAnImmutableCollection()
    { 
        var text = new TextWithTerminator("ac");
        var t = text.Terminator;
        var incorrectMatrixContent = new string[] { $"{t}ab", $"ab{t}", $"b{t}a" };
        var matrix = new BWMatrix(text, incorrectMatrixContent);
        var matrixContent = matrix.Content;
        Assert.ThrowsException<NotSupportedException>(() => matrixContent.Add("a"));
    }

    [TestMethod]
    public void Transform_IsCorrect()
    {
        var text = new TextWithTerminator("ab");
        var t = text.Terminator;
        Assert.AreEqual(
            new BWTransform(text, new($"b{t}a")), 
            new BWMatrix(text, new string[] { $"{t}ab", $"ab{t}", $"b{t}a" }).Transform);
    }

    [TestMethod]
    public void Transform_HasSameLengthAsTextPlusTerminator()
    {
        var text = new TextWithTerminator("selflessness");
        var matrix = Builder.BuildMatrix(text);
        Assert.AreEqual(text.Length, matrix.Transform.Length);
    }

    [TestMethod]
    public void FirstColumnAndLastColumn_AreConsistent()
    {
        var text = new TextWithTerminator("selflessness");
        var matrix = Builder.BuildMatrix(text);
        Assert.AreEqual(matrix.FirstColumn, matrix.FirstColumn);
        Assert.AreEqual(matrix.LastColumn, matrix.LastColumn);
    }

    [TestMethod]
    public void FirstColumn_IsATextPermutation()
    {
        var text = new TextWithTerminator("selflessness");
        var matrix = Builder.BuildMatrix(text);
        Assert.AreEqual(text.Length, matrix.FirstColumn.Length);
        Assert.IsTrue(new HashSet<char>(text).SetEquals(matrix.FirstColumn));
    }

    [TestMethod]
    public void LastColumn_IsATextPermutation()
    {
        var text = new TextWithTerminator("selflessness");
        var matrix = Builder.BuildMatrix(text);
        Assert.AreEqual(text.Length, matrix.LastColumn.Length);
        Assert.IsTrue(new HashSet<char>(text).SetEquals(matrix.LastColumn));
    }
}
