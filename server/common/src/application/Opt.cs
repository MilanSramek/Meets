using System.Diagnostics.CodeAnalysis;

namespace Meets.Common.Application;

/// <summary>
/// The optional type is used to differentiate between not set and set input values.
/// </summary>
public readonly struct Opt<T> : IEquatable<Opt<T>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Opt{T}"/> struct.
    /// </summary>
    /// <param name="value">The actual value.</param>
    public Opt(T? value)
    {
        Value = value;
        HasValue = true;
    }

    public Opt()
    {
        HasValue = false;
    }

    /// <summary>
    /// The name value.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// <c>true</c> if the optional was explicitly set.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Value))]
    public bool HasValue { get; }

    /// <summary>
    /// <c>true</c> if the optional was not explicitly set.
    /// </summary>
    public bool IsEmpty => !HasValue;

    /// <summary>
    /// Provides the name string.
    /// </summary>
    /// <returns>The name string value</returns>
    public override string ToString()
    {
        return HasValue ? Value?.ToString() ?? "null" : "<<unspecified>>";
    }

    /// <summary>
    /// Compares this <see cref="Opt{T}"/> value to another value.
    /// </summary>
    /// <param name="other">
    /// The second <see cref="Opt{T}"/> for comparison.
    /// </param>
    /// <returns>
    /// <c>true</c> if both <see cref="Opt{T}"/> values are equal.
    /// </returns>
    public bool Equals(Opt<T> other)
    {
        if (!HasValue && !other.HasValue)
        {
            return true;
        }

        if (HasValue != other.HasValue)
        {
            return false;
        }

        return Equals(Value, other.Value);
    }

    /// <summary>
    /// Compares this <see cref="Opt{T}"/> value to another value.
    /// </summary>
    /// <param name="obj">
    /// The second <see cref="Opt{T}"/> for comparison.
    /// </param>
    /// <returns>
    /// <c>true</c> if both <see cref="Opt{T}"/> values are equal.
    /// </returns>
    public override bool Equals(object? obj) => obj is Opt<T> other
        && Equals(other);

    /// <summary>
    /// Serves as a hash function for a <see cref="Opt{T}"/> object.
    /// </summary>
    /// <returns>
    /// A hash code for this instance that is suitable for use in hashing
    /// algorithms and data structures such as a hash table.
    /// </returns>
    public override int GetHashCode() => HasValue
        ? Value?.GetHashCode() ?? 0
        : 0;

    /// <summary>
    /// Operator call through to Equals
    /// </summary>
    /// <param name="left">The left parameter</param>
    /// <param name="right">The right parameter</param>
    /// <returns>
    /// <c>true</c> if both <see cref="Opt{T}"/> values are equal.
    /// </returns>
    public static bool operator ==(Opt<T> left, Opt<T> right) => left.Equals(right);

    /// <summary>
    /// Operator call through to Equals
    /// </summary>
    /// <param name="left">The left parameter</param>
    /// <param name="right">The right parameter</param>
    /// <returns>
    /// <c>true</c> if both <see cref="Opt{T}"/> values are not equal.
    /// </returns>
    public static bool operator !=(Opt<T> left, Opt<T> right) => !left.Equals(right);

    /// <summary>
    /// Implicitly creates a new <see cref="Opt{T}"/> from
    /// the given value.
    /// </summary>
    /// <param name="value">The value.</param>
    public static implicit operator Opt<T>(T value) => new(value);

    /// <summary>
    /// Implicitly gets the optional value.
    /// </summary>
    [return: MaybeNull]
    public static implicit operator T(Opt<T> optional) => optional.Value;

    /// <summary>
    /// Creates an empty optional that provides a default value.
    /// </summary>
    /// <param name="defaultValue">The default value.</param>
    public static Opt<T> Empty() => new();
}
