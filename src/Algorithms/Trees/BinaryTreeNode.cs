using System.Diagnostics;
using Widemeadows.Algorithms.Properties;

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
        /// <value>The node.</value>
        [CanBeNull]
        public BinaryTreeNode<T> LeftNode { get; set; }

        /// <summary>
        /// Gets or sets the right node reference.
        /// </summary>
        /// <value>The node.</value>
        [CanBeNull]
        public BinaryTreeNode<T> RightNode { get; set; }

        /// <summary>
        /// Gets a value indicating whether this node is a leaf node.
        /// </summary>
        /// <remarks>
        /// A node is considered to be a leaf if it has no children.
        /// </remarks>
        /// <value><see langword="true"/> if this node is a leaf; <see langword="false"/> otherwise.</value>
        public bool IsLeaf => LeftNode == null && RightNode == null;

        /// <summary>
        /// Gets a value indicating whether this node is a full node.
        /// </summary>
        /// <remarks>
        /// A node is considered to be full if each branch or sub-tree is non-<see langword="null"/>.
        /// </remarks>
        /// <value><see langword="true"/> if this node is full; <see langword="false"/> otherwise.</value>
        public bool IsFull => LeftNode != null && RightNode != null;

        /// <summary>
        /// Gets a value indicating whether this node is a half node.
        /// </summary>
        /// <remarks>
        /// A binary tree node is considered to be half if exactly one branch or sub-tree is non-<see langword="null"/>.
        /// </remarks>
        /// <value><see langword="true"/> if this node is half; <see langword="false"/> otherwise.</value>
        public bool IsHalf => !(IsLeaf || IsFull);
    }
}