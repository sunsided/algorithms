using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Widemeadows.Algorithms.Tests.Model
{
    /// <summary>
    /// A test item.
    /// </summary>
    [DebuggerDisplay("ReferenceItem({" + nameof(Value) + "})")]
    [DebuggerStepThrough]
    public sealed class ReferenceItem<T> : IEquatable<ReferenceItem<T>>, IComparable<ReferenceItem<T>>
        where T : IComparable<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericalItem"/> struct.
        /// </summary>
        /// <param name="value">The item value.</param>
        public ReferenceItem([CanBeNull] T value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the item value.
        /// </summary>
        /// <value>The value.</value>
        [CanBeNull]
        public T Value { get; }

        /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
        public bool Equals(ReferenceItem<T> other) => Equals(Value, other.Value);

        /// <inheritdoc cref="ValueType.Equals(object)" />
        public override bool Equals(object obj) => obj is ReferenceItem<T> other && Equals(other);

        /// <inheritdoc cref="ValueType.GetHashCode" />
        public override int GetHashCode() => Value?.GetHashCode() ?? 0;

        /// <inheritdoc cref="IComparable{T}.CompareTo" />
        public int CompareTo(ReferenceItem<T> other)
        {
            if (ReferenceEquals(Value, other.Value)) return 0;
            if (ReferenceEquals(null, Value) && !ReferenceEquals(other.Value, null)) return -1;
            if (!ReferenceEquals(null, Value) && ReferenceEquals(other.Value, null)) return 1;
            Debug.Assert(Value != null, "Value != null");
            return Value.CompareTo(other.Value);
        }

        /// <inheritdoc cref="ValueType.ToString"/>
        public override string ToString() => Value?.ToString();

        /// <summary>
        /// Implicitly converts an <see cref="int"/> to a <see cref="NumericalItem"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The <see cref="NumericalItem"/> representing the <paramref name="value"/>.</returns>
        public static implicit operator ReferenceItem<T>(T value) => new ReferenceItem<T>(value);
    }
}