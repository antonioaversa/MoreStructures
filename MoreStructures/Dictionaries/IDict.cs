namespace MoreStructures.Dictionaries;

/// <summary>
/// A mapping between instances of <typeparamref name="TKey"/> and instances of <typeparamref name="TValue"/>, where 
/// a key is associated to 0 or 1 value.
/// </summary>
/// <typeparam name="TKey">The type of the key instances in the dictionary.</typeparam>
/// <typeparam name="TValue">The type of the value instances in the dictionary.</typeparam>
public interface IDict<TKey, TValue>
    where TKey : notnull
{
    /// <summary>
    /// The number of (key, value) mappings in this dictionary.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Gets or inserts/updates the item with the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The key associated to the value to be retrieved.</param>
    /// <returns>
    /// The value associated with the provided <paramref name="key"/>, if any.
    /// <br/>
    /// If such a key does not exist, a <see cref="KeyNotFoundException"/> is thrown.
    /// </returns>
    TValue this[TKey key] { get; set; }

    /// <summary>
    /// A sequence enumerating all the keys in this dictionary. The order is specific to the implementation.
    /// </summary>
    /// <remarks>
    /// In general, it's not guaranteed that the order of <see cref="Keys"/> is the same as the order of 
    /// <see cref="Values"/>.
    /// </remarks>
    IEnumerable<TKey> Keys { get; }

    /// <summary>
    /// A sequence enumerating all the values in this dictionary. The order is specific to the implementation.
    /// </summary>
    /// <remarks>
    /// In general, it's not guaranteed that the order of <see cref="Values"/> is the same as the order of 
    /// <see cref="Keys"/>.
    /// </remarks>
    IEnumerable<TValue> Values { get; }

    /// <summary>
    /// Adds a mapping between the provided <paramref name="key"/> and <paramref name="value"/>.
    /// </summary>
    /// <param name="key">The key of the mapping.</param>
    /// <param name="value">The value of the mapping.</param>
    void Add(TKey key, TValue value);

    /// <summary>
    /// Whether this dictionary contains a mapping for the provided <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The key, to look for in the dictionary.</param>
    /// <returns>Whether there is a mapping with the provided <paramref name="key"/> in this dictionary.</returns>
    bool ContainsKey(TKey key);

    /// <summary>
    /// Removes the mapping with the provided <paramref name="key"/> from this dictionary, if any.
    /// </summary>
    /// <param name="key">The key of the mapping to be removed.</param>
    /// <returns>The value of the mapping with the provided <paramref name="key"/>.</returns>
    TValue? Remove(TKey key);

    /// <summary>
    /// Tries to get the value for the provided <paramref name="key"/>, storing into the provided 
    /// <paramref name="value"/> reference.
    /// </summary>
    /// <param name="key">The key, to look for in the dictionary.</param>
    /// <param name="value">
    /// Upon method return, it contains the value associated to <paramref name="key"/>, if such a mapping exists in the
    /// dictionary. Returns the <see langword="default"/> for <typeparamref name="TValue"/> otherwise.
    /// </param>
    /// <returns>Whether the retrieval attempt was successful or not.</returns>
    bool TryGetValue(TKey key, out TValue? value);
}

