using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Widemeadows.Algorithms.Trees.TreeTraversals.BinaryTree
{
    /// <summary>
    /// Pre-order tree traversal.
    /// </summary>
    /// <remarks>
    /// Pre-order traversal is also known as "depth-first".
    /// </remarks>
    /// <seealso cref="RecursivePreOrderBinaryTreeTraversal{TData}"/>
    internal sealed class PreOrderBinaryTreeTraversal<TData> : TreeTraversal<BinaryTreeNode<TData>>
    {
        /// <inheritdoc cref="TreeTraversal{TNode}.TraverseNodes"/>
        [Pure]
        public override IEnumerable<BinaryTreeNode<TData>> TraverseNodes(BinaryTreeNode<TData>? node)
        {
            if (node == null)
            {
                yield break;
            }

            var stack = new Stack<BinaryTreeNode<TData>>();
            while (true)
            {
                while (node != null)
                {
                    // Process the current sub-tree's root node first.
                    yield return node;

                    // Push the current sub-tree's right sub-tree to the stack.
                    if (node.RightNode != null)
                    {
                        stack.Push(node.RightNode);
                    }

                    // Descend into left sub-tree.
                    node = node.LeftNode;
                }

                if (stack.Count == 0)
                {
                    yield break;
                }

                // Restore the last sub-tree's right node,
                // thus descending into the right sub-tree.
                node = stack.Pop();
            }
        }
    }
}