namespace MoreStructures.Strings;

/// <summary>
/// The result of matching a <see cref="string"/> text against a <see cref="string"/> pattern.
/// </summary>
/// <param name="Success">Whether the text has been completely matched or not.</param>
/// <param name="Begin">The index in text of the best match.</param>
/// <param name="MatchedChars">The number of chars matched from the text.</param>
/// <param name="Path">The path of nodes visited by the matching algorithm.</param>
/// <typeparam name="TPath">
/// The type of path. Depends on the algorithm and the data structure used for pattern matching.
/// </typeparam>
public sealed record Match<TPath>(bool Success, int Begin, int MatchedChars, TPath Path);