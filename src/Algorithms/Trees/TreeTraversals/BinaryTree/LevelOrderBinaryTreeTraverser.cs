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
    internal sealed class LevelOrderBinaryTreeTraverser<TData> : BinaryTreeTraversal<TData>
    {
        /// <inheritdoc cref="TreeTraversal{TNode,TData}.Traverse"/>
        [Pure]
        public override IEnumerable<TData> Traverse(BinaryTreeNode<TData> node)
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
                yield return node.Value;

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