using MoreStructures.RecImmTrees;
using MoreStructures.RecImmTrees.Paths;
using MoreStructures.RecImmTrees.Visitor;
using MoreStructures.SuffixTries;
using MoreStructures.SuffixTries.Builders;
using MoreStructures.Utilities;

namespace MoreStructures.SuffixStructures.Matching;

/// <summary>
/// A <see cref="ISnssFinder"/> implementation and <see cref="SuffixStructureBasedSnssFinder"/> concretion which uses
/// a <see cref="SuffixTrieNode"/> structure to implement 
/// <see cref="ISnssFinder.Find(IEnumerable{char}, IEnumerable{char})"/>.
/// </summary>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - Compared to the naive implementation of <see cref="NaiveSnssFinder"/>, has better runtime, at the cost of 
///       space used (which is O(1) for the naive implementation).
///       <br/>
///     - Compared to the SuffixTree-based implementation, it has way worse Time and Space Complexity, but it's easier
///       to implement, visualize and debug step-by-step.
///     </para>
///     <para id="algo">
///     ALGORITHM
///     <br/>
///     - First uses the <see cref="NaivePartiallyRecursiveSuffixTrieBuilder"/> to build a suffix trie of the 
///       concatenation of first text, <see cref="SuffixStructureBasedSnssFinder.Terminator1"/>, second text and 
///       <see cref="SuffixStructureBasedSnssFinder.Terminator2"/>, a.k.a. generalized suffix trie.
///       <br/>
///     - Visit the suffix trie breadth-first, stopping at the first node such that the root-to-node prefix is 
///       substring of text1 but not of text2.
///       <br/>
///     - The root-to-node prefix is a substring of text1 when there is a path-to-leaf which contains an edge including
///       <see cref="SuffixStructureBasedSnssFinder.Terminator1"/>.
///       <br/>
///     - The root-to-node prefix is NOT a substring of text2 when there is no path-to-leaf which doesn't contain 
///       <see cref="SuffixStructureBasedSnssFinder.Terminator1"/>.
///       <br/>
///     - Such substring of text1 is guaranteed to be the shortest by the visit order imposed by the breadth-first 
///       search.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Validating the input requires going through text1 and text2, and takes linear time in the the number of chars 
///       n of the concatenated text "text1 ## separator1 ## text2 ## separator2", and constant space.
///       <br/>
///     - Building the Generalized Suffix Trie takes time and space at least proportional to the number of nodes of the
///       trie, which is quadratic with n.
///       <br/>
///     - For each level of the breadth-first traversal of the trie, all node-to-leaf paths are checked (in the worst 
///       case).
///       <br/>
///     - There are at most n levels in the trie, since there can't be a path longer than a suffix of the concatenated
///       text. The higher is the level, the shorter are node-to-leaf paths. However, their number is always the same
///       or lower.
///       <br/>
///     - There are as many node-to-leaf paths as leaves, and there are at most n leaves in the trie (since each
///       suffix can add potentially multiple intermediate nodes, but always a single leaf, having terminator2 as 
///       incoming edge).
///       <br/>
///     - Checking whether a path contains terminator1 takes constant space and a time proportional to the number of
///       nodes in the path, which is O(n).
///       <br/>
///     - Rebuilding the string from the identified path takes O(n) time and space.
///       <br/>
///     - So in conclusion, Time Complexity is O(n^3) and Space Complexity is O(n^2).
///     </para>
/// </remarks>
public class SuffixTrieBasedSnssFinder : SuffixStructureBasedSnssFinder
{
    private static readonly INodeToLeafPathsBuilder NodeToLeafPathsBuilder = 
        new FullyIterativeNodeToLeafPathsBuilder();

    /// <inheritdoc/>
    public SuffixTrieBasedSnssFinder(char terminator1, char terminator2) : base(terminator1, terminator2)
    {
    }

    /// <inheritdoc cref="SuffixStructureBasedSnssFinder" path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    /// </summary>
    public override string? Find(IEnumerable<char> text1Content, IEnumerable<char> text2Content)
    {
        if (ValidateInput)
        {
            if (text1Content.Contains(Terminator1) || text1Content.Contains(Terminator2))
                throw new ArgumentException(
                    $"Should not contain {nameof(Terminator1)} nor {nameof(Terminator2)}: {Terminator1} {Terminator2}",
                    nameof(text1Content));
            if (text2Content.Contains(Terminator1) || text2Content.Contains(Terminator2))
                throw new ArgumentException(
                    $"Should not contain {nameof(Terminator1)} nor {nameof(Terminator2)}: {Terminator1} {Terminator2}",
                    nameof(text2Content));
        }

        // Build Generalized Suffix Trie
        var text1 = new TextWithTerminator(text1Content, Terminator1);
        var text2 = new TextWithTerminator(text2Content, Terminator2);
        var text1And2 = new TextWithTerminator(text1Content.Append(Terminator1).Concat(text2Content), Terminator2);

        var suffixTrieBuilder = new NaivePartiallyRecursiveSuffixTrieBuilder();
        var suffixTrieRoot = suffixTrieBuilder.BuildTree(text1And2);

        // Breadth First Visit of the Trie
        var terminator1Index = text1.TerminatorIndex;
        var terminator2Index = text1And2.TerminatorIndex;
        var breadthFirstTraversal = new FullyIterativeBreadthFirstTraversal<SuffixTrieEdge, SuffixTrieNode>()
        {
            ChildrenSorter = visit => visit.Node.Children.Where(
                child => child.Key.Index != terminator1Index && child.Key.Index != terminator2Index),
            TraversalOrder = TreeTraversalOrder.ParentFirst,
        };
        var cachedVisits = new Dictionary<SuffixTrieNode, TreeTraversalVisit<SuffixTrieEdge, SuffixTrieNode>> { };
        var visits = breadthFirstTraversal
            .Visit(suffixTrieRoot)
            .Select(visit =>
            {
                cachedVisits[visit.Node] = visit;
                return visit;
            });

        var shortestSubstrNode = (
            from visit in visits
            let pathsToLeaf = NodeToLeafPathsBuilder.GetAllNodeToLeafPaths<SuffixTrieEdge, SuffixTrieNode>(visit.Node)
            // All path-to-leaf contain Terminator1 => root-to-node prefix is not substring of text2
            let notSubstringOfText2 = pathsToLeaf.All(path => path.ContainsIndex(terminator1Index))
            where notSubstringOfText2
            select visit.Node)
            .FirstOrDefault();

        if (shortestSubstrNode == null)
            return null;

        // Collect result, iterating up to the root
        return string.Concat(CollectPrefixChars(text1And2, shortestSubstrNode, cachedVisits).Reverse());
    }

    private static IEnumerable<string> CollectPrefixChars(
        TextWithTerminator text,
        SuffixTrieNode initialNode, 
        IDictionary<SuffixTrieNode, TreeTraversalVisit<SuffixTrieEdge, SuffixTrieNode>> cachedVisits)
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
