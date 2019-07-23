using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;

namespace Widemeadows.Algorithms.Trees
{
    /// <summary>
    /// A naive binary tree implementation.
    /// </summary>
    /// <typeparam name="T">The type of the item.</typeparam>
    [SuppressMessage("ReSharper", "CA1710", Justification = "Tree shouldn't be named collection")]
    public sealed class NaiveBinaryTree<T> : IReadOnlyCollection<T>
        where T : IComparable<T>
    {
        /// <summary>
        /// The root node.
        /// </summary>
        [CanBeNull]
        private BinaryTreeNode<T> _root;

        /// <summary>
        /// The height (or depth) of the tree, as a cached value.
        /// </summary>
        private int? _height;

        /// <summary>
        /// The size of the tree.
        /// </summary>
        private int _count;

        /// <summary>
        /// Gets the number of items in the tree.
        /// </summary>
        /// <value>The number of items.</value>
        /// <seealso cref="CalculateSize"/>
        [SuppressMessage("ReSharper", "ConvertToAutoPropertyWithPrivateSetter", Justification = "Consistency with Height")]
        public int Count => _count;

        /// <summary>
        /// Gets the height (or depth) of the tree.
        /// </summary>
        /// <value>The number of items.</value>
        /// <seealso cref="CalculateHeight"/>
        public int Height => _height ?? ThrowForNoElements();

        /// <summary>
        /// Calculates the number of items in the tree.
        /// </summary>
        /// <remarks>
        /// Note that there is no reason to actually dynamically calculate the size
        /// if a storage field such as <see cref="Count"/> can be used.
        /// This implementation is only here for reference purposes.
        /// </remarks>
        /// <returns>The size of the tree.</returns>
        /// <seealso cref="Count"/>
        /// <seealso cref="CalculateSizeRecursively"/>
        public int CalculateSize() => TraversePreOrder(_root).Count();

        /// <summary>
        /// Recursively calculates the number of items in the tree.
        /// </summary>
        /// <remarks>
        /// Note that there is no reason to actually dynamically calculate the size
        /// if a storage field such as <see cref="Count"/> can be used.
        /// This implementation is only here for reference purposes.
        /// </remarks>
        /// <returns>The size of the tree.</returns>
        /// <seealso cref="Count"/>
        /// <seealso cref="CalculateSize"/>
        public int CalculateSizeRecursively() => GetSizeRecursively(_root);

        /// <summary>
        /// Calculates the height (or depth) of the tree.
        /// </summary>
        /// <returns>The height of the tree.</returns>
        /// <exception cref="InvalidOperationException">The tree has no elements.</exception>
        /// <seealso cref="Height"/>
        public int CalculateHeight()
        {
            var root = _root;
            if (root == null) ThrowForNoElements();

            var hasHeight = TryGetHeightRecursively(root, out var height);
            Debug.Assert(hasHeight, "hasHeight");
            return height;
        }

        /// <summary>
        /// Inserts a node into the tree.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add([NotNull] in T item)
        {
            // We simply track the number of items by incrementing the size counter.
            ++_count;

            // We initialize the new potential height to zero and increase it as we go.
            var height = 0;

            if (_root == null)
            {
                _root = new BinaryTreeNode<T>(item);
                _height = height;
                return;
            }

            var token = _root;
            while (true)
            {
                // Increase the depth with every step we go deeper.
                ++height;

                if (item.CompareTo(token.Value) <= 0)
                {
                    if (token.LeftNode == null)
                    {
                        token.LeftNode = new BinaryTreeNode<T>(item);
                        break;
                    }

                    // descend left
                    token = token.LeftNode;
                }
                else
                {
                    if (token.RightNode == null)
                    {
                        token.RightNode = new BinaryTreeNode<T>(item);
                        break;
                    }

                    // descend right
                    token = token.RightNode;
                }
            }

            // The new tree height is the maximum of the current tree
            // height and the height of the newly inserted node.
            _height = Math.Max(_height ?? 0, height);
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

        /// <summary>
        /// Traverses the tree's leaves in ascending order.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> enumerating the leaves.</returns>
        /// <seealso cref="TraversalMode.InOrder"/>
        /// <seealso cref="Traverse"/>
        [NotNull]
        public IEnumerable<T> TraverseLeaves()
        {
            // This method uses recursion-free in-order traversal to
            // find all the leaf nodes. See the appropriate traversal methods for more information.
            var node = _root;
            if (node == null)
            {
                yield break;
            }

            var stack = new Stack<BinaryTreeNode<T>>();
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

                // Emit only if the node is a leaf.
                if (node.LeftNode == null && node.RightNode == null)
                {
                    yield return node.Value;
                }

                // Descend into the right sub-tree.
                node = node.RightNode;
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
            var token = _root;
            if (token == null) ThrowForNoElements();

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
            var token = _root;
            if (token == null) ThrowForNoElements();

            while (token.RightNode != null)
            {
                token = token.RightNode;
            }

            return token.Value;
        }

        /// <summary>
        /// Removes all items from the tree.
        /// </summary>
        public void Clear()
        {
            if (_root == null)
            {
                Debug.Assert(_count == 0, "_count == 0");
                Debug.Assert(_height == null || _height == 0, "_height == null || _height == 0");
                return;
            }

            // In theory, we could set the root node to null and be done with it.
            // However, this results in a big connected graph of nodes that the
            // Garbage Collection has to deal with.
            var stack = new Stack<BinaryTreeNode<T>>();
            stack.Push(_root);

            while (stack.Count > 0)
            {
                var node = stack.Pop();
                if (node == null)
                {
                    continue;
                }

                stack.Push(node.LeftNode);
                stack.Push(node.RightNode);

                node.LeftNode = null;
                node.RightNode = null;
            }

            // Reset counters and root
            _root = null;
            _height = null;
            _count = 0;
        }

        /// <summary>
        /// Returns the deepest node of the tree.
        /// </summary>
        /// <returns>The deepest node.</returns>
        /// <exception cref="InvalidOperationException">The tree has no elements.</exception>
        public T GetDeepestNode()
        {
            if (_root == null) ThrowForNoElements();

            var maxDepth = 0;
            var deepest = _root;

            var stack = new Stack<(int height, BinaryTreeNode<T> node)>();
            stack.Push((height: 0, node: _root));

            while (stack.Count > 0)
            {
                var (nodeDepth, node) = stack.Pop();

                if (node.LeftNode != null)
                {
                    stack.Push((height: nodeDepth + 1, node: node.LeftNode));
                }

                if (node.RightNode != null)
                {
                    stack.Push((height: nodeDepth + 1, node: node.RightNode));
                }

                // We now compare the current depth against the previously registered best depth.
                // Note that if any of the above conditions were true, this node can't be the deepest one;
                // however, to simplify the code, we ignore that check here.
                if (nodeDepth <= maxDepth) continue;

                maxDepth = nodeDepth;
                deepest = node;
            }

            return deepest.Value;
        }

        /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if the tree has no elements.
        /// </summary>
        /// <remarks>
        /// This method implements a small hack to work around a compiler issue where methods
        /// won't get inlined if they contain a <see langword="throw"/> statement. By outsourcing
        /// the <see langword="throw"/> into a separate method, the calling code is available for inlining.
        /// </remarks>
        /// <returns>Never returns, but return type is required to suppress compiler errors.</returns>
        /// <exception cref="InvalidOperationException">The tree has no elements.</exception>
        [ContractAnnotation("=>halt")]
        private static int ThrowForNoElements() => throw new InvalidOperationException("The tree needs to have at least one item.");

        /// <summary>
        /// Traverses the tree's items in pre-order mode.
        /// </summary>
        /// <param name="node">The node to process.</param>
        /// <seealso cref="TraversePreOrder"/>
        [NotNull]
        private static IEnumerable<T> TraversePreOrderRecursively([CanBeNull] BinaryTreeNode<T> node)
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
        private static IEnumerable<T> TraversePreOrder([CanBeNull] BinaryTreeNode<T> node)
        {
            if (node == null)
            {
                yield break;
            }

            var stack = new Stack<BinaryTreeNode<T>>();
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
        private static IEnumerable<T> TraverseInOrder([CanBeNull] BinaryTreeNode<T> node)
        {
            if (node == null)
            {
                yield break;
            }

            var stack = new Stack<BinaryTreeNode<T>>();
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
        private static IEnumerable<T> TraverseInOrderRecursively([CanBeNull] BinaryTreeNode<T> node)
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
        private static IEnumerable<T> TraversePostOrder([CanBeNull] BinaryTreeNode<T> node)
        {
            if (node == null)
            {
                yield break;
            }

            var stack = new Stack<BinaryTreeNode<T>>();

            // We use a token variable to test whether we're ascending
            // from a child back to its parent.
            BinaryTreeNode<T> previousNode = null;

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
        private static IEnumerable<T> TraversePostOrderRecursively([CanBeNull] BinaryTreeNode<T> node)
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
        private static IEnumerable<T> TraverseLevelOrder([CanBeNull] BinaryTreeNode<T> node)
        {
            if (node == null)
            {
                yield break;
            }

            var expansionList = new Queue<BinaryTreeNode<T>>();
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
        private static int GetSizeRecursively([CanBeNull] BinaryTreeNode<T> node)
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
        private static bool TryGetHeightRecursively([CanBeNull] BinaryTreeNode<T> node, out int height)
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

            // ReSharper disable once InvertIf
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