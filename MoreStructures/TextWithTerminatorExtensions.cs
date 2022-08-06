namespace MoreStructures;

/// <summary>
/// Extension methods for <see cref="TextWithTerminator"/>.
/// </summary>
public static class TextWithTerminatorExtensions
{
    /// <summary>
    /// Builds a virtual rotation of the provided <see cref="TextWithTerminator"/>, by a number of chars defined by
    /// <paramref name="rotation"/>, in constant time.
    /// </summary>
    /// <param name="text">The text which  has to be rotated.</param>
    /// <param name="rotation">The number of chars to virtually rotate <paramref name="text"/>.</param>
    /// <returns>
    /// An object constructed in constant time and behaving like a rotation of the provided text.
    /// </returns>
    /// <remarks>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - The rotation is "virtual" because no new string of length n is computed (which would make the constructor 
    ///       take linear time in the number of chars of <paramref name="text"/>).
    ///       <br/>
    ///     - Instead, a new object storing the rotation and keeping the reference to <paramref name="text"/> is 
    ///       created in O(1) time and space.
    ///       <br/>
    ///     - Such an object is able to appear as if the underlying string was recomputed, taking into account the 
    ///       rotation in all its exposed functionalities.
    ///     </para>
    /// </remarks>
    public static VirtuallyRotatedTextWithTerminator ToVirtuallyRotated(this TextWithTerminator text, int rotation) =>
        // TODO: avoid building RotatedTextWithTerminator and have VirtuallyRotatedTextWithTerminator access directly
        // into the underlying TextWithTerminator
        new(new(text, text.Terminator, false), rotation);

    /// <summary>
    /// Builds a single <see cref="TextWithTerminator"/>, concatenating the <see cref="TextWithTerminator"/> instances
    /// in <paramref name="texts"/>. Returns as well the <see cref="ISet{T}"/> of all the terminators and their 
    /// indexes.
    /// </summary>
    /// <param name="texts">The text instances to join into a single text.</param>
    /// <returns>
    /// A couple. 
    /// <br/>
    /// The first item of the couple is a text with the <see cref="TextWithTerminator.Text"/> of all items of 
    /// <paramref name="texts"/> concatenated, each followed by its own <see cref="TextWithTerminator.Terminator"/> 
    /// except the last, and the <see cref="TextWithTerminator.Terminator"/> of the last item.
    /// <br/>
    /// The second item of the couple is a set of all the terminators included in the resulting text.
    /// </returns>
    /// <remarks>
    /// <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     Requires iterating over the <see cref="TextWithTerminator"/> items of <paramref name="texts"/>, but not on
    ///     their content.
    ///     <br/>
    ///     Therefore, Time Complexity is O(n), where n is the number of items of <paramref name="texts"/>, and not 
    ///     O(t), where t is the length of the concatenated text.
    ///     <br/>
    ///     Space Complexity is also O(n), since the <see cref="ISet{T}"/> of terminators contains n items and the 
    ///     generated full text receives a lazy evaluated <see cref="IEnumerable{T}"/> of the <see cref="char"/> in
    ///     each <see cref="TextWithTerminator"/> of <paramref name="texts"/>.
    /// </para>
    /// </remarks>
    public static (TextWithTerminator fullText, ISet<char> terminators) GenerateFullText(
        this TextWithTerminator[] texts)
    {
        if (texts.Length == 0)
        {
            return (new TextWithTerminator(""), new HashSet<char> { TextWithTerminator.DefaultTerminator });
        }
        
        if (texts.Length == 1)
        {
            var singleText = texts.Single();
            return (singleText, new HashSet<char> { singleText.Terminator });
        }

        var fullTextContent = Enumerable.Empty<char>();
        var terminators = new HashSet<char>();

        var queue = new Queue<TextWithTerminator>();
        foreach (var text in texts)
        {
            if (!terminators.Add(text.Terminator))
                throw new ArgumentException("Terminators should be unique.", nameof(texts));

            queue.Enqueue(text);

            if (queue.Count > 1)
            {
                var previousText = queue.Dequeue();
                fullTextContent = fullTextContent.Concat(previousText);
            }
        }

        var lastText = queue.Dequeue();
        fullTextContent = string.Concat(fullTextContent.Concat(lastText.Text));
        var fullText = new TextWithTerminator(fullTextContent, lastText.Terminator);

        return (fullText, terminators);
    }

    /// <summary>
    /// Builds the Cumulative Distribution Function (CDF) of the provided <paramref name="terminators"/> in the 
    /// provided <paramref name="fullText"/>.
    /// </summary>
    /// <param name="fullText">
    /// The text, composed of a single or multiple concatenated <see cref="TextWithTerminator"/>.
    /// </param>
    /// <param name="terminators">
    /// The set of terminators of <paramref name="fullText"/>.
    /// </param>
    /// <returns>
    /// A lazy-generated sequence of integers, representing the values of the CDF indexed by char index in 
    /// <paramref name="fullText"/>.
    /// </returns>
    /// <remarks>
    ///     <para id="definition">
    ///     DEFINITION
    ///     <br/>
    ///     - The Cumulative Distribution Function of a text T with terminators t is a function CDF such that CDF at 
    ///       index i is the number of chars in T up to index i included which are in t.
    ///       <br/>
    ///     - In other terms, <c>CDF[i] = sum(j = 0 to i, isTerminator(T[j]))</c>, where 
    ///       <c>isTerminator(c) = 1 if c is in t, 0 otherwise</c>.
    ///     </para>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - The definition is applied, iterating over the chars of <paramref name="fullText"/> and yielding item by 
    ///       item.
    ///       <br/>
    ///     - The algorithm is online.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Time Complexity is O(n * Ttc), where n is the length of <paramref name="fullText"/> and Ttc is the time
    ///       required to check whether a char of the text is a terminator or not.
    ///       <br/>
    ///     - The "terminator checking" performance depends on the implementation of <see cref="ISet{T}"/>,
    ///       <paramref name="terminators"/> is an instance of.
    ///       <br/>
    ///     - If <paramref name="terminators"/> is an <see cref="HashSet{T}"/>, 
    ///       <see cref="ICollection{T}.Contains(T)"/> is executed in constant time, and Time Complexity becomes O(n).
    ///       <br/>
    ///     - Space Complexity is O(n) in all cases, since the only data structure instantiated by the algorithm is the
    ///       output, which has as many items as the input text.
    ///     </para>
    /// </remarks>
    public static IEnumerable<int> BuildTerminatorsCDF(TextWithTerminator fullText, ISet<char> terminators)
    {
        var numberOfTerminators = 0;
        for (var i = 0; i < fullText.Length; i++)
        {
            if (terminators.Contains(fullText[i]))
                numberOfTerminators++;
            yield return numberOfTerminators;
        }
    }
}
