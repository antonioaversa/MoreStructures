namespace MoreStructures.Graphs;

/// <summary>
/// Represents a mapping between edges of a <see cref="IGraph"/> and distances, in a spatial context, or weights, in
/// a more general setting.
/// </summary>
public interface IGraphDistances
{
    /// <summary>
    /// Returns the distance, or weight, of the provided <paramref name="edge"/>.
    /// </summary>
    /// <param name="edge">The edge, to provide the distance of.</param>
    /// <returns>Any positive or negative number.</returns>
    int this[(int edgeStart, int edgeEnd) edge] { get; }
}
