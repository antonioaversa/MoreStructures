using MoreStructures.Utilities;

namespace MoreStructures;

/// <summary>
/// A text string with a terminator character which has been rotated leftwards or rightwards, of a number of positions 
/// (0 included).
/// </summary>
/// <param name="Underlying">The <see cref="TextWithTerminator"/> instance which has been rotated.</param>
/// <param name="Rotation">
/// The number of characters to rotate: positive = rightwards, negative = leftwards.
/// </param>
/// <remarks>
/// A virtually rotated terminator-terminated text is required by Burrows-Wheeler Transform construction, when the 
/// length of the text is too high to build the Burrows-Wheeler Matrix, which would have n^2 items.
/// </remarks>
public record VirtuallyRotatedTextWithTerminator(RotatedTextWithTerminator Underlying, int Rotation) 
    : IValueEnumerable<char>, IComparable<VirtuallyRotatedTextWithTerminator>
{
    // CharOrTerminatorComparer is a record, so compared by value
    private readonly IComparer<char> _charsComparer = CharOrTerminatorComparer.Build(Underlying.Terminator);

    private sealed class Enumerator : IEnumerator<char>
    {
        private readonly RotatedTextWithTerminator _underlying;
        private readonly int _rotation;
        private int _current;

        public Enumerator(RotatedTextWithTerminator underlying, int rotation)
        {
            _underlying = underlying;
            _rotation = rotation;
            Reset();
        }

        public char Current
        {
            get 
            {
                if (_current < 0)
                    throw new InvalidOperationException($"Enumeration has not started. Call {nameof(MoveNext)}.");
                if (_current >= _underlying.Length)
                    throw new InvalidOperationException("Enumeration already finished");
                var index = (_current - _rotation) % _underlying.Length;
                return _underlying[index >= 0 ? index : _underlying.Length + index];
            }
        }

        object System.Collections.IEnumerator.Current => Current;

        public void Dispose() 
        {
            // Nothing, for the time being
        }
        public bool MoveNext() => ++_current < _underlying.Length;
        public void Reset() => _current = -1;
    }

    /// <summary>
    /// Select a part of <see cref="Underlying"/> by the provided index (either w.r.t. the start or to the end of the 
    /// text), applying the <see cref="Rotation"/>. Treat <paramref name="index"/> as circular, over modulo the length
    /// of <see cref="Underlying"/>.
    /// </summary>
    /// <param name="index">The index applied to the underlying string.</param>
    /// <returns>A char containing the selected part.</returns>
    public char this[Index index]
    {
        get
        {
            var length = Underlying.Length;
            var rotatedIndex = (index.GetOffset(length) - Rotation) % length;
            return Underlying[rotatedIndex >= 0 ? rotatedIndex : length + rotatedIndex];
        }
    }

    /// <inheritdoc/>
    public IEnumerator<char> GetEnumerator() => new Enumerator(Underlying, Rotation);

    /// <inheritdoc/>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc/>
    public int CompareTo(VirtuallyRotatedTextWithTerminator? other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other), "Cannot compare to null.");

        using var enumerator = GetEnumerator();
        using var otherEnumerator = other.GetEnumerator();

        var moveNext = enumerator.MoveNext();
        var otherMoveNext = otherEnumerator.MoveNext();

        while (moveNext && otherMoveNext && enumerator.Current == otherEnumerator.Current)
        {
            moveNext = enumerator.MoveNext();
            otherMoveNext = otherEnumerator.MoveNext();
        }

        if (moveNext && !otherMoveNext) return 1;
        if (!moveNext && otherMoveNext) return -1;
        if (moveNext) return _charsComparer.Compare(enumerator.Current, otherEnumerator.Current);
        return 0;
    }
}
