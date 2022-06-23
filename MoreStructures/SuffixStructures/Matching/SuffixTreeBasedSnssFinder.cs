using MoreStructures.RecImmTrees.Paths;
using MoreStructures.RecImmTrees.Visitor;
using MoreStructures.SuffixStructures.Builders;
using MoreStructures.SuffixTrees;
using MoreStructures.SuffixTrees.Builders;
using MoreStructures.Utilities;

namespace MoreStructures.SuffixStructures.Matching;

/// <summary>
/// A <see cref="ISnssFinder"/> implementation and <see cref="SuffixStructureBasedSnssFinder"/> concretion which uses
/// a <see cref="SuffixTreeNode"/> structure to implement 
/// <see cref="ISnssFinder.Find(IEnumerable{char}, IEnumerable{char})"/>.
/// </summary>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - Compared to the naive implementation of <see cref="NaiveSnssFinder"/>, has both average and worst case better 
///       runtime, at the cost of space used (which is O(1) for the naive implementation).
///       <br/>
///     - Compared to <see cref="SuffixTrieBasedSnssFinder"/>, it has better Time and Space Complexity, due to Branches 
///       Coaleascing combined with Path Compression. It's, however, more complex to implement, visualize and debug 
///       step-by-step.
///     </para>
///     <para id="algo">
///     ALGORITHM
///     <br/>
///     - First uses the <see cref="UkkonenSuffixTreeBuilder"/> to build a suffix tree of the 
///       concatenation of first text, <see cref="SuffixStructureBasedSnssFinder.Terminator1"/>, second text and 
///       <see cref="SuffixStructureBasedSnssFinder.Terminator2"/>, stopping branches at the first terminator 
///       encountered. This structure is also known as Generalized Suffix Tree.
///       <br/>
///     - Visit the suffix tree breadth-first, stopping at the first node such that the root-to-node prefix is 
///       substring of text1 but not of text2.
///       <br/>
///     - The root-to-node prefix is a substring of text1 when there is a path-to-leaf which contains an edge including
///       <see cref="SuffixStructureBasedSnssFinder.Terminator1"/>.
///       <br/>
///     - The root-to-node prefix is NOT a substring of text2 when there is no path-to-leaf which doesn't contain 
///       <see cref="SuffixStructureBasedSnssFinder.Terminator1"/>.
///       <br/>
///     - Such substring of text1 is guaranteed to be the shortest in number of edges by the visit order imposed by the 
///       breadth-first search. However, unlike for Suffix Tries, in Suffix Trees the number of chars per edges varies.
///       <br/>
///     - Therefore, all pontential results have to be collected, then sorted by actual prefix length.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Validating the input requires going through text1 and text2, and takes linear time in the the number of chars 
///       n of the concatenated text "text1 ## separator1 ## text2 ## separator2", and constant space.
///       <br/>
///     - Building the Generalized Suffix Tree via the Ukkonen algorithm takes time and space at least proportional to 
///       the number of nodes of the tree, which is linear with n.
///       <br/>
///     - For each level of the breadth-first traversal of the trie, all node-to-leaf paths are checked (in the worst 
///       case).
///       <br/>
///     - There are at most n levels in the tree, since there can't be a path longer than a suffix of the concatenated
///       text. The higher is the level, the shorter are node-to-leaf paths. However, their number is always the same
///       or lower.
///       <br/>
///     - For each node there are as many node-to-leaf paths as leaves, and there are at most n leaves in the tree 
///       (since each suffix can add at most a single intermediate node and a single leaf, having terminator 1 or 
///       terminator2 as incoming edge).
///       <br/>
///     - Checking whether a path contains terminator1 takes constant space and a time proportional to the number of
///       nodes in the path, which is O(n).
///       <br/>
///     - The following optimization is implemented: if a path P1 starting from a node N1 identifies a prefix p1 which
///       is a potential SNSS, all paths starting from nodes Pi which are descendants of N1 would identify prefixes pi
///       which would be longer than p1, so they can be excluded.
///       <br/>
///     - Rebuilding the string from each identified path takes O(n) time and space. Sorting would take time 
///       O(m * log(m) * n) where m is the number of potential SNSS (which is in average much smaller than n).
///       <br/>
///     - So in conclusion, Time Complexity is O(n^2) and Space Complexity is O(n).
///     </para>
/// </remarks>
public class SuffixTreeBasedSnssFinder : SuffixStructureBasedSnssFinder
{
    private static readonly IBuilder<SuffixTreeEdge, SuffixTreeNode> SuffixTreeBuilder =
        new UkkonenSuffixTreeBuilder();

