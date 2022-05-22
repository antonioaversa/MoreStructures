namespace StringAlgorithms.Utilities;

/// <summary>
/// An empty object with value equality (always true), to be used as lock object in records and other value structures.
/// </summary>
public class LockValueObject
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="obj"><inheritdoc path="/param[@name='obj']"/></param>
    /// <returns>
    /// Since all <see cref="LockValueObject"/> instances are empty, and equality is done by value, this method always 
    /// returns <see langword="true"/>, when <paramref name="obj"/> is of type <see cref="LockValueObject"/>, and 
    /// <see langword="false"/> otherwise.
    /// </returns>
    public override bool Equals(object? obj) => obj is LockValueObject;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns>
    /// Since all <see cref="LockValueObject"/> instances are empty, this method always returns the same value (0).
    /// </returns>
    public override int GetHashCode() => 0;
}
