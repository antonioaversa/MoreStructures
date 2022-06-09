namespace MoreStructures.RecImmTrees.Visitor;

/// <summary>
/// The order of visit of a "node and its children" sub-tree.
/// </summary>
public enum TreeTraversalOrder 
{
    /// <summary>
    /// First visit the parent node, then its children.
    /// </summary>
    ParentFirst, 

    /// <summary>
    /// First visit all the children nodes, then the parent.
    /// </summary>
    ChildrenFirst 
}
