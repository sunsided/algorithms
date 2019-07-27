namespace Widemeadows.Algorithms.Trees.TreeTraversals.BinaryTree
{
    /// <summary>
    /// Base class for binary tree traversals.
    /// </summary>
    /// <typeparam name="TData">The type of the data to emit.</typeparam>
    internal abstract class BinaryTreeTraversal<TData> : TreeTraversal<BinaryTreeNode<TData>, TData>
    {
    }
}