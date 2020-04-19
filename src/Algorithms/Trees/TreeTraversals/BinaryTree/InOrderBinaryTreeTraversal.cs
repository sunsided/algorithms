using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Widemeadows.Algorithms.Trees.TreeTraversals.BinaryTree
{
    /// <summary>
    /// In-order tree traversal.
    /// </summary>
    /// <seealso cref="RecursiveInOrderBinaryTreeTraversal{TData}"/>
    internal sealed class InOrderBinaryTreeTraversal<TData> : TreeTraversal<BinaryTreeNode<TData>>
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
                    // Push the current sub-tree's root to the stack.
                    stack.Push(node);

                    // Descend into left sub-tree.
                    node = node.LeftNode;
                }

                if (stack.Count == 0)
                {
                    yield break;
                }

                // Restore and process the last sub-tree's root node.
                // Note that we descended into the left arm first, so this
                // is the last node's left sub-tree.
                node = stack.Pop();
                yield return node;

                // Descend into the right sub-tree.
                node = node.RightNode;
            }
        }
    }
}