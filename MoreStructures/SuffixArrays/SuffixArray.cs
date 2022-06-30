namespace MoreStructures.SuffixArrays;

/// <summary>
/// The Suffix Array of a terminator-terminated text T is an <see cref="int"/> sequence where the i-th element is the
/// index in T of the 1st char of a suffix Si of T, and Si &lt; Sj for each i &lt; j.
/// </summary>
/// <remarks>
///     Suffixes of the terminator-terminated text "mississippi$" are:
///     <br/>
///     - "mississippi$", at index 0;
///       <br/>
///     - "ississippi$", at index 1;
///       <br/>
///     - "ssissippi$", at index 2;
///       <br/>
///     - etc.
///     <br/>
///     Sorting suffixes in lexicographic order gives:    
///     - "$mississippi", at index 11;
///       <br/>
///     - "i$", at index 10;
///       <br/>
///     - "ippi$", at index 7;
///       <br/>
///     - "issippi$", at index 4;
///       <br/>
///     - etc.
///     <br/>
///     So the Suffix Array of "mississippi$" is { 11, 10, 7, 4, ... }.
/// </remarks>
/// <param name="Indexes">
/// The sequence of indexes, each one marking the beginning of a suffix of the text. Is a <see cref="IEnumerable{T}"/>,
/// rather than an <see cref="IList{T}"/>, so that values can be generated lazily and dynamically.
/// </param>
public record struct SuffixArray(IEnumerable<int> Indexes);
