using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Widemeadows.Algorithms.Trees.TreeTraversals.BinaryTree
{
    /// <summary>
    /// Recursive post-order tree traversal.
    /// </summary>
    /// <seealso cref="PostOrderBinaryTreeTraverser{TData}"/>
    internal sealed class RecursivePostOrderBinaryTreeTraverser<TData> : TreeTraversal<BinaryTreeNode<TData>>
    {
        /// <inheritdoc cref="TreeTraversal{TNode}.TraverseNodes"/>
        [Pure]
        public override IEnumerable<BinaryTreeNode<TData>> TraverseNodes(BinaryTreeNode<TData> node)
        {
            if (node == null)
            {
                yield break;
            }

            foreach (var item in TraverseNodes(node.LeftNode))
            {
                yield return item;
            }

            foreach (var item in TraverseNodes(node.RightNode))
            {
                yield return item;
            }

            yield return node;
        }
    }
}