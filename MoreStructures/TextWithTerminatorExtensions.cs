using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    ///     <para>
    ///     The rotation is "virtual" because no new string of length n is computed (which would make the constructor 
    ///     take linear time in the number of chars of <paramref name="text"/>).
    ///     </para>
    ///     <para>
    ///     Instead, a new object storing the rotation and keeping the reference to <paramref name="text"/> is created
    ///     in O(1) time and space.
    ///     Such an object is able to appear as if the underlying string was recomputed, taking into account the 
    ///     rotation in all its exposed functionalities.
    ///     </para>
    /// </remarks>
    public static VirtuallyRotatedTextWithTerminator ToVirtuallyRotated(this TextWithTerminator text, int rotation) =>
        new(new(text.Text + text.Terminator), rotation);
}
