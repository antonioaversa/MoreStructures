using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringAlgorithms.SuffixStructures.Transformations;

/// <summary>
/// Exposes utility methods to convert a <see cref="ISuffixStructureNode{TEdge, TNode, TPath, TBuilder}"/> 
/// structure into a readable string.
/// </summary>
/// <remarks>
/// Particularly useful for testing and debugging.
/// </remarks>
public static class SuffixStructureStringifier
{
    /// <summary>
    /// Converts the provided <see cref="ISuffixStructureNode{TEdge, TNode, TPath, TBuilder}"/> into a string.
    /// </summary>
    /// <param name="node">The root of the structure to stringify.</param>
    /// <param name="indentation">The string to be used as indentation.</param>
    /// <returns>A string version of the provided structure.</returns>
    public static string Stringify<TEdge, TNode, TPath, TBuilder>(
        this ISuffixStructureNode<TEdge, TNode, TPath, TBuilder> node,
        string indentation)
        where TEdge : ISuffixStructureEdge<TEdge, TNode, TPath, TBuilder>
        where TNode : ISuffixStructureNode<TEdge, TNode, TPath, TBuilder>
        where TPath : ISuffixStructurePath<TEdge, TNode, TPath, TBuilder>
        where TBuilder : ISuffixStructureBuilder<TEdge, TNode, TPath, TBuilder>, new()
    {
        return string.Empty;
    }
}
