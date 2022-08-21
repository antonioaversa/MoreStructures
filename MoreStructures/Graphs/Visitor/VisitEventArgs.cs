namespace MoreStructures.Graphs.Visitor;

/// <summary>
/// An object carrying information about the visit of a vertex of a graph being visited by a 
/// <see cref="IVisitStrategy"/>.
/// </summary>
/// <param name="Vertex">The vertex being visited.</param>
public record VisitEventArgs(int Vertex);
