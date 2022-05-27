using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using MoreStructures.BurrowsWheelerTransform;
using System.Collections.Generic;

namespace MoreStructures.Tests.BurrowsWheelerTransform;

[TestClass]
public class BWMatrixTests
{
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
    public void Content_ReturnsAllRotationsOfText()
    {
        var text = new TextWithTerminator("abcabcd");
        var matrixContent = new BWMatrix(text).Content;

        Assert.AreEqual(text.Length, matrixContent.Count);
        Assert.IsTrue(matrixContent.All(
            rotation => string.Join(string.Empty, rotation.Split(text.Terminator).Reverse()) == text.Text));
    }

    [TestMethod]
    public void Content_ReturnsRotationsSorted()
    {
        var text = new TextWithTerminator("abcabcd");
        var matrixContent = new BWMatrix(text).Content;

        Assert.IsTrue(matrixContent
            .Zip(matrixContent.Skip(1))
            .All(((string rotation, string nextRotation) pair) => 
                string.Compare(pair.rotation, pair.nextRotation) <= 0));
    }

    [TestMethod]
    public void Content_AllRotationsAreDifferent()
    {
        var text = new TextWithTerminator("aaaaa");
        var matrix = new BWMatrix(text).Content;

        Assert.AreEqual(matrix.Count, matrix.Distinct().Count());
    }

    [TestMethod]
    public void Content_ReturnsAnImmutableCollection()
    {
        var text = new TextWithTerminator("mississippi");
        var matrixContent = new BWMatrix(text).Content;
        Assert.ThrowsException<NotSupportedException>(() => matrixContent.Add("a"));
    }

    [TestMethod]
    public void Content_LastFirstProperty()
    {
        var text = new TextWithTerminator("mississippi");
        var matrix = new BWMatrix(text);
        var occurrencesByChar = new Dictionary<char, int>();
        for (int i = 0; i < matrix.FirstColumn.Length; i++)
        {
            var distanceOfCharToTerminator1 = matrix.Content[i].IndexOf(text.Terminator);
            if (!occurrencesByChar.TryGetValue(matrix.FirstColumn[i], out var occurrences))
                occurrences = 0;
            var indexNthOccurrenceOfCharInLastColumn = Enumerable
                .Range(0, matrix.LastColumn.Length)
                .Where(j => matrix.LastColumn[j] == matrix.FirstColumn[i])
                .Skip(occurrences)
                .First();
            var distanceOfCharToTerminator2 = (matrix
                .Content[indexNthOccurrenceOfCharInLastColumn]
                .IndexOf(text.Terminator) + 1) % text.Length;
            Assert.AreEqual(distanceOfCharToTerminator1, distanceOfCharToTerminator2);
            occurrencesByChar[matrix.FirstColumn[i]] = ++occurrences;
        }
    }

    [TestMethod]
    public void Transform_IsCorrect()
    {
        var text = new TextWithTerminator("ab");
        var t = text.Terminator;
        Assert.AreEqual($"b{t}a", new BWMatrix(text, new string[] { $"{t}ab", $"ab{t}", $"b{t}a" }).Transform);
    }

    [TestMethod]
    public void Transform_HasSameLengthAsTextPlusTerminator()
    {
        var text = new TextWithTerminator("selflessness");
        var matrix = new BWMatrix(text);
        Assert.AreEqual(text.Length, matrix.Transform.Length);
    }

    [TestMethod]
    public void FirstColumnAndLastColumn_AreConsistent()
    {
        var text = new TextWithTerminator("selflessness");
        var matrix = new BWMatrix(text);
        Assert.AreEqual(matrix.FirstColumn, matrix.FirstColumn);
        Assert.AreEqual(matrix.LastColumn, matrix.LastColumn);
    }

    [TestMethod]
    public void FirstColumn_IsATextPermutation()
    {
        var text = new TextWithTerminator("selflessness");
        var matrix = new BWMatrix(text);
        Assert.AreEqual(text.Length, matrix.FirstColumn.Length);
        Assert.IsTrue(new HashSet<char>(text).SetEquals(matrix.FirstColumn));
    }

    [TestMethod]
    public void LastColumn_IsATextPermutation()
    {
        var text = new TextWithTerminator("selflessness");
        var matrix = new BWMatrix(text);
        Assert.AreEqual(text.Length, matrix.LastColumn.Length);
        Assert.IsTrue(new HashSet<char>(text).SetEquals(matrix.LastColumn));
    }
}
