namespace MoreStructures.Dictionaries;

/// <summary>
/// A <see cref="IDict{TKey, TValue}"/> implementation based on a Binary Search Tree.
/// </summary>
public class BSTDict<TKey, TValue> : IDict<TKey, TValue>
    where TKey : notnull, IComparable<TKey>
{
    private sealed class Node
    {
        private Node? _left = null;
        private Node? _right = null;
        private int _count = 1;

        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public Node? Left { get => _left; set { _left = value; UpdateCount(); } }
        public Node? Right { get => _right; set { _right = value; UpdateCount(); } }
        public int Count => _count;

        public Node(TKey key, TValue value, BSTDict<TKey, TValue>.Node? left, BSTDict<TKey, TValue>.Node? right)
        {
            Key = key;
            Value = value;
            Left = left;
            Right = right;
        }

        private void UpdateCount()
        {
            _count = (_left?.Count ?? 0) + (_right?.Count ?? 0) + 1;
        }

        public Node Mutate(Action<Node> mutation)
        {
            mutation(this);
            return this;
        }
    }

    private static (bool found, TValue? valueFound) Find(Node? node, TKey key) =>
        node?.Key.CompareTo(key) switch
        {
            null => (false, default),
            0 => (true, node.Value),
            > 0 => Find(node.Left, key),
            _ => Find(node.Right, key),
        };

    private static IEnumerable<T> InOrderTraversal<T>(Node? node, Func<Node, T> valueProvider)
    {
        if (node == null)
            yield break;

        foreach (var leftDescendant in InOrderTraversal(node.Left, valueProvider))
            yield return leftDescendant;

        yield return valueProvider(node);

        foreach (var rightDescendant in InOrderTraversal(node.Right, valueProvider))
            yield return rightDescendant;
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
    /// Both retrieval and insertion are done by traversing the tree from its root downwards, and looking for the node 
    /// with key equal to the provided <paramref name="key"/>.
    /// <br/>
    /// Retrieval just returns the value of the node, if found, raising <see cref="KeyNotFoundException"/> otherwise.
    /// <br/>
    /// Insertion changes the value in the tree by replacing the node or adding a new one, also replacing all the nodes
    /// in the path from the root to the insertion/update point.
    /// <br/>
    /// Time Complexity is O(h), where h is the height of the tree. Space Complexity is O(1).
    /// </remarks>
    public TValue this[TKey key] 
    {
        get
        {
            var (found, valueFound) = Find(Root, key);
            if (!found)
                throw new KeyNotFoundException($"Couldn't find key '{key}' in the dictionary.");
            return valueFound!;
        }
        set 
        {
            Root = InsertOrUpdate(Root);

            Node? InsertOrUpdate(Node? node) => node?.Key.CompareTo(key) switch
            {
                null => new(key, value, null, null),
                0 => node.Mutate(n => n.Value = value),
                > 0 => node.Mutate(n => n.Left = InsertOrUpdate(node.Left)),
                _ => node.Mutate(n => n.Right = InsertOrUpdate(node.Right)),
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
    /// Space Complexity is O(1). Values are streamed to the client.
    /// </remarks>
    public IEnumerable<TKey> Keys => InOrderTraversal(Root, node => node.Key);

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Performs an in-order traversal of tree, yielding all the node values.
    /// <br/>
    /// In this implementation, the order of <see cref="Keys"/> is consistent with the order of <see cref="Values"/>.
    /// <br/>
    /// Time Complexity is O(n), when enumerated, where n is the number of items in the dictionary.
    /// <br/>
    /// Space Complexity is O(1). Values are streamed to the client.
    /// </remarks>
    public IEnumerable<TValue> Values => InOrderTraversal(Root, node => node.Value);

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Insertion is done by traversing the tree and looking for a node with key equal to the provided key, as in
    /// the setter of <see cref="this[TKey]"/>.
    /// <br/>
    /// Insertion replaces all nodes in the path from the root to the insertion point, recalculating summarization the
    /// properties kept in tree nodes, such as the count of nodes in the subtree rooted at the node.
    /// <br/>
    /// An <see cref="ArgumentException"/> is raised when an item with the same key as the one provided already exists
    /// in the dictionary, since key duplications is not allowed in <see cref="IDict{TKey, TValue}"/> implementations.
    /// <br/>
    /// Time Complexity is O(h), where h is the height of the tree. Space Complexity is O(1).
    /// </remarks>
    public void Add(TKey key, TValue value)
    {
        Root = Insert(Root);
         
        Node? Insert(Node? node) => node?.Key.CompareTo(key) switch
        {
            null => new(key, value, null, null),
            0 => throw new ArgumentException($"An item with the key '{key}' already exists in the dictionary."),
            > 0 => node.Mutate(n => n.Left = Insert(node.Left)),
            _ => node.Mutate(n => n.Right = Insert(node.Right)),
        };
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// As in value retrieval, a traversal of the tree is done, looking for the node with key equal to the 
    /// provided key.
    /// <br/>
    /// Time Complexity is O(h), where h is the height of the tree. Space Complexity is O(1).
    /// </remarks>
    public bool ContainsKey(TKey key) => Find(Root, key).found;

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// Performs Hibbard deletion:
    /// <br/>
    /// - first the node v, associated with the given <paramref name="key"/>, and its parent p, are retrieved from the 
    ///   tree;
    ///   <br/>
    /// - if v is not found, the deletion cannot be performed and the <see langword="default"/> for 
    ///   <typeparamref name="TValue"/> is returned;
    ///   <br/>
    /// - if v is found and is a leaf, the reference to t in p is set to null, and no other change is made in the tree;
    ///   <br/>
    /// -   
    /// </remarks>
    public TValue? Remove(TKey key)
    {
        var (node, parent) = Find(Root, null);
        if (node == null)
            return default;

        Remove(node);


        static Node Remove(Node node)
        {
            // First case: the node is a leaf or has a single child
            if (node.Left == null || node.Right == null)
            {
                var child = node.Left ?? node.Right;
                if (parent == null)
                    Root = child;
                else if (ReferenceEquals(parent.Left, node))
                    parent.Left = child;
                else
                    parent.Right = child;

                return node;
            }

            // Second case: the node has two children
            var leftChild = node.Left;


        }

        static (Node? node, Node? parent) Find(Node? node, Node? parent) => 
            node?.Key.CompareTo(key) switch
            {
                null => (default, default),
                0 => (node, parent),
                > 0 => Find(node.Left, node),
                _ => Find(node.Right, node),
            };
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
        var (found, valueFound) = Find(Root, key);
        value = found ? valueFound : default;
        return found;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// TODO
    /// </remarks>
    public TValue? Remove(TKey key)
    {
        throw new NotImplementedException();
    }
}

