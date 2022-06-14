using MoreStructures.RecImmTrees;
using MoreStructures.SuffixTrees;
using MoreStructures.SuffixTries;

namespace MoreStructures.SuffixStructures.Matching;

/// <summary>
/// Base class for all <see cref="ISnssFinder"/> concretions which implement 
/// <see cref="ISnssFinder.Find(IEnumerable{char}, IEnumerable{char})"/> using a suffix structure
/// (a concretions of <see cref="ISuffixStructureNode{TEdge, TNode}"/>, implementing 
/// <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/>), such as <see cref="SuffixTreeNode"/> or
/// <see cref="SuffixTrieNode"/>).
/// </summary>
public abstract class SuffixStructureBasedSnssFinder : ISnssFinder
{
    /// <summary>
    /// A special char to be used as end delimiter for the first text, used for building the suffix structure. 
    /// Should not occur in first text, nor in the second, since a single suffix structure is built, embedding both
    /// texts (a.k.a. generalized suffix structure).
    /// </summary>
    public char Terminator1 { get; }

    /// <summary>
    /// A special char to be used as end delimiter for the second text, used for building the suffix structure. 
    /// Should not occur in first text, nor in the second, since a single suffix structure is built, embedding both
    /// texts (a.k.a. generalized suffix structure).
    /// </summary>
    public char Terminator2 { get; }

    /// <summary>
    /// Whether the two sequences of chars (first and second text) should be evaluated, in order to
    /// make sure that are valid, i.e. they don't contain <see cref="Terminator1"/> nor <see cref="Terminator2"/>.
    /// By default set to <see langword="true"/>.
    /// </summary>
    public bool ValidateInput { get; init; }

    /// <summary>
    /// Builds an instance with the provided terminators, validating the input by default. All text passed 
    /// to <see cref="Find(IEnumerable{char}, IEnumerable{char})"/> must conform with these two terminators.
    /// </summary>
    /// <param name="terminator1"><inheritdoc cref="Terminator1" path="/summary"/></param>
    /// <param name="terminator2"><inheritdoc cref="Terminator2" path="/summary"/></param>
    protected SuffixStructureBasedSnssFinder(char terminator1, char terminator2)
    {
        if (terminator1 == terminator2)
            throw new ArgumentException($"{nameof(terminator1)} and {nameof(terminator2)} must be different");

        Terminator1 = terminator1;
        Terminator2 = terminator2;
        ValidateInput = true;
    }

    /// <inheritdoc/>
    public abstract string? Find(IEnumerable<char> text1, IEnumerable<char> text2);
}
