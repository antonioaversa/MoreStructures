namespace MoreStructures.Utilities;

/// <summary>
/// Generic utilities and extensions for strings.
/// </summary>
public static class NumericUtilities
{
    /// <summary>
    /// Returns the middle point of the provided <paramref name="interval"/>.
    /// </summary>
    /// <param name="interval">A 1D interval, identified by a lower and an upper extreme.</param>
    /// <returns>The middle point of the interval.</returns>
    /// <remarks>
    /// Uses the geometric interpretation of middle point to avoid int overflow, when the sum of the lower and the 
    /// upper extremes goes above <see cref="int.MaxValue"/> or below <see cref="int.MinValue"/>.
    /// </remarks>
    public static int Middle(this (int lower, int higher) interval) => 
        interval.lower + (interval.higher - interval.lower) / 2;

}
