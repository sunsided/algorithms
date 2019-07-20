using System;
using System.Diagnostics;

namespace Widemeadows.Algorithms.Tests.Data
{
    /// <summary>
    /// A test item.
    /// </summary>
    [DebuggerDisplay("NumericalItem({" + nameof(Value) + "})")]
    [DebuggerStepThrough]
    internal readonly struct NumericalItem : IEquatable<NumericalItem>, IComparable<NumericalItem>
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
        public bool Equals(NumericalItem other)
        {
            return Value == other.Value;
        }

        /// <inheritdoc cref="object.Equals(object)" />
        public override bool Equals(object obj)
        {
            return obj is NumericalItem other && Equals(other);
        }

        /// <inheritdoc cref="object.GetHashCode" />
        public override int GetHashCode()
        {
            return Value;
        }

        /// <inheritdoc cref="IComparable{T}.CompareTo" />
        public int CompareTo(NumericalItem other)
        {
            return Value.CompareTo(other.Value);
        }
    }
}