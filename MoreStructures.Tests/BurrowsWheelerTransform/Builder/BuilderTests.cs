using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.BurrowsWheelerTransform;
using MoreStructures.BurrowsWheelerTransform.Builders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Builder;

using static BuilderTests.TestCase;

public abstract class BuilderTests
{
    public enum TestCase { Banana, Allele, Mississippi, BurrowsWheelerTransform, PanamaBananas }

    private readonly Dictionary<TestCase, (BWTransform, BWMatrix)> TestCases = new()
    {
        [Banana] = (
            new(new("banana"), "annb$aa"),
            new(new("banana"), new string[]
            {
                "$banana",
                "a$banan",
                "ana$ban",
                "anana$b",
                "banana$",
                "na$bana",
                "nana$ba",
            })),

        [Allele] = (
            new(new("allele"), "e$llela"),
            new(new("allele"), new string[]
            {
                "$allele",
                "allele$",
                "e$allel",
                "ele$all",
                "le$alle",
                "lele$al",
                "llele$a",
            })),

        [Mississippi] = (
            new(new("mississippi"), "ipssm$pissii"),
            new(new("mississippi"), new string[]
            {
                "$mississippi",
                "i$mississipp",
                "ippi$mississ",
                "issippi$miss",
                "ississippi$m",
                "mississippi$",
                "pi$mississip",
                "ppi$mississi",
                "sippi$missis",
                "sissippi$mis",
                "ssippi$missi",
                "ssissippi$mi",
            })),

        [BurrowsWheelerTransform] = (
            new(new("Burrows-Wheeler Transform"), "mrs$ -rhelsWerafreToruwnBo"),
            new(new("Burrows-Wheeler Transform"), new string[]
            {
                "$Burrows-Wheeler Transform",
                " Transform$Burrows-Wheeler",
                "-Wheeler Transform$Burrows",
                "Burrows-Wheeler Transform$",
                "Transform$Burrows-Wheeler ",
                "Wheeler Transform$Burrows-",
                "ansform$Burrows-Wheeler Tr",
                "eeler Transform$Burrows-Wh",
                "eler Transform$Burrows-Whe",
                "er Transform$Burrows-Wheel",
                "form$Burrows-Wheeler Trans",
                "heeler Transform$Burrows-W",
                "ler Transform$Burrows-Whee",
                "m$Burrows-Wheeler Transfor",
                "nsform$Burrows-Wheeler Tra",
                "orm$Burrows-Wheeler Transf",
                "ows-Wheeler Transform$Burr",
                "r Transform$Burrows-Wheele",
                "ransform$Burrows-Wheeler T",
                "rm$Burrows-Wheeler Transfo",
                "rows-Wheeler Transform$Bur",
                "rrows-Wheeler Transform$Bu",
                "s-Wheeler Transform$Burrow",
                "sform$Burrows-Wheeler Tran",
                "urrows-Wheeler Transform$B",
                "ws-Wheeler Transform$Burro",
            })),

        [PanamaBananas] = (
            new(new("panamabananas"), "smnpbnnaaaaa$a"),
            new(new("panamabananas"), new string[]
            {
                "$panamabananas",
                "abananas$panam",
                "amabananas$pan",
                "anamabananas$p",
                "ananas$panamab",
                "anas$panamaban",
                "as$panamabanan",
                "bananas$panama",
                "mabananas$pana",
                "namabananas$pa",
                "nanas$panamaba",
                "nas$panamabana",
                "panamabananas$",
                "s$panamabanana",
            })),
    };

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

    [DataRow(Banana)]
    [DataRow(Allele)]
    [DataRow(Mississippi)]
    [DataRow(BurrowsWheelerTransform)]
    [DataRow(PanamaBananas)]
    [DataTestMethod]
    public void BuildTransform_WithText_IsCorrect(TestCase testCase)
    {
        var (transform, _) = TestCases[testCase];
        Assert.AreEqual(transform, Builder.BuildTransform(transform.Text));
    }

    [DataRow(Banana)]
    [DataRow(Allele)]
    [DataRow(Mississippi)]
    [DataRow(BurrowsWheelerTransform)]
    [DataRow(PanamaBananas)]
    [DataTestMethod]
    public void BuildTransform_WithMatrix_IsCorrect(TestCase testCase)
    {
        var (transform, matrix) = TestCases[testCase];
        Assert.AreEqual(transform, Builder.BuildTransform(matrix));
    }

    [DataRow(Banana)]
    [DataRow(Allele)]
    [DataRow(Mississippi)]
    [DataRow(BurrowsWheelerTransform)]
    [DataRow(PanamaBananas)]
    [DataTestMethod]
    public void InvertMatrix_IsCorrect(TestCase testCase)
    {
        var (_, matrix) = TestCases[testCase];
        Assert.AreEqual(matrix.Text, Builder.InvertMatrix(matrix));
    }

    [DataRow(Banana)]
    [DataRow(Allele)]
    [DataRow(Mississippi)]
    [DataRow(BurrowsWheelerTransform)]
    [DataRow(PanamaBananas)]
    [DataTestMethod]
    public void InvertTransform_IsCorrect(TestCase testCase)
    {
        var (transform, _) = TestCases[testCase];
        Assert.AreEqual(transform.Text, Builder.InvertTransform(new(transform.Content, transform.Text.Terminator)));
    }
}
