using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Widemeadows.Algorithms.Trees.TreeTraversals.BinaryTree
{
    /// <summary>
    /// Level-order tree traversal.
    /// </summary>
    /// <remarks>
    /// Level-order traversal is also known as "breadth-first".
    /// </remarks>
    internal sealed class LevelOrderBinaryTreeTraversal<TData> : TreeTraversal<BinaryTreeNode<TData>>
    {
        /// <inheritdoc cref="TreeTraversal{TNode}.TraverseNodes"/>
        [Pure]
        public override IEnumerable<BinaryTreeNode<TData>> TraverseNodes(BinaryTreeNode<TData> node)
        {
            if (node == null)
            {
                yield break;
            }

            var expansionList = new Queue<BinaryTreeNode<TData>>();
            expansionList.Enqueue(node);

            while (expansionList.Count > 0)
            {
                node = expansionList.Dequeue();
                yield return node;

                if (node.LeftNode != null)
                {
                    // expand left
                    expansionList.Enqueue(node.LeftNode);
                }

                if (node.RightNode != null)
                {
                    // expand right
                    expansionList.Enqueue(node.RightNode);
                }
            }
        }
    }
}