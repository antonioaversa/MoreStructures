namespace StringAlgorithms.Utilities
{
    /// <summary>
    /// Extension methods for value readonly collections.
    /// </summary>
    public static class ValueReadOnlyCollectionsExtensions
    {
        /// <summary>
        /// Builds a <see cref="ValueReadOnlyCollection{T}"/> out of the provided dictionary.
        /// </summary>
        /// <typeparam name="T">
        ///     <inheritdoc cref="ValueReadOnlyCollection{T}" path="/typeparam[@name='T']"/>
        /// </typeparam>
        /// <param name="enumerable">
        ///     <inheritdoc cref="ValueReadOnlyCollection{T}.ValueReadOnlyCollection(IEnumerable{T})" 
        ///     path="/param[@name='enumerable']"/>
        /// </param>
        /// <returns>
        /// An instance of <see cref="ValueReadOnlyCollection{T}"/>, independent from the provided enumerable.
        /// </returns>
        public static ValueReadOnlyCollection<T> ToValueReadOnlyCollection<T>(
            this IEnumerable<T> enumerable)
            where T : notnull => 
            new(enumerable);

        /// <summary>
        /// Builds a <see cref="ValueReadOnlyDictionary{TKey, TValue}"/> out of the provided dictionary.
        /// </summary>
        /// <typeparam name="TKey">
        ///     <inheritdoc cref="ValueReadOnlyDictionary{TKey, TValue}" path="/typeparam[@name='TKey']"/>
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     <inheritdoc cref="ValueReadOnlyDictionary{TKey, TValue}" path="/typeparam[@name='TValue']"/>
        /// </typeparam>
        /// <param name="dictionary">
        ///     <inheritdoc cref="ValueReadOnlyDictionary{TKey, TValue}.ValueReadOnlyDictionary(IDictionary{TKey, TValue})" 
        ///     path="/param[@name='dictionary']"/>
        /// </param>
        /// <returns>
        /// An instance of <see cref="ValueReadOnlyDictionary{TKey, TValue}"/>, independent from the provided dictionary.
        /// </returns>
        public static ValueReadOnlyDictionary<TKey, TValue> ToValueReadOnlyDictionary<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary)
            where TKey: notnull
            where TValue: notnull =>
            new(dictionary);
    }
}
