namespace MoreStructures.Graphs;

/// <summary>
/// A <see cref="IGraphDistances"/> retrieving distances from a <see cref="IDictionary{TKey, TValue}"/>, mapping
/// couples of <see cref="int"/> values (ids of endpoints of each edge of the graph) to <see cref="int"/> values
/// (edge distances).
/// </summary>
public class DictionaryAdapterGraphDistances : IGraphDistances
{
    private IDictionary<(int, int), int> Dictionary { get; }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Retrieves the value from the underlying dictionary.
    /// </remarks>
    public int this[(int edgeStart, int edgeEnd) edge] => Dictionary[edge];

    /// <summary>
    ///     <inheritdoc cref="DictionaryAdapterGraphDistances"/>
    /// </summary>
    /// <param name="dictionary">The mapping between edges and distances.</param>
    public DictionaryAdapterGraphDistances(IDictionary<(int, int), int> dictionary)
    {
        Dictionary = dictionary;
    }
}
