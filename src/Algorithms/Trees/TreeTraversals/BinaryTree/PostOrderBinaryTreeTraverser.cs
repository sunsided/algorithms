using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Widemeadows.Algorithms.Trees.TreeTraversals.BinaryTree
{
    /// <summary>
    /// Post-order tree traversal.
    /// </summary>
    /// <seealso cref="RecursivePostOrderBinaryTreeTraverser{TData}"/>
    internal sealed class PostOrderBinaryTreeTraverser<TData> : BinaryTreeTraversal<TData>
    {
        /// <inheritdoc cref="TreeTraversal{TNode,TData}.Traverse"/>
        [Pure]
        public override IEnumerable<TData> Traverse(BinaryTreeNode<TData> node)
        {
            if (node == null)
            {
                yield break;
            }

            var stack = new Stack<BinaryTreeNode<TData>>();

            // We use a token variable to test whether we're ascending
            // from a child back to its parent.
            BinaryTreeNode<TData> previousNode = null;

            do
            {
                while (node != null)
                {
                    // Push the current sub-tree's root to the stack.
                    stack.Push(node);

                    // Descend into left sub-tree.
                    node = node.LeftNode;
                }

                // For post-order traversal, we are visiting each node twice,
                // the first time for exploring both the left and right sub-tree,
                // the second time for processing the node itself.
                while (node == null && stack.Count > 0)
                {
                    // Since we exhausted the entire left sub-tree, we're now peeking
                    // at a node with no left child.
                    // Note that we don't pop the node from the stack since we may need
                    // to revisit it later, should it have a right sub-tree.
                    node = stack.Peek();

                    // If the node has a right sub-tree - or if we're not ascending
                    // from its right sub-tree - continue descent.
                    if (node.RightNode != null && node.RightNode != previousNode)
                    {
                        // Descend into the right sub-tree.
                        node = node.RightNode;
                    }
                    else
                    {
                        // Here, the node is either a leaf or we are ascending from its right sub-tree.
                        // In either case, we can now process the node (the current sub-tree's root) itself.
                        yield return node.Value;

                        // Since we visited the node, we can discard it.
                        stack.Pop();

                        // Keep track of the current node to be able to
                        // test whether we're ascending back to our parent.
                        previousNode = node;

                        // Indicate that there is no right sub-tree to explore.
                        node = null;
                    }
                }
            } while (stack.Count != 0);
        }
    }
}