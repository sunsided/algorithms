using System;
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
        [SuppressMessage("ReSharper", "CA1822")]
        public int GetSize() => GetSizeRecursive(_root);

        /// <summary>
        /// Calculates the height (or depth) of the tree.
        /// </summary>
        /// <param name="height">The height of the tree, or <c>-1</c> if the tree has no elements.</param>
        /// <returns><see langword="true"/> if the tree has a height; <see langword="false"/> otherwise.</returns>
        [SuppressMessage("ReSharper", "CA1822")]
        public bool TryGetHeight(out int height) => TryGetHeightRecursive(_root, out height);

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
        /// Calculates the number of items in the tree.
        /// </summary>
        /// <returns>The size of the tree.</returns>
        private int GetSizeRecursive([CanBeNull] TreeNode<T> node)
        {
            if (node == null)
            {
                return 0;
            }

            return 1 + GetSizeRecursive(node.LeftNode) + GetSizeRecursive(node.RightNode);
        }

        /// <summary>
        /// Calculates the number of items in the tree.
        /// </summary>
        /// <returns>The size of the tree.</returns>
        private bool TryGetHeightRecursive([CanBeNull] TreeNode<T> node, out int height)
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
            var hasChild = TryGetHeightRecursive(node.LeftNode, out var leftHeight) |
                           TryGetHeightRecursive(node.RightNode, out var rightHeight);
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