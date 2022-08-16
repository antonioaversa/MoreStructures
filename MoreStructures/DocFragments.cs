namespace MoreStructures;

/// <summary>
/// Class used to share and reuse documentation sentences and paragraphs among different, potentially uncorrelated 
/// functionalities.
/// </summary>
/// <remarks>
///     <para id="fully-iterative-advantages">
///     Implemented fully iteratively via a <see cref="Stack{T}"/>, so not limited by call stack depth but rather by 
///     the maximum size of the stack stored in the heap.
///     <br/>
///     Convenient with deep trees (i.e. trees having a height &gt; ~1K nodes) or graphs (i.e. graphs having a size
///     &gt; ~1K nodes).
///     </para>
/// </remarks>
internal static class DocFragments
{
}
