using JetBrains.Annotations;

namespace Widemeadows.Algorithms.Trees
{
    /// <summary>
    /// A tree node.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    internal sealed class TreeNode<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode{T}"/> class.
        /// </summary>
        /// <param name="value">The value of the node.</param>
        public TreeNode([NotNull] T value)
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
        public TreeNode<T> LeftNode { get; set; }

        /// <summary>
        /// Gets or sets the right node reference.
        /// </summary>
        /// /// <value>The node.</value>
        [CanBeNull]
        public TreeNode<T> RightNode { get; set; }
    }
}