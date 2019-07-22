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
        /// Determines whether the tree contains the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><see langword="true"/> if the tree contains the item; <see langword="false"/> otherwise.</returns>
        public bool Contains([NotNull] in T item)
        {
            var token = _root;
            while (token != null)
            {
                var result = item.CompareTo(token.Value);
                if (result < 0)
                {
                    // descend left
                    token = token.LeftNode;
                }
                else if (result > 0)
                {
                    // descend right
                    token = token.RightNode;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Traverses the tree's items in the specified traversal order.
        /// </summary>
        /// <param name="mode">The traversal order.</param>
        /// <returns>An <see cref="IEnumerable{T}"/>.</returns>
        [SuppressMessage("ReSharper", "HeuristicUnreachableCode", Justification = "Null action gracefully exits")]
        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse", Justification = "Null action gracefully exits")]
        [NotNull]
        public IEnumerable<T> Traverse(TraversalMode mode)
        {
            switch (mode)
            {
                case TraversalMode.PreOrder:
                {
                    return TraversePreOrderRecursively(_root);
                }

                case TraversalMode.InOrder:
                {
                    return TraverseInOrderRecursively(_root);
                }

                case TraversalMode.PostOrder: // "depth-first":
                {
                    return TraversePostOrderRecursively(_root);
                }

                case TraversalMode.LevelOrder: // "breadth-first":
                {
                    return TraverseLevelOrder(_root);
                }

                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, $"Unhandled traversal mode: {mode}");
                }
            }
        }

        /// <summary>
        /// Traverses the tree's items in pre-order mode.
        /// </summary>
        /// <param name="node">The node to process.</param>
        [NotNull]
        private IEnumerable<T> TraversePreOrderRecursively([CanBeNull] TreeNode<T> node)
        {
            if (node == null)
            {
                yield break;
            }

            yield return node.Value;
            foreach (var item in TraversePreOrderRecursively(node.LeftNode))
            {
                yield return item;
            }

            // ReSharper disable once TailRecursiveCall
            foreach (var item in TraversePreOrderRecursively(node.RightNode))
            {
                yield return item;
            }
        }

        /// <summary>
        /// Traverses the tree's items in in-order mode.
        /// </summary>
        /// <param name="node">The node to process.</param>
        [NotNull]
        private IEnumerable<T> TraverseInOrderRecursively([CanBeNull] TreeNode<T> node)
        {
            if (node == null)
            {
                yield break;
            }

            foreach (var item in TraverseInOrderRecursively(node.LeftNode))
            {
                yield return item;
            }
            
            yield return node.Value;

            // ReSharper disable once TailRecursiveCall
            foreach (var item in TraverseInOrderRecursively(node.RightNode))
            {
                yield return item;
            }
        }

        /// <summary>
        /// Traverses the tree's items in post-order mode.
        /// </summary>
        /// <param name="node">The node to process.</param>
        [NotNull]
        private IEnumerable<T> TraversePostOrderRecursively([CanBeNull] TreeNode<T> node)
        {
            if (node == null)
            {
                yield break;
            }

            foreach (var item in TraversePostOrderRecursively(node.LeftNode))
            {
                yield return item;
            }

            foreach (var item in TraversePostOrderRecursively(node.RightNode))
            {
                yield return item;
            }

            yield return node.Value;
        }

        /// <summary>
        /// Traverses the tree's items in post-order mode.
        /// </summary>
        /// <param name="node">The node to process.</param>
        [NotNull]
        private IEnumerable<T> TraverseLevelOrder([CanBeNull] TreeNode<T> node)
        {
            if (node == null)
            {
                yield break;
            }

            var expansionList = new Queue<TreeNode<T>>();
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

            return 1 + GetSizeRecursively(node.LeftNode)
                     + GetSizeRecursively(node.RightNode);
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