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
    public void Equals_ByValue()
    {
        var text1 = new TextWithTerminator("ac");
        var t1 = text1.Terminator;
        var matrixContent1 = new string[] { $"{t1}test", $"est{t1}t", $"st{t1}te", $"t{t1}tes", $"test{t1}" };
        var matrix1 = new BWMatrix(text1, matrixContent1);

        var text2 = new TextWithTerminator("ac");
        var t2 = text1.Terminator;
        var matrixContent2 = new string[] { $"{t2}test", $"est{t2}t", $"st{t2}te", $"t{t2}tes", $"test{t2}" };
        var matrix2 = new BWMatrix(text2, matrixContent2);

        Assert.AreEqual(matrix1, matrix2);
        Assert.IsTrue(matrix1.Equals(matrix2));
        Assert.IsTrue(matrix2.Equals(matrix1));
        Assert.IsTrue(matrix1 == matrix2);

        var text3 = new TextWithTerminator("ac");
        var t3 = text1.Terminator;
        var matrixContent3 = new string[] { $"{t3}txst", $"est{t3}t", $"st{t3}te", $"t{t3}tes", $"test{t3}" };
        var matrix3 = new BWMatrix(text3, matrixContent3);

        Assert.AreNotEqual(matrix1, matrix3);
        Assert.IsFalse(matrix1.Equals(matrix3));
        Assert.IsFalse(matrix3.Equals(matrix1));
        Assert.IsFalse(matrix1 == matrix3);
    }

    [TestMethod]
    public void GetHashCode_ByValue()
    {
        var text1 = new TextWithTerminator("ac");
        var t1 = text1.Terminator;
        var matrixContent1 = new string[] { $"{t1}test", $"est{t1}t", $"st{t1}te", $"t{t1}tes", $"test{t1}" };
        var matrix1 = new BWMatrix(text1, matrixContent1);

        var text2 = new TextWithTerminator("ac");
        var t2 = text1.Terminator;
        var matrixContent2 = new string[] { $"{t2}test", $"est{t2}t", $"st{t2}te", $"t{t2}tes", $"test{t2}" };
        var matrix2 = new BWMatrix(text2, matrixContent2);

        Assert.AreEqual(matrix1.GetHashCode(), matrix2.GetHashCode());
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
