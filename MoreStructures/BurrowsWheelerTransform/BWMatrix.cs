using MoreLinq;
using MoreStructures.Utilities;

namespace MoreStructures.BurrowsWheelerTransform;

/// <summary>
/// Burrows-Wheeler Matrix (BWM) of a <see cref="TextWithTerminator"/> is the square matrix all cyclic rotations of the 
/// provided <see cref="TextWithTerminator"/>, with rows sorted in ascending order and taking into account that 
/// <see cref="TextWithTerminator.Terminator"/> is to be considered smaller than any other char in the text.
/// </summary>
/// <param name="Text">The text, to calculate the BWM of.</param>
/// <remarks>
///     <para id="lazy-initialization">
///     All calculation is performed lazily: not at <see cref="BWMatrix(MoreStructures.TextWithTerminator)"/> 
///     time, but when the <see cref="Content"/> property is accessed the first time. The result is then cached for 
///     following access.
///     </para>
/// </remarks>
public record BWMatrix(TextWithTerminator Text)
{
    private readonly LockValueObject _lockContent = new();
    private readonly IComparer<char> _charComparer = new CharOrTerminatorComparer(Text.Terminator);
    private IList<string>? _content;

    /// <summary>
    /// Builds a Burrows-Wheeler Matrix of the provided <see cref="Text"/> with the precomputed <see cref="Content"/>.
    /// </summary>
    /// <param name="text"><inheritdoc cref="BWMatrix(TextWithTerminator)" path="/param[@name='Text']"/></param>
    /// <param name="content">
    /// A list of strings, each one containing a row of the matrix, i.e. a cyclic rotation of <paramref name="text"/>.
    /// </param>
    /// <remarks>
    /// Useful to avoid repeated computation of <see cref="Content"/>, which is an expensive operation.
    /// </remarks>
    public BWMatrix(TextWithTerminator text, IList<string> content)
        : this(text)
    {
        _content = content.ToValueReadOnlyCollection();
    }

    /// <summary>
    /// The content of the Barrows-Wheeler Matrix (BWM) of <see cref="Text"/>.
    /// </summary>
    /// <returns>
    /// A readonly immutable list of strings, each one containing a row of the matrix, i.e. a cyclic rotation of 
    /// <see cref="Text"/>.
    /// </returns>
    /// <example>
    /// Code: 
    /// <code>
    /// new BWTMatrix(new("mississippi")).Content
    /// </code>
    /// 
    /// Result:
    /// <code>
    /// {
    ///     "$mississippi",
    ///     "i$mississipp",
    ///     "ippi$mississ",
    ///     "issippi$miss",
    ///     "ississippi$m",
    ///     "mississippi$",
    ///     "pi$mississip",
    ///     "ppi$mississi",
    ///     "sippi$missis",
    ///     "sissippi$mis",
    ///     "ssippi$missi",
    ///     "ssissippi$mi",
    /// }
    /// </code>
    /// </example>
    /// <remarks>
    ///     <para>
    ///     Since this operation requires computing a n * n matrix, where n is the 
    ///     <see cref="TextWithTerminator.Length"/> of <see cref="Text"/>, it can be intensive operation, both in 
    ///     time: 
    ///     <list type="bullet">
    ///         <item>
    ///         Sorting a large number of strings on a large non-constant alphabet takes n * log(n) * m, where m is
    ///         the cost of a comparison of two n-sized strings, which is O(n) => Time Complexity is O(n^2 * log(n)). 
    ///         If the alphabet can be considered of constant size and comparison between two strings happens in
    ///         constant time, the complexity is O(n * log(n)).
    ///         </item>
    ///         <item>
    ///         the output is a n * n matrix of chars (all cyclic rotations of a n-sized string): Space Complexity is 
    ///         O(n^2 * m), when no assumption is made on the size of a char being constant, where m = log(w, M), with
    ///         w = size of a word in memory and M = size of the alphabet. If the alphabet can be considered of 
    ///         constant size, the complexity is O(n^2).
    ///         </item>
    ///     </list>
    ///     </para>
    ///     <inheritdoc cref="BWMatrix" path="/remarks/para[@id='lazy-initialization']"/>
    /// </remarks>
    public IList<string> Content
    {
        get
        {
            if (_content == null)
            {
                lock (_lockContent)
                {
                    if (_content == null)
                    {
                        _content = Enumerable
                            .Range(0, Text.Length)
                            .Select(i => new string(Text.Skip(i).Take(Text.Length - i).Concat(Text.Take(i)).ToArray()))
                            .OrderBy(i => i, new StringIncludingTerminatorComparer(Text.Terminator))
                            .ToValueReadOnlyCollection();
                    }
                }
            }
            return _content;
        }
    }

    /// <summary>
    /// Builds the Barrows-Wheeler Transform from this <see cref="BWMatrix"/>, which corresponds to the last column
    /// of the matrix, stored in <see cref="Content"/>.
    /// </summary>
    /// <returns>A string containing the Barrows-Wheeler transform.</returns>
    /// <example>
    ///     Code: 
    ///     <code>
    ///     new BWTMatrix(new("mississippi")).Transform;
    ///     </code>
    ///     
    ///     Result:
    ///     <code>
    ///     "ipssm$pissii"
    ///     </code>
    /// </example>
    /// <remarks>
    /// Requires <see cref="Content"/> calculation.
    /// <inheritdoc cref="BWMatrix" path="/remarks/para[@id='lazy-initialization']"/>
    /// </remarks>
    public string Transform =>
        string.Join(string.Empty, Content.Select(r => r[^1]));

    /// <summary>
    /// Returns the first column of this <see cref="BWMatrix"/>. Corresponds to the sorted <see cref="Text"/> and 
    /// also to the sorted <see cref="Transform"/> of this <see cref="BWMatrix"/>.
    /// </summary>
    /// <example>
    ///     Code: 
    ///     <code>
    ///     new BWTMatrix(new("mississippi")).FirstColumn
    ///     </code>
    ///     
    ///     Result:
    ///     <code>
    ///     "$iiiimppssss"
    ///     </code>
    /// </example>
    /// <remarks>
    /// Unlike <see cref="LastColumn"/> and <see cref="Transform"/>, <see cref="FirstColumn"/> doesn't require 
    /// computation of the <see cref="Content"/> of this <see cref="BWMatrix"/>, since the <see cref="FirstColumn"/>
    /// can easily be calculated by sorting the input <see cref="Text"/>.
    /// </remarks>
    public string FirstColumn =>
        string.Join(string.Empty, Text.OrderBy(c => c, _charComparer));

    /// <summary>
    /// Returns the last column of this <see cref="BWMatrix"/>. Corresponds to the <see cref="Transform"/> of this
    /// <see cref="BWMatrix"/>.
    /// </summary>
    /// <example>
    ///     Code: 
    ///     <code>
    ///     new BWTMatrix(new("mississippi")).LastColumn
    ///     </code>
    ///     
    ///     Result:
    ///     <code>
    ///     "ipssm$pissii"
    ///     </code>
    /// </example>
    /// <remarks>
    /// Requires <see cref="Content"/> calculation.
    /// <inheritdoc cref="BWMatrix" path="/remarks/para[@id='lazy-initialization']"/>
    /// </remarks>
    public string LastColumn => Transform;
}