    private static readonly INodeToLeafPathsBuilder NodeToLeafPathsBuilder =
        new FullyIterativeNodeToLeafPathsBuilder();

    /// <inheritdoc/>
    public SuffixTreeBasedSnssFinder(char terminator1, char terminator2) : base(terminator1, terminator2)
    {
    }

    /// <inheritdoc cref="SuffixStructureBasedSnssFinder" path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc cref="SuffixStructureBasedSnssFinder.Find(IEnumerable{char}, IEnumerable{char})"/>
    ///     <br/>
    ///     This implementation builds and uses a <see cref="SuffixTreeNode"/> structure to perform the search.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="SuffixTreeBasedSnssFinder" path="/remarks"/>
    /// </remarks>
    public override IEnumerable<string> Find(IEnumerable<char> text1, IEnumerable<char> text2)
    {
        if (ValidateInput)
            ValidateTexts(text1, text2);

        // Build Generalized Suffix Tree
        var text1WithTerminator = new TextWithTerminator(text1, Terminator1);
        var text2WithTerminator = new TextWithTerminator(text2, Terminator2);
        var text1And2 = new TextWithTerminator(text1.Append(Terminator1).Concat(text2), Terminator2);
        var suffixTree = SuffixTreeBuilder.BuildTree(text1WithTerminator, text2WithTerminator);

        // Breadth First Visit of the Tree
        var terminator1Index = text1WithTerminator.TerminatorIndex;
        var terminator2Index = text1And2.TerminatorIndex;
        var breadthFirstTraversal = new FullyIterativeBreadthFirstTraversal<SuffixTreeEdge, SuffixTreeNode>()
        {
            ChildrenSorter = visit => visit.Node.Children
                .Where(child => child.Key.Start != terminator1Index && child.Key.Start != terminator2Index)
                .OrderBy(child => child.Key.Length),
            TraversalOrder = TreeTraversalOrder.ParentFirst,
        };
        var cachedVisits = new Dictionary<SuffixTreeNode, TreeTraversalVisit<SuffixTreeEdge, SuffixTreeNode>>();
        var visits = breadthFirstTraversal
            .Visit(suffixTree)
            .Select(visit =>
            {
                cachedVisits[visit.Node] = visit;
                return visit;
            });

        var shortestSubstrNodes = new HashSet<SuffixTreeNode>(EqualityComparer<object>.Default); // Just compare refs

        foreach (var visit in visits)
        {
            // If the parent of this node has already identified a SNSS => this node would make a longer string => skip
            if (visit.ParentNode != null && shortestSubstrNodes.Contains(visit.ParentNode))
                continue;

            // If the incoming edge contains Terminator2 => it's a leaf substring of text2 => skip
            if (visit.IncomingEdge != null && visit.IncomingEdge.ContainsIndex(terminator2Index))
                continue;

            var pathsToLeaf = NodeToLeafPathsBuilder.GetAllNodeToLeafPaths<SuffixTreeEdge, SuffixTreeNode>(visit.Node);

            // Any path-to-leaf contains Terminator2 => root-to-node prefix is substring of text2 => skip
            if (pathsToLeaf.Any(path => path.ContainsIndex(terminator2Index)))
                continue;

            shortestSubstrNodes.Add(visit.Node);
        }

        // Collect result, iterating up to the root (take only 1st char of last edge) and find the shortest.
        var results =
            from shortestSubstrNode in shortestSubstrNodes
            let prefix = GetPrefix(shortestSubstrNode, text1And2, cachedVisits)
            orderby prefix.Length ascending
            select prefix;

        var (firstOrEmpty, reminder) = results.EnumerateAtMostFirst(1);
        if (!firstOrEmpty.Any())
            return firstOrEmpty;

        var first = firstOrEmpty.Single();
        return results.TakeWhile(s => s.Length == first.Length).Prepend(first);
    }

    private static string GetPrefix(
        SuffixTreeNode node,
        TextWithTerminator text1And2,
        Dictionary<SuffixTreeNode, TreeTraversalVisit<SuffixTreeEdge, SuffixTreeNode>> cachedVisits)
    {
        var edges = CollectPrefixChars(text1And2, node, cachedVisits);
        var firstCharOfLastEdge = edges.First()[0];
        return string.Concat(edges.Skip(1).Reverse()) + firstCharOfLastEdge;
    }
}