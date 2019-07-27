using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Widemeadows.Algorithms.Trees.TreeTraversals.BinaryTree
{
    /// <summary>
    /// Recursive in-order tree traversal.
    /// </summary>
    /// <seealso cref="InOrderBinaryTreeTraverser{TData}"/>
    internal sealed class RecursiveInOrderBinaryTreeTraverser<TData> : BinaryTreeTraversal<TData>
    {
        /// <inheritdoc cref="TreeTraversal{TNode,TData}.Traverse"/>
        [Pure]
        public override IEnumerable<TData> Traverse(BinaryTreeNode<TData> node)
        {
            if (node == null)
            {
                yield break;
            }

            foreach (var item in Traverse(node.LeftNode))
            {
                yield return item;
            }

            yield return node.Value;

            // ReSharper disable once TailRecursiveCall
            foreach (var item in Traverse(node.RightNode))
            {
                yield return item;
            }
        }
    }
}