namespace MoreStructures.DisjointSets;

/// <summary>
/// A data structure modelling a collection of sets of non-negative consecutive integer values 0..k-1, where set can be
/// easily merged together and values can be easily checked for membership to the same set.
/// </summary>
public interface IDisjointSet
{
    /// <summary>
    /// The number of integer values in the data structure.
    /// </summary>
    int ValuesCount { get; }

    /// <summary>
    /// The number of distinct sets, the data structure is made of.
    /// </summary>
    int SetCount { get; }

    /// <summary>
    /// Establishes a "union" relashionship between the integer values <paramref name="first"/> and 
    /// <paramref name="second"/>, merging the sets of the two values into a single set.
    /// </summary>
    /// <param name="first">The value of the first integer.</param>
    /// <param name="second">The value of the second integer.</param>
    void Union(int first, int second);

    /// <summary>
    /// Whether the two provided integer values belong to the same set, or two disjoint sets.
    /// </summary>
    /// <param name="first">The value of the first integer.</param>
    /// <param name="second">The value of the second integer.</param>
    /// <returns><see langword="true"/> if the values belong to the same set, false otherwise.</returns>
    bool AreConnected(int first, int second);

    /// <summary>
    /// Returns the set identifier of the provided <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value of the integer, to find the set of.</param>
    /// <returns>An <see cref="int"/> identifing the set, <paramref name="value"/> is member of.</returns>
    int Find(int value);
}
