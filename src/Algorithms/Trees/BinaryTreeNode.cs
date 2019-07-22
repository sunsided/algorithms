using System.Diagnostics;
using JetBrains.Annotations;

namespace Widemeadows.Algorithms.Trees
{
    /// <summary>
    /// A tree node.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    [DebuggerDisplay("BTN[{" + nameof(Value) + "}]")]
    [DebuggerStepThrough]
    internal sealed class BinaryTreeNode<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryTreeNode{T}"/> class.
        /// </summary>
        /// <param name="value">The value of the node.</param>
        public BinaryTreeNode([NotNull] T value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets or sets the value of the node.
        /// </summary>
        /// <value>The item.</value>
        [NotNull]
        public T Value { get; }

        /// <summary>
        /// Gets or sets the left node reference.
        /// </summary>
        /// /// <value>The node.</value>
        [CanBeNull]
        public BinaryTreeNode<T> LeftNode { get; set; }

        /// <summary>
        /// Gets or sets the right node reference.
        /// </summary>
        /// /// <value>The node.</value>
        [CanBeNull]
        public BinaryTreeNode<T> RightNode { get; set; }
    }
}