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
        Preorder = 0,

        /// <summary>
        /// Traverses the tree items in in-order (left-root-right) mode.
        /// </summary>
        Inorder = 1,

        /// <summary>
        /// Traverses the tree items in post-order (left-right-root) mode.
        /// </summary>
        Postorder = 2
    }
}