using MoreStructures.SuffixTrees;

namespace MoreStructures.SuffixArrays;

/// <summary>
/// A Partial <see cref="SuffixArray"/> which only contains the indexes of the Suffix Array which are multiple of a
/// given constant <see cref="K"/> (0, k, 2k, ...), indexed by their position in the complete Suffix Array.
/// </summary>
/// <param name="K">
/// The constant, whose multiples (0, k, 2k, ...) are the indexes of the Suffix Array stored in 
/// <paramref name="Indexes"/>. A <see cref="SuffixArray"/> corresponds to a <see cref="IndexModKPartialSuffixArray"/>
/// with <paramref name="K"/> equal to 1.
/// </param>
/// <param name="Indexes">The indexes multiple of <paramref name="K"/>.</param>
/// <remarks>
/// <see cref="SuffixArray"/> is a space-efficient alternative to <see cref="SuffixTreeNode"/> structures, due to 
/// their Θ(n) with c=1 space used, when the size of an index can be considered constant w.r.t. the size of the input 
/// strings.
/// <br/>
/// There are, however, cases where even the linear Space Complexity of a <see cref="SuffixArray"/> is considered still
/// too high, and a smaller data structure has to be stored, potentially at the cost of the algorithm runtime.
/// <br/>
/// These are the scenarios where a Partial Suffix Array is used.
/// <br/>
/// Text pattern matching with Suffix Arrays can also be done with partial structures, for example by using the
/// Last-First property of the Burrows-Wheeler Transform and its sorted version.
/// </remarks>
public record struct IndexModKPartialSuffixArray(int K, IDictionary<int, int> Indexes);