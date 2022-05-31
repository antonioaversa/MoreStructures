# More Structures .NET API Documentation
This is the entrypoint of the .NET API documentation of More Structures.

More Structures provides:
- **mostly basic, and some more advanced, data structures** such as Trees, Tries, Burrows-Wheeler Transform and Matrix wrappers etc.
- **implementation of well-known algorithms** running on them, such as Suffix Trie and Tree construction, Hukkonen algorithm, Burrows-Wheeler Transform pattern matching, etc.

## Functionalities provided
- **Burrows-Wheeler Transform and Matrix**: builders, inversion, pattern matching
- **Recursively Defined Trees**: descendants counting, stringification
- **Lists**: binary search
- **Suffix Tries**: builders, pattern matching, conversion into trees
- **Suffix Trees**: builders, pattern matching, conversion into tries, Hukkonen algorithm for linear time building

## Design aspects
The library is conceived with a few key aspects in mind:
- type-safety
- immutability
- value-based semantics
- performance
- 100% statement and condition coverage
- extensive auto-generated documentation
- warnings as errors

### Type-safety
All concepts have wrappers around standard types provided by the framework, enforcing data structure-specific constraints: 

For example, all terminator-terminated strings used by Suffix Trees and Tries are mapped to a specific `record` named `TextWithTerminator`, which ensures that the string is properly formed. Similarly, all cyclic rotations of a text are wrapped into a `RotatedTextWithTerminator`.

This approch allows to avoid in most cases dangerous casting operations, sometimes leading to runtime errors. Type-safety is preserved by generic constraints whenever possible.

For example, by binding together interfaces of different objects of the same data structure by some variants of the [Curiously recurring template pattern](https://en.wikipedia.org/wiki/Curiously_recurring_template_pattern):

```cs
public interface IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
{
}

public interface IRecImmDictIndexedTreeNode<TEdge, TNode>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
{
    IDictionary<TEdge, TNode> Children { get; }
}
```

The notation can be heavy at times, especially when defining new data structures implementing such interfaces.

```cs
public record SuffixTreeEdge(int Start, int Length)
    : ISuffixStructureEdge<SuffixTreeEdge, SuffixTreeNode>, IComparable<SuffixTreeEdge>
{
    ...
}

public abstract record SuffixTreeNode(IDictionary<SuffixTreeEdge, SuffixTreeNode> Children, int? Start)
    : ISuffixStructureNode<SuffixTreeEdge, SuffixTreeNode>
{
    ...
}

```

However, the advantages of type-safety more often than not outrun the disadvantages, keeping data structures coherent and predictable at compile time.

Another typical source of runtime errors are null reference exceptions. These are avoided by:
- requiring use of *nullable reference types*, introduced in C# 8, everywhere in the project;
- limiting to the maximum extent possible the use of *null-forgiving operator*;
- avoiding the semantic abuse of `null`.

### Immutability
Whenever possible, structures are kept immutable, meaning that, whenever possible:
- fields are made `readonly`, and either assigned in-line or in the constructor;
- properties are made either `{ get; }` or `{ get; init; }`, and either assigned in-line or in the constructor;
- methods don't change object state;
- complex object initialization which may require multiple steps and intermediate incoherent states are externalized to a *Builder* object. 

### Value-based semantics
C# 9 introduced `records` which are `classes` or `struct` with *value-based equality*.

This library makes extensive use of such construct, especially when wrapping other objects, in order to give them a domain-specific semantics:

```cs
public record TextWithTerminator(
    IEnumerable<char> Text, 
    char Terminator = TextWithTerminator.DefaultTerminator,
    bool ValidateInput = true)
    : IValueEnumerable<char>
    ...

public record BWTransform(TextWithTerminator Text, RotatedTextWithTerminator Content)
    ...

public record BWMatrix(TextWithTerminator Text, IList<string> Content)
    ...

public sealed record CountTreeEdge<TEdge, TNode>(TEdge WrappedEdge) 
    : IRecImmDictIndexedTreeEdge<CountTreeEdge<TEdge, TNode>, CountTreeNode<TEdge, TNode>>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
    ...
```

### Performance
While type soundness has the priority in the design of this library, performance is measured and optimized.

Due to lack of proper tail-recurse optimization in the .NET runtime, all operations on deep recursive structures are implemented in at least two variants:
- a fully recursive, typically functional fashion, where no mutation of any type (not even local) happens;
- an iterative, `Stack` or `Queue` based, where some local mutations are allowed;
- sometimes the two approaches are combined.

### 100% statement and condition coverage
The code is currently covered by tests in its entirety. There is mostly a 1-to-1 mapping between every public class and method, and their corresponding test class and method in the `MoreStructures.Test` project.
  - Tests are a great source of documentation, showing working and reproducible examples of how to use this library. When looking for directions about how to use a data structure or invoke an algorithm, it's highly recommended to also have a look at unit tests covering that functionality. 

### Extensive auto-generated documentation
All public members and their parameters and generic types are documented.

## Warnings as errors
