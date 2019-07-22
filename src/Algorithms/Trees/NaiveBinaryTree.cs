using System;
using System.Collections;
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
    [SuppressMessage("ReSharper", "CA1710", Justification = "Tree shouldn't be named collection")]
    public sealed class NaiveBinaryTree<T> : IEnumerable<T>
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
        /// <returns>The height of the tree.</returns>
        /// <exception cref="InvalidOperationException">The tree has no elements.</exception>
        public int GetHeight()
        {
            var root = GetRootOrThrowIfNoElements();
            var hasHeight = TryGetHeightRecursively(root, out var height);
            Debug.Assert(hasHeight, "hasHeight");
            return height;
        }

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
        /// <seealso cref="Traverse"/>
        [SuppressMessage("ReSharper", "HeuristicUnreachableCode", Justification = "Null action gracefully exits")]
        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse", Justification = "Null action gracefully exits")]
        [NotNull]
        public IEnumerable<T> TraverseRecursively(TraversalMode mode)
        {
            switch (mode)
            {
                case TraversalMode.InOrder:
                {
                    return TraverseInOrderRecursively(_root);
                }

                case TraversalMode.PreOrder:
                {
                    return TraversePreOrderRecursively(_root);
                }

                case TraversalMode.PostOrder: // "depth-first":
                {
                    return TraversePostOrderRecursively(_root);
                }

                case TraversalMode.LevelOrder: // "breadth-first":
                {
                    // This method can't be implemented recursively.
                    return TraverseLevelOrder(_root);
                }

                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, $"Unhandled traversal mode: {mode}");
                }
            }
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
                case TraversalMode.InOrder:
                {
                    return TraverseInOrder(_root);
                }

                case TraversalMode.PreOrder:
                {
                    return TraversePreOrder(_root);
                }

                case TraversalMode.PostOrder: // "depth-first":
                {
                    return TraversePostOrder(_root);
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

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public IEnumerator<T> GetEnumerator() => TraverseInOrder(_root).GetEnumerator();

        /// <summary>
        /// Returns the smallest element inserted to the tree.
        /// </summary>
        /// <returns>The smallest element.</returns>
        /// <exception cref="InvalidOperationException">The tree has no elements.</exception>
        [NotNull]
        public T GetSmallest()
        {
            var token = GetRootOrThrowIfNoElements();
            while (token.LeftNode != null)
            {
                token = token.LeftNode;
            }

            return token.Value;
        }

        /// <summary>
        /// Returns the largest element inserted to the tree.
        /// </summary>
        /// <returns>The largest element.</returns>
        /// <exception cref="InvalidOperationException">The tree has no elements.</exception>
        [NotNull]
        public T GetLargest()
        {
            var token = GetRootOrThrowIfNoElements();
            while (token.RightNode != null)
            {
                token = token.RightNode;
            }

            return token.Value;
        }

        /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if the tree has no elements.
        /// </summary>
        /// <exception cref="InvalidOperationException">The tree has no elements.</exception>
        [NotNull]
        private TreeNode<T> GetRootOrThrowIfNoElements()
        {
            if (_root != null)
            {
                return _root;
            }

            throw new InvalidOperationException("The tree needs to have at least one item.");
        }

        /// <summary>
        /// Traverses the tree's items in pre-order mode.
        /// </summary>
        /// <param name="node">The node to process.</param>
        /// <seealso cref="TraversePreOrder"/>
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
        /// Traverses the tree's items in pre-order mode.
        /// </summary>
        /// <param name="node">The node to process.</param>
        [NotNull]
        private IEnumerable<T> TraversePreOrder([CanBeNull] TreeNode<T> node)
        {
            if (node == null)
            {
                yield break;
            }

            var stack = new Stack<TreeNode<T>>();
            while (true)
            {
                while (node != null)
                {
                    // Process the current sub-tree's root node first.
                    yield return node.Value;

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

        /// <summary>
        /// Traverses the tree's items in in-order mode.
        /// </summary>
        /// <param name="node">The node to process.</param>
        [NotNull]
        private IEnumerable<T> TraverseInOrder([CanBeNull] TreeNode<T> node)
        {
            if (node == null)
            {
                yield break;
            }

            var stack = new Stack<TreeNode<T>>();
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
                yield return node.Value;

                // Descend into the right sub-tree.
                node = node.RightNode;
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
        private IEnumerable<T> TraversePostOrder([CanBeNull] TreeNode<T> node)
        {
            if (node == null)
            {
                yield break;
            }

            var stack = new Stack<TreeNode<T>>();

            // We use a token variable to test whether we're ascending
            // from a child back to its parent.
            TreeNode<T> previousNode = null;

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

            // The provided node is the root of a (possibly empty) sub-tree.
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