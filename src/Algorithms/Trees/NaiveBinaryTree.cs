using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Widemeadows.Algorithms.Trees
{
    /// <summary>
    /// A naive binary tree implementation.
    /// </summary>
    /// <typeparam name="T">The type of the item.</typeparam>
    public sealed class NaiveBinaryTree<T>
        where T : IComparable<T>
    {
        /// <summary>
        /// The root node.
        /// </summary>
        [CanBeNull]
        private TreeNode<T> _root;

        /// <summary>
        /// Calculates the number of items in the tree.
        /// </summary>
        /// <returns>The size of the tree.</returns>
        public int GetSize() => GetSizeRecursively(_root);

        /// <summary>
        /// Calculates the height (or depth) of the tree.
        /// </summary>
        /// <param name="height">The height of the tree, or <c>-1</c> if the tree has no elements.</param>
        /// <returns><see langword="true"/> if the tree has a height; <see langword="false"/> otherwise.</returns>
        public bool TryGetHeight(out int height) => TryGetHeightRecursively(_root, out height);

        /// <summary>
        /// Inserts a node into the tree.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Insert([NotNull] in T item)
        {
            if (_root == null)
            {
                _root = new TreeNode<T>(item);
                return;
            }

            var token = _root;
            while (true)
            {
                if (item.CompareTo(token.Value) <= 0)
                {
                    if (token.LeftNode == null)
                    {
                        token.LeftNode = new TreeNode<T>(item);
                        return;
                    }

                    // descend left
                    token = token.LeftNode;
                }
                else
                {
                    if (token.RightNode == null)
                    {
                        token.RightNode = new TreeNode<T>(item);
                        return;
                    }

                    // descend right
                    token = token.RightNode;
                }
            }
        }

        /// <summary>
        /// Traverses the tree's items in the specified traversal order.
        /// </summary>
        /// <param name="mode">The traversal order.</param>
        /// <param name="action">The action to apply to each item.</param>
        [SuppressMessage("ReSharper", "HeuristicUnreachableCode", Justification = "Null action gracefully exits")]
        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse", Justification = "Null action gracefully exits")]
        public void Traverse(TraversalMode mode, [NotNull] Action<T> action)
        {
            if (action == null)
            {
                return;
            }

            switch (mode)
            {
                case TraversalMode.PreOrder:
                {
                    TraversePreorderRecursively(_root, action);
                    break;
                }

                case TraversalMode.InOrder:
                {
                    TraverseInorderRecursively(_root, action);
                    break;
                }

                case TraversalMode.PostOrder: // "depth-first":
                {
                    TraversePostorderRecursively(_root, action);
                    break;
                }

                case TraversalMode.LevelOrder: // "breadth-first":
                {
                    TraverseLevelOrder(_root, action);
                    break;
                }
            }
        }

        /// <summary>
        /// Traverses the tree's items in pre-order mode.
        /// </summary>
        /// <param name="node">The node to process.</param>
        /// <param name="action">The action to apply to each node.</param>
        private void TraversePreorderRecursively([CanBeNull] TreeNode<T> node, [NotNull] Action<T> action)
        {
            if (node == null)
            {
                return;
            }

            action(node.Value);
            TraversePreorderRecursively(node.LeftNode, action);
            TraversePreorderRecursively(node.RightNode, action);
        }

        /// <summary>
        /// Traverses the tree's items in in-order mode.
        /// </summary>
        /// <param name="node">The node to process.</param>
        /// <param name="action">The action to apply to each node.</param>
        private void TraverseInorderRecursively([CanBeNull] TreeNode<T> node, [NotNull] Action<T> action)
        {
            if (node == null)
            {
                return;
            }

            TraverseInorderRecursively(node.LeftNode, action);
            action(node.Value);
            TraverseInorderRecursively(node.RightNode, action);
        }

        /// <summary>
        /// Traverses the tree's items in post-order mode.
        /// </summary>
        /// <param name="node">The node to process.</param>
        /// <param name="action">The action to apply to each node.</param>
        private void TraversePostorderRecursively([CanBeNull] TreeNode<T> node, [NotNull] Action<T> action)
        {
            if (node == null)
            {
                return;
            }

            TraversePostorderRecursively(node.LeftNode, action);
            TraversePostorderRecursively(node.RightNode, action);
            action(node.Value);
        }

        /// <summary>
        /// Traverses the tree's items in post-order mode.
        /// </summary>
        /// <param name="node">The node to process.</param>
        /// <param name="action">The action to apply to each node.</param>
        private void TraverseLevelOrder([CanBeNull] TreeNode<T> node, [NotNull] Action<T> action)
        {
            if (node == null)
            {
                return;
            }

            var expansionList = new Queue<TreeNode<T>>();
            expansionList.Enqueue(node);

            while (expansionList.Count > 0)
            {
                node = expansionList.Dequeue();

                action(node.Value);

                if (node.LeftNode != null)
                {
                    expansionList.Enqueue(node.LeftNode);
                }

                if (node.RightNode != null)
                {
                    expansionList.Enqueue(node.RightNode);
                }
            }
        }

        /// <summary>
        /// Calculates the number of items in the tree.
        /// </summary>
        /// <returns>The size of the tree.</returns>
        private int GetSizeRecursively([CanBeNull] TreeNode<T> node)
        {
            if (node == null)
            {
                return 0;
            }

            return 1 + GetSizeRecursively(node.LeftNode) + GetSizeRecursively(node.RightNode);
        }

        /// <summary>
        /// Calculates the number of items in the tree.
        /// </summary>
        /// <returns>The size of the tree.</returns>
        private bool TryGetHeightRecursively([CanBeNull] TreeNode<T> node, out int height)
        {
            if (node == null)
            {
                height = -1;
                return false;
            }

            // The provided node is the root of a (possibly empty) subtree.
            // Since a root exists, this method will always return true.
            height = 0;

            // If the node has at least one child, the tree's height is at least 1,
            // and each sub-tree's height is at least 0.
            var hasChild = TryGetHeightRecursively(node.LeftNode, out var leftHeight) |
                           TryGetHeightRecursively(node.RightNode, out var rightHeight);
            if (hasChild)
            {
                var maxHeight = Math.Max(leftHeight, rightHeight);
                Debug.Assert(maxHeight >= 0, "maxHeight >= 0");

                height = 1 + maxHeight;
            }

            return true;
        }
    }
}