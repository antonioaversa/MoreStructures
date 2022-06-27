using MoreStructures.SuffixStructures;

namespace MoreStructures.SuffixArrays.Builders;

/// <summary>
/// An algorithm for building Suffix Arrays.
/// </summary>
/// <remarks>
///     <para id="definition">
///     The Suffix Array of a terminator-terminated text T is an array where the i-th element is the index in T of the 
///     1st char of a suffix Si of T, and Si &lt; Sj for each i &lt; j.
///     <br/>
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
///     </para>
/// </remarks>
/// <example>
///     <code>
///     TextWithTerminator text = new("mississippi");
///     ISuffixArrayBuilder suffixArrayBuilder = ... // Some implementation of ISuffixArrayBuilder
///     var suffixArray = suffixArrayBuilder.Build().ToList(); // ToList necessary if Build is lazy
///     // suffixArray is new List&lt;int&gt; { 11, 10, 7, 4, ... }
///     </code>
/// </example>
public interface ISuffixArrayBuilder
{
    /// <summary>
    /// Builds the Suffix Array.
    /// </summary>
    /// <returns>A <see cref="IEnumerable{T}"/> of <see cref="int"/>.</returns>
    IEnumerable<int> Build();
}
