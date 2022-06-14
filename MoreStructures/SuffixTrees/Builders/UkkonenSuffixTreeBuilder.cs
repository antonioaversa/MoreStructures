using MoreStructures.SuffixStructures.Builders;
using MoreStructures.SuffixTrees.Builders.Ukkonen;
using System.Diagnostics;

namespace MoreStructures.SuffixTrees.Builders;

/// <summary>
/// Builds objects, such as edges and nodes, for <see cref="SuffixTreeNode"/> structures.
/// </summary>
/// <remarks>
///     <para id="intro">
///     Iterative implementation of the Ukkonen algorithm: see 
///     <see href = "https://en.wikipedia.org/wiki/Ukkonen%27s_algorithm" /> for an introduction and
///     <see href="https://www.cs.helsinki.fi/u/ukkonen/SuffixT1withFigs.pdf"/> for the original paper.
///     </para>
///     <para id="algo-optimizations">
///     ALGORITHM OPTIMIZATIONS
///     <br/>
///     The algorithm applies some optimizations to achieve linear complexity:
///     <br/>
///     - <b>Edge Labels compression</b>: strings on edges are stored as (start, end) indexes. This makes the space
///       used by a single edge constant, i.e. O(1), because it always consists of two integers, regardless of 
///       how many chars of the text it represents.
///       <br/>
///     - <b>Global End</b>: all leafs have a dynamic end index, autoincremented every new phase. This makes the time 
///       used to apply Rule 1 Extension constant, i.e. O(1), because incrementing the global end also increments
///       all leaf implicitely.
///       <br/>
///     - <b>Suffix Links</b>: all internal nodes point to another internal node sharing the suffix. This makes more
///       efficient traversal, because storing and jumping the suffix link when needed means traversal doesn't 
///       have to be done again.
///     </para>
///     <para id="algo">
///     ALGORITHM
///     <br/>
///     The algorithm is structured in phases, as many as the number of chars in the text.
///     <br/>
///     At the beginning of a new phase, both the number of remaining suffixes to take care of, and the global end
///     pointer are both increased by 1.
///     <br/>
///     Each phase is composed of at least 1 iteration, each one taking care of remaining suffixes.
///     At the beginning 
///     </para>
///     <para id="usecases">
///     USECASES
///     <br/>
///     Not limited by call stack depth. Convenient with long input text (i.e. string having a length &lt; ~1K chars).
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     Time Complexity = O(t * as) and Space Complexity = O(2 * t) where t = length of the text to match and
///     as = size of the alphabet of the text. If the alphabet is of constant size, Time Complexity is linear.
///     Otherwise it is O(t * log(t)).
///     <br/>
///     While there are as many phases as number of chars in text (t), and there can be multiple iterations per
///     phase (as many as the number of remaining suffixes to process), the complexity is still linear, ~ 2t.
///     </para>
/// </remarks>
public class UkkonenSuffixTreeBuilder
    : IBuilder<SuffixTreeEdge, SuffixTreeNode>
{
    /// <summary>
    ///     <inheritdoc/>
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="UkkonenSuffixTreeBuilder" path="/remarks"/>
    /// </remarks>
    public SuffixTreeNode BuildTree(TextWithTerminator text)
    {
        var state = new IterationState(text);

        while (state.IsThereANextPhase())
        {
            state.StartPhaseIncreasingRemainingAndGlobalEnd();

            while (state.StillRemainingSuffixesInCurrentPhase())
            {
                state.JumpActiveNodeIfNecessary();

                bool showStopper;
                if (state.NoActivePointAndEdgeStartingFromActiveNodeWithCurrentChar() is MutableEdge edge)
                {
                    state.InitializeActiveEdgeAndLength(edge);
                    showStopper = true;
                }
                else if (state.ActivePointFollowedByCurrentChar())
                {
                    state.IncrementActiveLength();
                    showStopper = true;
                }
                else // No active point nor edge, or active point not followed by current char => Rule 2
                {
                    state.CreateLeafAndPossiblyIntermediateAndDecrementRemainingSuffixes();
                    showStopper = false;
                }

                Trace.WriteLine($"State at end of iteration: {state}");
                Trace.WriteLine("");

                if (showStopper)
                    break;
            }

            Trace.WriteLine("End of phase " + state.Phase);
            Trace.WriteLine("");
        }

        return BuildResult(state.Root);
    }

    private record struct BuildResultStackFrame(
        SuffixTreeEdge SuffixTreeEdge, 
        MutableNode MutableNode, 
        IDictionary<SuffixTreeEdge, SuffixTreeNode> ParentChildren,
        IDictionary<SuffixTreeEdge, SuffixTreeNode>? NodeChildrenMaybe);

    private static SuffixTreeNode BuildResult(MutableNode root)
    {
        var stack = new Stack<BuildResultStackFrame>();
        var rootParentChildren = new Dictionary<SuffixTreeEdge, SuffixTreeNode>() { };
        var suffixTreeEdge = new SuffixTreeEdge(0, 1);

        stack.Push(new(suffixTreeEdge, root, rootParentChildren, null));
        while (stack.Count > 0)
            ProcessStack(stack);
        return rootParentChildren[suffixTreeEdge];
    }

    private static void ProcessStack(Stack<BuildResultStackFrame> stack)
    {
        var (suffixTreeEdge, mutableNode, parentChildren, nodeChildrenMaybe) = stack.Pop();

        if (mutableNode.Children.Count == 0 || nodeChildrenMaybe != null)
        {
            parentChildren[suffixTreeEdge] = 
                nodeChildrenMaybe is IDictionary<SuffixTreeEdge, SuffixTreeNode> nodeChildren
                ? new SuffixTreeNode.Intermediate(nodeChildren)
                : new SuffixTreeNode.Leaf(mutableNode.LeafStart!.Value);
        }   
        else
        {
            var nodeChildren = new Dictionary<SuffixTreeEdge, SuffixTreeNode>() { };
            stack.Push(new(suffixTreeEdge, mutableNode, parentChildren, nodeChildren));
            foreach (var (childMutableEdge, childMutableNode) in mutableNode.Children)
            {
                var childSuffixTreeEdge = new SuffixTreeEdge(
                    childMutableEdge.Start, childMutableEdge.End.Value - childMutableEdge.Start + 1);
                stack.Push(new(childSuffixTreeEdge, childMutableNode, nodeChildren, null));
            }
        }
    }
}
