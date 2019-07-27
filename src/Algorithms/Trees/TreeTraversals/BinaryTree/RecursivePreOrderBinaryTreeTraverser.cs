using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Widemeadows.Algorithms.Trees.TreeTraversals.BinaryTree
{
    /// <summary>
    /// Recursive pre-order tree traversal.
    /// </summary>
    /// <seealso cref="PreOrderBinaryTreeTraverser{TData}"/>
    internal sealed class RecursivePreOrderBinaryTreeTraverser<TData> : BinaryTreeTraversal<TData>
    {
        /// <inheritdoc cref="TreeTraversal{TNode,TData}.Traverse"/>
        [Pure]
        public override IEnumerable<TData> Traverse(BinaryTreeNode<TData> node)
        {
            if (node == null)
            {
                yield break;
            }

            yield return node.Value;

            foreach (var item in Traverse(node.LeftNode))
            {
                yield return item;
            }

            // ReSharper disable once TailRecursiveCall
            foreach (var item in Traverse(node.RightNode))
            {
                yield return item;
            }
        }
    }
}