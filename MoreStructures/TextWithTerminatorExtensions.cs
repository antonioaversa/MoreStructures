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
    /// in <paramref name="texts"/>. Returns as well the <see cref="ISet{T}"/> of all the terminators and their indexes.
    /// </summary>
    /// <param name="texts">The text instances to join into a single text.</param>
    /// <returns>
    /// A text with the <see cref="TextWithTerminator.Text"/> of all items of <paramref name="texts"/> concatenated, 
    /// each followed by its own <see cref="TextWithTerminator.Terminator"/> except the last, and the 
    /// <see cref="TextWithTerminator.Terminator"/> of the last item.
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
}
