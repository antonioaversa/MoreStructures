using MoreStructures.SuffixTrees;

namespace MoreStructures.MutableTrees.Conversions;

/// <summary>
/// An implementation of <see cref="IConversion"/> using a <see cref="Stack{T}"/> to perform conversions.
/// </summary>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     <inheritdoc cref="DocFragments" path="/remarks/para[@id='fully-iterative-advantages']"/>
///     </para>
///     <para id="algorithm">
///     ALGORITHM
///     <br/>
///     - The algorithm first visits the provided <see cref="MutableTree.Node"/> structure top-down, stacking up each
///       mutable node (root, intermediate and leaves) in custom "stack frame objects" onto a <see cref="Stack{T}"/> 
///       data structure.
///       <br/>
///     - Then, it builds the output <see cref="SuffixTreeNode"/> structure node by node, bottom-up, trimming at the 
///       same time the edges at the first encountered terminator. It does so by adding each 
///       <see cref="MutableTree.Node"/> instance encountered in the top-down visit, which is not a leaf, to the same 
///       <see cref="Stack{T}"/> instance used for the top-down visit.
///       <br/>
///     - Together with the mutable node being processed, the stack frame object also stores other data, required to
///       build the output structure.
///       <br/>
///     - 1. The incoming <see cref="SuffixTreeEdge"/>. It's non-nullable.
///       <br/>
///     - 2. The <see cref="IDictionary{TKey, TValue}"/> of (<see cref="SuffixTreeEdge"/>, 
///       <see cref="SuffixTreeNode"/>) pairs for the children of the <see cref="SuffixTreeNode"/> parent of this 
///       mutable node. Once fully populated, the dictionary will be used to build the immutable parent node. It's 
///       non-nullable.
///       <br/>
///     - 3. The one for the children of the <see cref="SuffixTreeNode"/> corresponding to this mutable node. 
///       Once fully populated, the dictionary will be used to build the immutable node. It's nullable.
///       <br/>
///     - A special treatment is reserved for the root mutable node, which doesn't have an incoming edge, nor a parent
///       node and the stack frame of which initializes the stack before top-down traversal: dummy objects are created.
///       <br/>
///     - Then top-down traversal of the tree is done, by processing the frame on top of the stack while the stack is
///       not empty.
///       <br/>
///     - During tree descent, mutable nodes of children (intermediate and leaves) are added to the stack without 
///       dictionary of children.
///       <br/>
///     - Such dictionary is instantiated when visiting each node during descent and passed to the stack frames of each of
///       the children, so that each children can add the <see cref="SuffixTreeNode"/> to the collection of children of
///       its parent during the bottom-up traversal phase.
///       <br/>
///     - After instantiating the dictionary of children, the node being visited is re-added to the top of the stack. 
///       <br/>
///     - Then, children of the node (and their incoming edges) are added to the top of the stack, in reverse order, 
///       so that children are visited in straight order and before the parent.
///       <br/>
///     - While stacking up children, edge labels are trimmed to the 1st encountered terminator, if any.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Each node is added to the stack at most twice: once during the top-down visit (when the dictionary of 
///       children is not set) and once during the bottom-up visit (when the dictionary of children is set) - the
///       second scenario only happening for non-leaves.
///       <br/>
///     - Operations per stack frame processing are all done in constant time: stack frame deconstruction, stack depth
///       check, children count, children dictionary instantiation, <see cref="SuffixTreeNode.Leaf"/> and 
///       <see cref="SuffixTreeNode.Intermediate"/> creation.
///       <br/>
///     - There is one exception though: edge trimming takes time O(n * m), where n is the length of the text and m
///       is the number of terminators, hence the number of concatenated <see cref="TextWithTerminator"/>.
///       <br/>
///     - In conclusion, Time Complexity is O(n^2 * m) and Space Complexity is O(n).
///     </para>
/// </remarks>
internal class FullyIterativeConversion : IConversion
{
    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="FullyIterativeConversion"/>
    /// </remarks>
    public SuffixTreeNode ConvertToSuffixTree(
        MutableTree.Node mutableNode, TextWithTerminator fullText, ISet<char> terminators)
    {
        var stack = new Stack<StackFrame>();
        var rootIncomingEdge = new SuffixTreeEdge(0, 1);
        var rootParentChildren = new Dictionary<SuffixTreeEdge, SuffixTreeNode>();
        stack.Push(new(mutableNode, rootIncomingEdge, rootParentChildren, null));

        var terminatorsIndexMap = 
            TextWithTerminatorExtensions.BuildTerminatorsIndexMap(fullText, terminators).ToList();

        while (stack.Count > 0)
            ProcessStack(stack, terminatorsIndexMap);

        return rootParentChildren[rootIncomingEdge];
    }

    private sealed record StackFrame(
            MutableTree.Node MutableNode,
            SuffixTreeEdge IncomingEdge,
            IDictionary<SuffixTreeEdge, SuffixTreeNode> ParentChildren,
            IDictionary<SuffixTreeEdge, SuffixTreeNode>? Children);

    private static void ProcessStack(Stack<StackFrame> stack, IList<int> terminatorsIndexMap)
    {
        var (mutableNode, incomingEdge, parentChildren, children) = stack.Pop();

        if (!mutableNode.Children.Any())
        {
            // It is a leaf => generate node and store into parentChildren
            parentChildren[incomingEdge] = new SuffixTreeNode.Leaf(mutableNode.LeafStart!.Value);
            return;
        }

        if (children != null)
        {
            // Intermediate, whose children have already been processed => generate node and store into parentChildren
            parentChildren[incomingEdge] = new SuffixTreeNode.Intermediate(children);
            return;
        }

        // Intermediate, whose children have not been processed yet => re-push mutable node, this time with a reference
        // to a newly created dictionary of children, then push children onto the stack, to have them processed first
        // and populating the instantiated children dictionary.
        children = new Dictionary<SuffixTreeEdge, SuffixTreeNode>();
        stack.Push(new(mutableNode, incomingEdge, parentChildren, children));
        foreach (var (childMutableEdge, childMutableNode) in mutableNode.Children.Reverse())
        {
            var childEdge = new SuffixTreeEdge(childMutableEdge.Start, childMutableEdge.Length);

            // If the child edge contains any terminator, trim tree
            if (terminatorsIndexMap[childEdge.Start + childEdge.Length - 1] != terminatorsIndexMap[childEdge.Start])
            {
                childMutableNode.Children.Clear();
                childEdge = new SuffixTreeEdge(childMutableEdge.Start, 
                    terminatorsIndexMap[childEdge.Start] - childMutableEdge.Start + 1);
            }

            stack.Push(new(childMutableNode, childEdge, children, null));
        }
    }
}
