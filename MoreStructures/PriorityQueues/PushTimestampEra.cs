namespace MoreStructures.PriorityQueues;

/// <summary>
/// Defines a reference-type object wrapping an <see cref="int"/> <paramref name="Era"/>, which is the integer value,
/// all push timestamps of <see cref="PrioritizedItem{T}"/> instances having such base should be part of.
/// </summary>
/// <param name="Era">The value of the era, the push timestamp refers to. Any integer, positive or negative.</param>
/// <remarks>
/// The order of push timestamps in different eras is solely determined by the era: push timestamps in lower eras are 
/// always considered smaller than push timestamps in higher eras, no matter their value.
/// <br/>
/// Within the same era, higher push timestamps are considered higher than lower push timestamps (i.e. the timestamp 
/// value is taken into account).
/// </remarks>
public record PushTimestampEra(int Era)
{
    /// <summary>
    /// <inheritdoc cref="PushTimestampEra" path="/param[@name='Era']"/>
    /// </summary>
    public int Era { get; set; } = Era;
}
