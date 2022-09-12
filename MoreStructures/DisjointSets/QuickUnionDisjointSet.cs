namespace MoreStructures.DisjointSets;

/// <summary>
/// An <see cref="IDisjointSet"/> implementation based on a simple <see cref="List{T}"/> conceptually storing a forest 
/// of trees, by defining the index of the parent node, for each value stored in the data structure.
/// </summary>
/// <remarks>
/// - This implementation optimizes the runtime of <see cref="Union(int, int)"/>, which only requires navigating to the
///   each root instead of a full linear scan of the list, at the cost of <see cref="Find(int)"/> and 
///   <see cref="AreConnected(int, int)"/>, which also require to navigate to the root.
///   <br/>
/// - Check <see cref="QuickFindDisjointSet"/> for an alternative implementation of <see cref="IDisjointSet"/> which 
///   does the opposite, in terms of optimization.
/// </remarks>
public class QuickUnionDisjointSet : IDisjointSet
{
    /// <summary>
    /// Maps the i-th item of the Disjoint Set to the parent in the tree structure, they belong to.
    /// <br/>
    /// I-th items which are roots have parent equal to themselves (i.e. <c>i</c>).
    /// </summary>
    protected int[] Parents { get; }

    /// <summary>
    /// Builds a Disjoint Set structure containing <paramref name="valuesCount"/> disjoint values, each one into its 
    /// own singleton tree.
    /// </summary>
    /// <param name="valuesCount"><inheritdoc cref="ValuesCount" path="/summary"/></param>
    /// <remarks>
    /// Requires initializing all the <paramref name="valuesCount"/> items of the underlying list, so that each item
    /// is conceptually in its own singleton tree.
    /// <br/>
    /// Time and Space Complexity are O(n).
    /// </remarks>
    public QuickUnionDisjointSet(int valuesCount)
    {
        ValuesCount = valuesCount >= 0
            ? valuesCount
            : throw new ArgumentException("Must be non-negative.", nameof(valuesCount));
        Parents = new int[valuesCount];

        for (var i = 0; i < ValuesCount; i++)
            Parents[i] = i;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Set at construction time, based on the constructor parameter.
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public virtual int ValuesCount { get; }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - Obtained as the number of distinct roots in the forest of trees.
    ///       <br/>
    ///     - All items i from 0 to <see cref="ValuesCount"/> - 1 are iterated over.
    ///       <br/>
    ///     - An item i is a root if it's parent of itself: <c>Parents[i] == i</c>.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Checking whether an item is parent of itself is a constant-time operation.
    ///       <br/>
    ///     - Therefore, Time and Space Complexity are O(n), where n is <see cref="ValuesCount"/>.
    ///     </para>
    /// </remarks>
    public virtual int SetsCount => Enumerable.Range(0, ValuesCount).Count(i => Parents[i] == i);

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - Finds the root of <paramref name="first"/> and the root of <paramref name="second"/> and compares them.
    ///       <br/>
    ///     - Because the root of a tree is unique, if the two roots coincide, <paramref name="first"/> and 
    ///       <paramref name="second"/> belong to the same tree, and are connected.
    ///       <br/>
    ///     - Otherwise they belong to two separate trees of the forest, and are not connected.
    ///     </para> 
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Find the two roots takes a time proportional to the height of the two trees.
    ///       <br/>
    ///     - If no mechanism to keep trees flat is put in place by this data structure, the height of each is O(n) in
    ///       the worst case.
    ///       <br/>
    ///     - Therefore, Time Complexity is O(n). Space Complexity is O(1).
    ///       <br/>
    ///     - If a sub-class of <see cref="QuickUnionDisjointSet"/> introduces mechanisms to keep trees flat, the 
    ///       complexity may become sub-linear. An example is <see cref="WeightedQuickUnionDisjointSet"/>.
    ///     </para>
    /// </remarks>
    public virtual bool AreConnected(int first, int second)
    {
        if (ValuesCount == 0)
            throw new InvalidOperationException(
                $"{nameof(AreConnected)} cannot be invoked on an empty set.");
        if (first < 0 || first >= ValuesCount)
            throw new ArgumentException(
                $"Must be non-negative and smaller than {nameof(ValuesCount)}.", nameof(first));
        if (second < 0 || second >= ValuesCount)
            throw new ArgumentException(
                $"Must be non-negative and smaller than {nameof(ValuesCount)}.", nameof(second));

        if (first == second)
            return true;

        return Find(first) == Find(second);
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - Finds the root of <paramref name="value"/>, by iteratively traversing the tree upwards, until the root
    ///       of the tree is reached.
    ///       <br/>
    ///     - Because the root of a tree is unique and can be reached by every node of the tree navigating upwards, it
    ///       represents a good identifier of the set of all items in the same tree of the forest.
    ///     </para> 
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Find the root takes a time proportional to the height of the tree, the item is in.
    ///       <br/>
    ///     - If no mechanism to keep trees flat is put in place by this data structure, the height of the tree 
    ///       is O(n) in the worst case.
    ///       <br/>
    ///     - Therefore, Time Complexity is O(n). Space Complexity is O(1).
    ///       <br/>
    ///     - If a sub-class of <see cref="QuickUnionDisjointSet"/> introduces mechanisms to keep trees flat, the 
    ///       complexity may become sub-linear. An example is <see cref="WeightedQuickUnionDisjointSet"/>.
    ///     </para>
    /// </remarks>
    public virtual int Find(int value)
    {
        if (ValuesCount == 0)
            throw new InvalidOperationException(
                $"{nameof(AreConnected)} cannot be invoked on an empty set.");
        if (value < 0 || value >= ValuesCount)
            throw new ArgumentException(
                $"Must be non-negative and smaller than {nameof(ValuesCount)}.", nameof(value));

        while (Parents[value] != value)
            value = Parents[value];
        return value;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - It first finds the root R1 of <paramref name="first"/> and the root R2 of <paramref name="second"/>, by
    ///       running <see cref="Find(int)"/> on each item.
    ///       <br/>
    ///     - Then, it attaches R2 as immediate child of R1, by setting the parent of R2 to R1.
    ///       <br/>
    ///     - This way the two three now are merged into a single one, which means that all items of the two trees, 
    ///       including <paramref name="first"/> and <paramref name="second"/> are now in the same set.
    ///     </para> 
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Find the two roots takes a time proportional to the height of the two trees.
    ///       <br/>
    ///     - If no mechanism to keep trees flat is put in place by this data structure, the height of each tree 
    ///       is O(n) in the worst case.
    ///       <br/>
    ///     - Attaching one root as child of the other is a constant-time operation, since it only requires setting the
    ///       parent in the list of parents, which is O(1) work.
    ///       <br/>
    ///     - Therefore, Time Complexity is O(n). Space Complexity is O(1).
    ///       <br/>
    ///     - If a sub-class of <see cref="QuickUnionDisjointSet"/> introduces mechanisms to keep trees flat, the 
    ///       complexity may become sub-linear. An example is <see cref="WeightedQuickUnionDisjointSet"/>.
    ///     </para>
    /// </remarks>
    public virtual void Union(int first, int second)
    {
        if (ValuesCount == 0)
            throw new InvalidOperationException(
                $"{nameof(AreConnected)} cannot be invoked on an empty set.");
        if (first < 0 || first >= ValuesCount)
            throw new ArgumentException(
                $"Must be non-negative and smaller than {nameof(ValuesCount)}.", nameof(first));
        if (second < 0 || second >= ValuesCount)
            throw new ArgumentException(
                $"Must be non-negative and smaller than {nameof(ValuesCount)}.", nameof(second));

        Parents[Find(second)] = Find(first);
    }
}
