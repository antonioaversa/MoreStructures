namespace MoreStructures.DisjointSets;

/// <summary>
/// A <see cref="WeightedQuickUnionDisjointSet"/> refinement which improves the performance of the data structure by 
/// performing a technique known as <b>path compression</b>.
/// </summary>
/// <remarks>
/// This variant of <see cref="WeightedQuickUnionDisjointSet"/> introduces a refinement of the 
/// <see cref="IDisjointSet.Find(int)"/> method, inherited by its upper classes.
/// <br/>
/// The <see cref="Find(int)"/> algorithm remains unchanged in its core. 
/// <br/>
/// However, it produces a flatter forest as side-effect, every time is executed on a value.
/// <br/>
/// Check the documentation of <see cref="Find(int)"/> for further details.
/// </remarks>
public class PathCompressionWeightedQuickUnionDisjointSet : WeightedQuickUnionDisjointSet
{
    /// <inheritdoc/>
    public PathCompressionWeightedQuickUnionDisjointSet(int valuesCount) : base(valuesCount)
    {
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - The algorithm is pretty much like <see cref="QuickUnionDisjointSet.Find(int)"/>, with one main 
    ///       difference.
    ///       <br/>
    ///     - The difference lies in <b>path compression</b>: once the root of the tree is found, a second traversal, 
    ///       from the <paramref name="value"/> up to its root, is performed.
    ///       <br/>
    ///     - During the second traversal, each node in path is directly attached to the root. 
    ///       <br/>
    ///     - That keeps the disjoint set equivalent to its previous state, by transitivity of the equivalence 
    ///       relationship.
    ///       <br/>
    ///     - However, it makes the tree much more flat, and the average height of all the nodes in the forest smaller.
    ///       <br/>
    ///     - That means that future <see cref="Find(int)"/>, but also <see cref="IDisjointSet.Union(int, int)"/> and
    ///       <see cref="IDisjointSet.AreConnected(int, int)"/>, will be much faster, since it's faster to reach the 
    ///       root of each tree.
    ///     </para> 
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Find the root takes a time proportional to the height of the tree, the item is in.
    ///       <br/>
    ///     - Because path compression keeps trees very flat, incrementing the fan out, while the complexity is not
    ///       constant and there is definitely a dependence over n, it is a sub-logarithmic dependency.
    ///       <br/>
    ///     - More precisely, Time Complexity is O(log*(n)), which can be considered "pseudo-constant" for any 
    ///       "real world" value of n.
    ///       <br/>
    ///     - Space Complexity remains O(1), since only a constant amount of additional space is required to run the 
    ///       algorithm.
    ///     </para>
    /// </remarks>
    public override int Find(int value)
    {
        if (ValuesCount == 0)
            throw new InvalidOperationException(
                $"{nameof(AreConnected)} cannot be invoked on an empty set.");
        if (value < 0 || value >= ValuesCount)
            throw new ArgumentException(
                $"Must be non-negative and smaller than {nameof(ValuesCount)}.", nameof(value));

        var root = value;
        while (Parents[root] != root)
        {
            root = Parents[root];
        }

        while (Parents[value] != value)
        {
            var parent = Parents[value];
            Parents[value] = root;
            value = parent;
        }

        return root;
    }
}
