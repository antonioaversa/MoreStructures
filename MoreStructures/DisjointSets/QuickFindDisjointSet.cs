namespace MoreStructures.DisjointSets;

/// <summary>
/// An <see cref="IDisjointSet"/> implementation based on a simple <see cref="List{T}"/> to store set ids of each of 
/// the values stored in the Disjoint Set.
/// </summary>
/// <remarks>
/// - This implementation optimizes <see cref="Find(int)"/> and <see cref="AreConnected(int, int)"/> runtime, making 
///   them constant-time operations, at the cost of <see cref="Union(int, int)"/>, which requires a full scan of the 
///   underlying list and hence is a O(n) operation.
///   <br/>
/// - Check <see cref="QuickUnionDisjointSet"/> for an alternative implementation of <see cref="IDisjointSet"/> which 
///   does the opposite, in terms of optimization.
/// </remarks>
public class QuickFindDisjointSet : IDisjointSet
{
    private int[] SetIds { get; }

    /// <summary>
    /// Builds a Disjoint Set structure containing <paramref name="valuesCount"/> disjoint values, each one into its 
    /// own singleton set.
    /// </summary>
    /// <param name="valuesCount"><inheritdoc cref="ValuesCount" path="/summary"/></param>
    /// <remarks>
    /// Requires initializing all the <paramref name="valuesCount"/> items of the underlying list.
    /// <br/>
    /// Time and Space Complexity are O(n).
    /// </remarks>
    public QuickFindDisjointSet(int valuesCount)
    {
        ValuesCount = valuesCount >= 0 
            ? valuesCount 
            : throw new ArgumentException("Must be non-negative.", nameof(valuesCount));
        SetIds = new int[valuesCount];

        for (var i = 0; i < ValuesCount; i++)
            SetIds[i] = i;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Set at construction time, based on the constructor parameter.
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public int ValuesCount { get; }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Obtained as the number of distinct values in the underlying list, which is scanned linearly.
    /// <br/>
    /// Time Complexity is O(n) and Space Complexity is O(1), where n is <see cref="ValuesCount"/>.
    /// </remarks>
    public int SetsCount => SetIds.Distinct().Count();

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Calculated in constant-time by comparing the set id of the first value with the set id of the second value.
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public bool AreConnected(int first, int second)
    {
        if (ValuesCount == 0)
            throw new InvalidOperationException(
                $"{nameof(AreConnected)} cannot be invoked on an empty queue.");
        if (first < 0 || first >= ValuesCount)
            throw new ArgumentException(
                $"Must be non-negative and smaller than {nameof(ValuesCount)}.", nameof(first));
        if (second < 0 || second >= ValuesCount)
            throw new ArgumentException(
                $"Must be non-negative and smaller than {nameof(ValuesCount)}.", nameof(second));

        return SetIds[first] == SetIds[second];
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Calculated in constant-time by retrieving the set id at index <paramref name="value"/> in the underlying list.
    /// </remarks>
    public int Find(int value)
    {
        if (ValuesCount == 0)
            throw new InvalidOperationException(
                $"{nameof(AreConnected)} cannot be invoked on an empty queue.");
        if (value < 0 || value >= ValuesCount)
            throw new ArgumentException(
                $"Must be non-negative and smaller than {nameof(ValuesCount)}.", nameof(value));

        return SetIds[value];
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Retrieves the set id A of <paramref name="first"/> and B of <paramref name="second"/>, via <see cref="Find"/>.
    /// <br/>
    /// Then replaces all occurrences of B with A, in the underlying list.
    /// <br/>
    /// Time Complexity is O(n) and Space Complexity is O(1).
    /// </remarks>
    public void Union(int first, int second)
    {
        var firstSetId = Find(first);
        var secondSetId = Find(second);

        if (firstSetId == secondSetId)
            return;

        for (var i = 0; i < ValuesCount; i++)
            if (SetIds[i] == secondSetId)
                SetIds[i] = firstSetId;
    }
}
