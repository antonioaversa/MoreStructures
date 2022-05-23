namespace MoreStructures.Utilities;

/// <summary>
/// An empty object with value equality (always true), to be used as lock object in records and other value structures.
/// </summary>
/// <remarks>
/// Since all <see cref="LockValueObject"/> instances are empty, and equality is done by value, 
/// <see cref="Equals(object?)"/> always returns <see langword="true"/>, when the provided object is of type 
/// <see cref="LockValueObject"/>, and <see cref="GetHashCode"/> always returns the same value (0).
/// </remarks>
public record LockValueObject;