using MoreStructures.RecImmTrees;
using MoreStructures.RecImmTrees.Visitor;
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

    /// <summary>
    /// Validates the provided texts against this finder, checking that they are compatible with 
    /// <see cref="Terminator1"/> and <see cref="Terminator2"/>.
    /// </summary>
    /// <param name="text1">The first text to validate.</param>
    /// <param name="text2">The second text to validate.</param>
    protected void ValidateTexts(IEnumerable<char> text1, IEnumerable<char> text2)
    {
        if (text1.Contains(Terminator1) || text1.Contains(Terminator2))
            throw new ArgumentException(
                $"Should not contain {nameof(Terminator1)} nor {nameof(Terminator2)}: {Terminator1} {Terminator2}",
                nameof(text1));
        if (text2.Contains(Terminator1) || text2.Contains(Terminator2))
            throw new ArgumentException(
                $"Should not contain {nameof(Terminator1)} nor {nameof(Terminator2)}: {Terminator1} {Terminator2}",
                nameof(text2));
    }

    /// <summary>
    /// Rebuilds the root-to-node prefix, from <paramref name="initialNode"/> up to the root of the Suffix Tree (node 
    /// with no parent), using the provided cache of visited nodes, <paramref name="cachedVisits"/>, to navigate the 
    /// Suffix Tree upwards.
    /// </summary>
    /// <param name="text">The text used to generate the Suffix Tree, and to be used to rebuild the prefix.</param>
    /// <param name="initialNode">The node, to start navigating from.</param>
    /// <param name="cachedVisits">A dictionary of visits by node, to jump from a node to its parent.</param>
    /// <returns>
    /// A lazily generated sequence of strings, corresponding to the edges from <paramref name="initialNode"/> up to 
    /// the root of the Suffix Tree. Empty if <paramref name="initialNode"/> is the root of the tree.
    /// </returns>
    protected static IEnumerable<string> CollectPrefixChars<TEdge, TNode>(
        TextWithTerminator text,
        TNode initialNode,
        IDictionary<TNode, TreeTraversalVisit<TEdge, TNode>> cachedVisits)
        where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>, TextWithTerminator.ISelector
        where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
    {
        var node = initialNode;
        while (true)
        {
            var (_, parentNode, incomingEdge, _) = cachedVisits[node];

            if (parentNode == null)
                yield break;

            yield return text[incomingEdge!]; // if parentNode != null, then incomingEdge is also != null
            node = parentNode;
        }
    }
}
