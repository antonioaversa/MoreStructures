namespace MoreStructures.Graphs;

internal abstract class ReverseProxyGraph<T> : IGraph
    where T : IGraph
{
    protected T Proxied { get; }

    protected ReverseProxyGraph(T proxied)
    {
        Proxied = proxied;
    }

    public abstract IEnumerable<IGraph.Adjacency> GetAdjacentVerticesAndEdges(
        int start, bool takeIntoAccountEdgeDirection);
    public virtual int GetNumberOfVertices() => Proxied.GetNumberOfVertices();
    public virtual IGraph Reverse() => Proxied;
}
