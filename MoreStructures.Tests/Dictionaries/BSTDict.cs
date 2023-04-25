using MoreStructures.Dictionaries;

namespace MoreStructures.Tests.Dictionaries;

/// <summary>
/// A <see cref="IDictionary{TKey, TValue}"/> implementation based on a Binary Search Tree.
/// </summary>
public class BSTDict<TKey, TValue> : IDict<TKey, TValue>
    where TKey : notnull, IComparable<TKey>
{
    private sealed record Node(TKey Key, TValue Value, Node? Left, Node? Right)
    {
        public int Count { get; } = Left?.Count ?? 0 + Right?.Count ?? 0 + 1;
    }

    private Node? Root { get; set; } = null;

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Retrieved from the count stored on the root of the BST.
    /// <br/>
    /// Time and Space Complexity are O(1).
    /// </remarks>
    public int Count => Root?.Count ?? 0;

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Both retrieval and insertion are done by traversing the tree and looking for the node with key equal to the 
    /// provided key.
    /// <br/>
    /// Retrieval just returns the value of the node, if found, whereas insertion changes the value in the tree by
    /// replacing the node or adding a new one.
    /// <br/>
    /// Time Complexity is O(h), where h is the height of the tree. Space Complexity is O(1).
    /// </remarks>
    public TValue this[TKey key] 
    {
        get
        {
            return Retrieve(Root);

            TValue Retrieve(Node? node) => node?.Key.CompareTo(key) switch
            {
                null => throw new KeyNotFoundException($"Couldn't find key '{key}' in the dictionary."),
                0 => node.Value,
                > 0 => Retrieve(node.Left),
                _ => Retrieve(node.Right),
            };
        }
        set 
        {
            Root = InsertOrUpdate(Root);

            Node? InsertOrUpdate(Node? node) => node?.Key.CompareTo(key) switch
            {
                null => new(key, value, null, null),
                0 => new(key, value, node.Left, node.Right),
                > 0 => new(node.Key, node.Value, InsertOrUpdate(node.Left), node.Right),
                _ => new(node.Key, node.Value, node.Left, InsertOrUpdate(node.Right)),
            };
        } 
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Performs an in-order traversal of tree, yielding all the node keys.
    /// <br/>
    /// In this implementation, the order of <see cref="Keys"/> is consistent with the order of <see cref="Values"/>.
    /// <br/>
    /// Time Complexity is O(n), when enumerated, where n is the number of items in the dictionary.
    /// <br/>
    /// Space Complexity is O(1).
    /// </remarks>
    public IEnumerable<TKey> Keys
    {
        get 
        {
            return InOrderTraversal(Root);

            static IEnumerable<TKey> InOrderTraversal(Node? node)
            {
                if (node == null) yield break;
                InOrderTraversal(node.Left);
                yield return node.Key;
                InOrderTraversal(node.Right);
            }
        }
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Performs an in-order traversal of tree, yielding all the node values.
    /// <br/>
    /// In this implementation, the order of <see cref="Keys"/> is consistent with the order of <see cref="Values"/>.
    /// <br/>
    /// Time Complexity is O(n), when enumerated, where n is the number of items in the dictionary.
    /// <br/>
    /// Space Complexity is O(1).
    /// </remarks>
    public IEnumerable<TValue> Values
    {
        get
        {
            return InOrderTraversal(Root);

            static IEnumerable<TValue> InOrderTraversal(Node? node)
            {
                if (node == null) yield break;
                InOrderTraversal(node.Left);
                yield return node.Value;
                InOrderTraversal(node.Right);
            }
        }
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Insertion is done by traversing the tree and looking for a node with key equal to the provided key, as in
    /// the setter of <see cref="this[TKey]"/>.
    /// <br/>
    /// An <see cref="ArgumentException"/> is raised when an item with the same key as the one provided already exists
    /// in the dictionary.
    /// <br/>
    /// Time Complexity is O(h), where h is the height of the tree. Space Complexity is O(1).
    /// </remarks>
    public void Add(TKey key, TValue value)
    {
        Root = Insert(Root);

        Node? Insert(Node? node) => node?.Key.CompareTo(key) switch
        {
            null => new(key, value, null, null, 1),
            0 => throw new ArgumentException($"An item with the key '{key}' already exists in the dictionary."),
            > 0 => Insert(node.Left),
            _ => Insert(node.Right),
        };
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// As in value retrieval, a traversal of the tree is done, looking for the node with key equal to the 
    /// provided key.
    /// <br/>
    /// Time Complexity is O(h), where h is the height of the tree. Space Complexity is O(1).
    /// </remarks>
    public bool ContainsKey(TKey key)
    {
        return Find(Root);

        bool Find(Node? node) => node?.Key.CompareTo(key) switch
        {
            null => false,
            0 => true,
            > 0 => Find(node.Left),
            _ => Find(node.Right),
        };
    }

    public TValue? Remove(TKey key)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Retrieval is done by traversing the tree and looking for a node with key equal to the provided key, as in
    /// the getter of <see cref="this[TKey]"/>.
    /// <br/>
    /// <see langword="false"/> is returned if such a mapping is not found, and <paramref name="value"/> is set to
    /// the <see langword="default"/> value for <typeparamref name="TValue"/>.
    /// <br/>
    /// Time Complexity is O(h), where h is the height of the tree. Space Complexity is O(1).
    /// </remarks>
    public bool TryGetValue(TKey key, out TValue? value)
    {
        var (found, valueFound) = Find(Root);
        value = found ? valueFound : default;
        return found;

        (bool, TValue?) Find(Node? node) => node?.Key.CompareTo(key) switch
        {
            null => (false, default),
            0 => (true, node.Value),
            > 0 => Find(node.Left),
            _ => Find(node.Right),
        };
    }
}

