namespace MoreStructures.Graphs.ShortestDistance;

/// <summary>
/// An <see cref="IPotentialBasedShortestDistanceFinder"/> implementation, wrapping a 
/// <see cref="IShortestDistanceFinder"/> algorithm and using a potential function as a heuristic to enhance graph 
/// exploration.
/// </summary>
/// <remarks>
/// Common ground for all A* variants, such as <see cref="AStarShortestDistanceFinder"/> and 
/// <see cref="BidirectionalAStarShortestDistanceFinder"/>.
/// <br/>
/// Check <see cref="IPotentialBasedShortestDistanceFinder"/> general documentation for the requirements and desired 
/// properties for the heuristic.
/// </remarks>
public abstract class PotentialBasedShortestDistanceFinder : IPotentialBasedShortestDistanceFinder
{
    /// <summary>
    /// A <see cref="IShortestDistanceFinder"/> instance, used to run the shortest distance algorithm on the provided
    /// graph.
    /// </summary>
    protected IShortestDistanceFinder Finder { get; }

    /// <inheritdoc cref="PotentialBasedShortestDistanceFinder"/>
    /// <param name="finder">
    ///     <inheritdoc cref="Finder" path="*"/>
    /// </param>
    protected PotentialBasedShortestDistanceFinder(IShortestDistanceFinder finder) 
    {
        Finder = finder;
    }

    /// <inheritdoc/>
    public (int, IList<int>) Find(IGraph graph, IGraphDistances distances, int start, int end) => 
        Find(graph, distances, v => 0, start, end);

    /// <inheritdoc/>
    public (int, IList<int>) Find(
        IGraph graph, IGraphDistances distances, Func<int, int> potentials, int start, int end)
    {
        var alteredDistances = new PotentialFunctionAlteredGraphDistances(distances, potentials);
        var (alteredDistance, shortestDistancePath) = Finder.Find(graph, alteredDistances, start, end);
        var actualDistance = alteredDistance == int.MaxValue || alteredDistance == int.MinValue 
            ? alteredDistance 
            : alteredDistance + potentials(start) - potentials(end);
        return (actualDistance, shortestDistancePath);
    }

    private sealed class PotentialFunctionAlteredGraphDistances : IGraphDistances
    {
        private IGraphDistances OriginalDistances { get; }
        private Func<int, int> Potentials { get; }

        public PotentialFunctionAlteredGraphDistances(
            IGraphDistances originalDistances, Func<int, int> potentials)
        {
            OriginalDistances = originalDistances;
            Potentials = potentials;
        }

        public int this[(int edgeStart, int edgeEnd) edge] => 
            OriginalDistances[edge] - Potentials(edge.edgeStart) + Potentials(edge.edgeEnd);
    }

}
