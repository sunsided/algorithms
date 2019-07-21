namespace Widemeadows.Algorithms.Trees
{
    /// <summary>
    /// An enumeration defining tree traversal order.
    /// </summary>
    public enum TraversalMode
    {
        /// <summary>
        /// Traverses the tree items in pre-order (root-left-right) mode.
        /// </summary>
        PreOrder,

        /// <summary>
        /// Traverses the tree items in in-order (left-root-right) mode.
        /// </summary>
        InOrder,

        /// <summary>
        /// Traverses the tree items in post-order (left-right-root) mode.
        /// </summary>
        /// <seealso cref="DepthFirst"/>
        PostOrder,

        /// <summary>
        /// Traverses the tree items in level-order (row-wise) mode.
        /// </summary>
        /// <seealso cref="BreadthFirst"/>
        LevelOrder,

        /// <summary>
        /// Traverses the tree items in depth-first (post-order) mode.
        /// </summary>
        /// <remarks>
        /// This mode is identical to <see cref="PostOrder"/>.
        /// </remarks>
        DepthFirst = PostOrder,

        /// <summary>
        /// Traverses the tree items in breadth-first (level-order) mode.
        /// </summary>
        /// <remarks>
        /// This mode is identical to <see cref="LevelOrder"/>.
        /// </remarks>
        BreadthFirst = LevelOrder,
    }
}