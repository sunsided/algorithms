using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Widemeadows.Algorithms.Trees.TreeTraversals.BinaryTree
{
    /// <summary>
    /// Recursive pre-order tree traversal.
    /// </summary>
    /// <seealso cref="PreOrderBinaryTreeTraversal{TData}"/>
    internal sealed class RecursivePreOrderBinaryTreeTraversal<TData> : TreeTraversal<BinaryTreeNode<TData>>
    {
        /// <inheritdoc cref="TreeTraversal{TNode}.TraverseNodes"/>
        [Pure]
        public override IEnumerable<BinaryTreeNode<TData>> TraverseNodes(BinaryTreeNode<TData> node)
        {
            if (node == null)
            {
                yield break;
            }

            yield return node;

            foreach (var item in TraverseNodes(node.LeftNode))
            {
                yield return item;
            }

            // ReSharper disable once TailRecursiveCall
            foreach (var item in TraverseNodes(node.RightNode))
            {
                yield return item;
            }
        }
    }
}