using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.BurrowsWheelerTransform;
using MoreStructures.BurrowsWheelerTransform.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Builder;

public abstract class BuilderTests
{
    private IBuilder Builder { get; }

    public BuilderTests(IBuilder builder)
    {
        Builder = builder;
    }

    [TestMethod]
    public void BuildMatrix_ReturnsAllRotationsOfText()
    {
        var text = new TextWithTerminator("abcabcd");
        var matrixContent = Builder.BuildMatrix(text).Content;

        Assert.AreEqual(text.Length, matrixContent.Count);
        Assert.IsTrue(matrixContent.All(
            rotation => string.Join(string.Empty, rotation.Split(text.Terminator).Reverse()) == text.Text));
    }

    [TestMethod]
    public void BuildMatrix_ReturnsRotationsSorted()
    {
        var text = new TextWithTerminator("abcabcd");
        var matrixContent = Builder.BuildMatrix(text).Content;

        Assert.IsTrue(matrixContent
            .Zip(matrixContent.Skip(1))
            .All(((string rotation, string nextRotation) pair) =>
                string.Compare(pair.rotation, pair.nextRotation) <= 0));
    }

    [TestMethod]
    public void BuildMatrix_AllRotationsAreDifferent()
    {
        var text = new TextWithTerminator("aaaaa");
        var matrix = Builder.BuildMatrix(text).Content;

        Assert.AreEqual(matrix.Count, matrix.Distinct().Count());
    }

    [TestMethod]
    public void BuildMatrix_LastFirstProperty()
    {
        var text = new TextWithTerminator("mississippi");
        var matrix = Builder.BuildMatrix(text);
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
    public void BuildTransform_WithText_IsCorrect()
    {
        var text1 = new TextWithTerminator("mississippi");
        Assert.AreEqual(new BWTransform(text1, "ipssm$pissii"), Builder.BuildTransform(text1));

        var text2 = new TextWithTerminator("Burrows-Wheeler Transform");
        Assert.AreEqual(new BWTransform(text2, "mrs$ -rhelsWerafreToruwnBo"), Builder.BuildTransform(text2));
    }

    [TestMethod]
    public void BuildTransform_WithMatrix_IsCorrect()
    {
        var text = new TextWithTerminator("banana");
        var matrix = new BWMatrix(text, new string[]
        {
            "$banana",
            "a$banan",
            "ana$ban",
            "anana$b",
            "banana$",
            "na$bana",
            "nana$ba",
        });
        Assert.AreEqual(new BWTransform(text, "annb$aa"), Builder.BuildTransform(matrix));
    }
}
