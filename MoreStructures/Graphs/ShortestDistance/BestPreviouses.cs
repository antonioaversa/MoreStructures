namespace MoreStructures.Graphs.ShortestDistance;

/// <summary>
/// A collection of <see cref="BestPrevious"/> estimation, indexed by vertex id.
/// </summary>
/// <param name="Values">The mapping between vertices id and best previous estimations.</param>
/// <remarks>
///     <inheritdoc cref="BestPrevious"/>
/// </remarks>
public record BestPreviouses(Dictionary<int, BestPrevious> Values);
