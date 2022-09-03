namespace MoreStructures.DisjointSets;

/// <summary>
/// A data structure modelling a collection of sets of non-negative consecutive integer values 0..k-1, where set can be
/// easily merged together and values can be easily checked for membership to the same set.
/// </summary>
/// <remarks>
/// Disjoint sets are effective when dealing with equivalence relationships, i.e. relationships which are reflexive, 
/// symmetric and transitive.
/// <br/>
/// It's not directly usable when relationships have direction (e.g. if A points to B, B doesn't necessarily points to 
/// A).
/// </remarks>
public interface IDisjointSet
{
    /// <summary>
    /// The number of integer values in the data structure.
    /// </summary>
    /// <value>
    /// A non-negative <see cref="int"/>.
    /// </value>
    int ValuesCount { get; }

    /// <summary>
    /// The number of distinct sets, the data structure is made of.
    /// </summary>
    /// <value>
    /// A non-negative <see cref="int"/>, non-bigger than <see cref="ValuesCount"/>.
    /// </value>
    int SetCount { get; }

    /// <summary>
    /// Establishes a "union" relashionship between the integer values <paramref name="first"/> and 
    /// <paramref name="second"/>, merging the sets of the two values into a single set.
    /// </summary>
    /// <param name="first">
    /// The value of the first integer. Must be non-negative and smaller than <see cref="ValuesCount"/>.
    /// </param>
    /// <param name="second">
    /// The value of the second integer. Must be non-negative and smaller than <see cref="ValuesCount"/>.
    /// </param>
    void Union(int first, int second);

    /// <summary>
    /// Whether the two provided integer values belong to the same set, or two disjoint sets.
    /// </summary>
    /// <param name="first">
    /// The value of the first integer. Must be non-negative and smaller than <see cref="ValuesCount"/>.
    /// </param>
    /// <param name="second">
    /// The value of the second integer. Must be non-negative and smaller than <see cref="ValuesCount"/>.
    /// </param>
    /// <returns><see langword="true"/> if the values belong to the same set, false otherwise.</returns>
    bool AreConnected(int first, int second);

    /// <summary>
    /// Returns the set identifier of the provided <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// The value of the integer, to find the set of. Must be non-negative and smaller than <see cref="ValuesCount"/>.
    /// </param>
    /// <returns>An <see cref="int"/> identifing the set, <paramref name="value"/> is member of.</returns>
    int Find(int value);
}
