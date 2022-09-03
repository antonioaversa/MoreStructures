namespace MoreStructures.PriorityQueues;

/// <summary>
/// An item of type <typeparamref name="T"/> with a priority assigned to it.
/// </summary>
/// <typeparam name="T">The type of the <paramref name="Item"/>.</typeparam>
/// <param name="Item">The item.</param>
/// <param name="Priority">The <see cref="int"/> defining the priority of the <paramref name="Item"/>.</param>
public record struct ItemAndPriority<T>(T Item, int Priority);
