using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreStructures.BurrowsWheelerTransform.Builder;

/// <summary>
/// Builds Burrows-Wheeler objects, such as <see cref="BWMatrix"/> and <see cref="BWTransform"/> of the provided
/// <see cref="TextWithTerminator"/>.
/// </summary>
public interface IBuilder
{
    /// <summary>
    /// Build a <see cref="BWMatrix"/> of the provided text, which is a 
    /// n-ary search tree in which edges coming out of a node are substrings of text which identify edges shared by all 
    /// paths to leaves, starting from the node.
    /// </summary>
    /// <param name="text">
    /// The text to build the BWM, with its terminator (required).
    /// </param>
    /// <returns>The matrix, wrapped into a <see cref="BWMatrix"/> object.</returns>
    /// <remarks>
    /// <inheritdoc cref="BWMatrix" path="/summary"/>
    /// </remarks>
    /// <example>
    /// Code: 
    /// <code>
    /// var builder = ...
    /// builder.BuildMatrix(new("mississippi")).Content
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
    BWMatrix BuildMatrix(TextWithTerminator text);

    /// <summary>
    /// Builds the Barrows-Wheeler Transform from the provided <see cref="BWMatrix"/>.
    /// </summary>
    /// <remarks>
    /// <inheritdoc cref="BWTransform" path="/summary"/>
    /// </remarks>
    BWTransform BuildTransform(BWMatrix matrix);

    /// <summary>
    /// Builds the Barrows-Wheeler Transform from the provided <see cref="TextWithTerminator"/>.
    /// </summary>
    /// <remarks>
    /// <inheritdoc cref="BWTransform" path="/summary"/>
    /// </remarks>
    BWTransform BuildTransform(TextWithTerminator text);
}

