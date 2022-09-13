namespace MoreStructures.PriorityQueues.BinomialHeap;

/// <summary>
/// A node of a tree, root or non-root, in the underlying forest representing the heap.
/// </summary>
public class TreeNode<T>
{
    /// <summary>
    /// The item of type <typeparamref name="T"/>, with its priority and push timestamp.
    /// </summary>
    public PrioritizedItem<T> PrioritizedItem { get; set; }

    /// <summary>
    /// A <see cref="LinkedList{T}"/> of the children of this node. Empty if leaf.
    /// </summary>
    public LinkedList<TreeNode<T>> Children { get; private set; } = new();

    /// <summary>
    /// A back-reference to the parent node. Null if a root.
    /// </summary>
    public TreeNode<T>? Parent { get; set; } = null;

    /// <summary>
    /// A back-reference to the <see cref="LinkedListNode{T}"/> wrapper, in the <see cref="LinkedList{T}"/> of 
    /// tree roots in the underlying forest representing the heap. Null if not a root.
    /// </summary>
    public LinkedListNode<TreeNode<T>>? RootsListNode { get; set; } = null;

    /// <summary>
    /// A back-reference to the <see cref="LinkedListNode{T}"/> wrapper, in the <see cref="LinkedList{T}"/> of
    /// children of the <see cref="Parent"/> of this node. Null if a root.
    /// </summary>
    public LinkedListNode<TreeNode<T>>? ParentListNode { get; set; } = null;

    /// <summary>
    /// Whether this node is in the heap, either as a root or as a non-root node in a tree of the forest, or it is
    /// a dangling or detached node.
    /// </summary>
    public bool IsInAHeap => ParentListNode != null || RootsListNode != null;

    /// <summary>
    /// Add the provides <paramref name="treeNode"/> to the <see cref="Children"/> of this instance.
    /// </summary>
    /// <param name="treeNode">The <see cref="TreeNode{T}"/> instance to become a child.</param>
    public void AddChild(TreeNode<T> treeNode)
    {
        if (treeNode.Parent != null || treeNode.ParentListNode != null)
            throw new InvalidOperationException($"{nameof(treeNode)} cannot be already a child of another node.");
        if (treeNode.RootsListNode != null)
            throw new InvalidOperationException($"{nameof(treeNode)} cannot be a root.");

        treeNode.Parent = this;
        treeNode.ParentListNode = Children.AddLast(treeNode);
    }

    /// <summary>
    /// Removes this node from the <see cref="Children"/> of its <see cref="Parent"/>.
    /// </summary>
    public void DetachFromParent()
    {
        if (Parent == null || ParentListNode == null)
            throw new InvalidOperationException($"This node must be child of a node.");
        if (RootsListNode != null)
            throw new InvalidOperationException("Incoherent state: node both a child and a root.");

        Parent.Children.Remove(ParentListNode!);

        Parent = null;
        ParentListNode = null;
    }

    /// <summary>
    /// Deep copies this <see cref="TreeNode{T}"/> and its entire structure.
    /// </summary>
    /// <returns>
    /// A new instance of <see cref="TreeNode{T}"/>, pointing to a new, separate but equivalent structure.
    /// </returns>
    /// <remarks>
    /// This method is supposed to be used for a <b>temporary copy</b> of the heap, in order to iterate over it
    /// without modifying the original heap.
    /// <br/>
    /// It is not conceived to support full clones of a heap, such the one required by <see cref="ICloneable"/>.
    /// <br/>
    /// It doesn't copy <see cref="Parent"/> for the top-level <see cref="TreeNode{T}"/>, nor its 
    /// <see cref="RootsListNode"/> or <see cref="ParentListNode"/>: those have to be set, according to the 
    /// scenario, by the caller of <see cref="DeepCopy"/>.
    /// </remarks>
    public TreeNode<T> DeepCopy()
    {
        // Parent and ParentListNode are taken care by the parent TreeNode
        // RootsListNode is taken care by the top-level copy of the heap
        var copy = new TreeNode<T> { PrioritizedItem = PrioritizedItem };

        foreach (var childCopy in Children.Select(c => c.DeepCopy()))
            copy.AddChild(childCopy);

        return copy;
    }

    /// <inheritdoc path="//*[not(self::summary)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     Includes the <see cref="PrioritizedItem"/> and <see cref="IsInAHeap"/>.
    /// </summary>
    public override string ToString()
    {
        return $"{PrioritizedItem} [{(IsInAHeap ? "In a heap" : "Not in a heap")}]";
    }
}