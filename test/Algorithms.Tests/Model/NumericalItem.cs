using System;
using System.Diagnostics;
using System.Globalization;

namespace Widemeadows.Algorithms.Tests.Model
{
    /// <summary>
    /// A test item.
    /// </summary>
    [DebuggerDisplay("NumericalItem({" + nameof(Value) + "})")]
    [DebuggerStepThrough]
    public readonly struct NumericalItem : IEquatable<NumericalItem>, IComparable<NumericalItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericalItem"/> struct.
        /// </summary>
        /// <param name="value">The item value.</param>
        public NumericalItem(int value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the item value.
        /// </summary>
        /// <value>The value.</value>
        public int Value { get; }

        /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
        public bool Equals(NumericalItem other) => Value == other.Value;

        /// <inheritdoc cref="ValueType.Equals(object)" />
        public override bool Equals(object? obj) => obj is NumericalItem other && Equals(other);

        /// <inheritdoc cref="ValueType.GetHashCode" />
        public override int GetHashCode() => Value;

        /// <inheritdoc cref="IComparable{T}.CompareTo" />
        public int CompareTo(NumericalItem other) => Value.CompareTo(other.Value);

        /// <inheritdoc cref="ValueType.ToString"/>
        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Implicitly converts an <see cref="int"/> to a <see cref="NumericalItem"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The <see cref="NumericalItem"/> representing the <paramref name="value"/>.</returns>
        public static implicit operator NumericalItem(int value) => new NumericalItem(value);

        public static bool operator <(NumericalItem lhs, NumericalItem rhs) => lhs.CompareTo(rhs) < 0;
        public static bool operator >(NumericalItem lhs, NumericalItem rhs) => lhs.CompareTo(rhs) > 0;
        public static bool operator <=(NumericalItem lhs, NumericalItem rhs) => lhs.CompareTo(rhs) <= 0;
        public static bool operator >=(NumericalItem lhs, NumericalItem rhs) => lhs.CompareTo(rhs) >= 0;
        public static bool operator ==(NumericalItem lhs, NumericalItem rhs) => lhs.CompareTo(rhs) == 0;
        public static bool operator !=(NumericalItem lhs, NumericalItem rhs) => lhs.CompareTo(rhs) != 0;
    }
}