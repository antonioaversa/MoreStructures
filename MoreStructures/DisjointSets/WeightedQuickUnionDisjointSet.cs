using MoreStructures.Utilities;

namespace MoreStructures.DisjointSets;

/// <summary>
/// A <see cref="QuickUnionDisjointSet"/> refinement which improves the performance of Find by minimizing the height of
/// trees in the forest when merging them in <see cref="Union"/>.
/// </summary>
/// <remarks>
/// By merging by height, the increment of height of the trees in the forest when merging is minimized, and the forest
/// is kept as flat as possible after the <see cref="Union(int, int)"/> operation.
/// <br/>
/// Keeping trees flat is the way to ensure sub-linear, Time Complexity in 
/// <see cref="QuickUnionDisjointSet.AreConnected(int, int)"/> and <see cref="QuickUnionDisjointSet.Find(int)"/>, which
/// in the specific case become logarithmic in time.
/// </remarks>
public class WeightedQuickUnionDisjointSet : QuickUnionDisjointSet
{
    private readonly int[] _heights;

    /// <inheritdoc path="//*[not(self::summary)]"/>
    /// <summary>
    /// <inheritdoc/>
    /// <br/>
    /// Initializes the heights of all singleton trees to 0.
    /// </summary>
    public WeightedQuickUnionDisjointSet(int valuesCount) : base(valuesCount)
    {
        _heights = new int[valuesCount];
    }

    /// <summary>
    /// Returns a copy of the heights of the trees of the forest representing the Disjoint Set.
    /// </summary>
    internal IList<int> GetHeights() => _heights.ToArray();

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - It first finds the root R1 of <paramref name="first"/> and the root R2 of <paramref name="second"/>, by
    ///       running <see cref="QuickUnionDisjointSet.Find(int)"/> on each item.
    ///       <br/>
    ///     - Then, it checks the height of R1, H1 and R2, H2 and uses them to decide whether to attach R1 to R2 or
    ///       viceversa.
    ///       <br/>
    ///     - If <c>H1 >= H2</c>, it attaches R2 as immediate child of R1, by setting the parent of R2 to R1.
    ///       <br/>
    ///     - Otherwise, it attaches R1 as immediate child of R2, by setting the parent of R1 to R2.
    ///       <br/>
    ///     - This way the two three now are merged into a single one, which means that all items of the two trees, 
    ///       including <paramref name="first"/> and <paramref name="second"/> are now in the same set.
    ///       <br/>
    ///     - Moreover, by merging based on heights H1 and H2, the height of the resulting tree is minimized.
    ///       <br/>
    ///     - Finally the height of the new root (R1 or R2), is updated, to reflect the merge.
    ///     </para> 
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Find the two roots takes a time proportional to the height of the two trees.
    ///       <br/>
    ///     - Thanks to the "merging by height", unlike in <see cref="QuickUnionDisjointSet"/>, the height of each tree
    ///       in the forest is not O(n) in the worst case, but O(log(n)).
    ///       <br/>
    ///     - Attaching one root as child of the other is a constant-time operation, since it only requires setting the
    ///       parent in the list of parents, which is O(1) work.
    ///       <br/>
    ///     - Updating the height of the root of the merged tree is also a constant-time operation.
    ///       <br/>
    ///     - Therefore, Time Complexity is O(log(n)). Space Complexity is O(1).
    ///     </para>
    /// </remarks>
    public override void Union(int first, int second)
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

        var firstRoot = Find(first);
        var secondRoot = Find(second);
        if (_heights[firstRoot] >= _heights[secondRoot])
        {
            Parents[secondRoot] = firstRoot;
            _heights[firstRoot] = Math.Max(_heights[firstRoot], _heights[secondRoot] + 1);
        }
        else
        {
            Parents[firstRoot] = secondRoot;
            _heights[secondRoot] = Math.Max(_heights[secondRoot], _heights[firstRoot] + 1);
        }
    }
}
