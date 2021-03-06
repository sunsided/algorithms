using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using JetBrains.Annotations;
using Widemeadows.Algorithms.Trees.TreeTraversals;
using Widemeadows.Algorithms.Trees.TreeTraversals.BinaryTree;

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
        /// Lookup for non-recursive tree traversals by <see cref="TraversalMode"/> value.
        /// </summary>
        private static readonly Dictionary<TraversalMode, TreeTraversal<BinaryTreeNode<T>>> Traversals =
            new Dictionary<TraversalMode, TreeTraversal<BinaryTreeNode<T>>>(4)
            {
                [TraversalMode.InOrder] = new InOrderBinaryTreeTraversal<T>(),
                [TraversalMode.PostOrder] = new PostOrderBinaryTreeTraversal<T>(),
                [TraversalMode.PreOrder] = new PreOrderBinaryTreeTraversal<T>(),
                [TraversalMode.LevelOrder] = new LevelOrderBinaryTreeTraversal<T>(),
            };

        /// <summary>
        /// Lookup for recursive tree traversals by <see cref="TraversalMode"/> value.
        /// </summary>
        private static readonly Dictionary<TraversalMode, TreeTraversal<BinaryTreeNode<T>>> RecursiveTraversals =
            new Dictionary<TraversalMode, TreeTraversal<BinaryTreeNode<T>>>(4)
            {
                [TraversalMode.InOrder] = new RecursiveInOrderBinaryTreeTraversal<T>(),
                [TraversalMode.PostOrder] = new RecursivePostOrderBinaryTreeTraversal<T>(),
                [TraversalMode.PreOrder] = new RecursivePreOrderBinaryTreeTraversal<T>(),

                // This method can't be implemented recursively.
                [TraversalMode.LevelOrder] = new LevelOrderBinaryTreeTraversal<T>(),
            };

        /// <summary>
        /// The root node.
        /// </summary>
        private BinaryTreeNode<T>? _root;

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
        /// <exception cref="InvalidOperationException">The tree has no elements.</exception>
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
        [Pure]
        public int CalculateSize() =>
            // Using non-recursive pre-order traversal here because
            // it is the simplest non-recursive traversal operation.
            Traverse(TraversalMode.PreOrder).Count();

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
        [Pure]
        public int CalculateSizeRecursively() => GetSizeRecursively(_root);

        /// <summary>
        /// Calculates the height (or depth) of the tree.
        /// </summary>
        /// <returns>The height of the tree.</returns>
        /// <exception cref="InvalidOperationException">The tree has no elements.</exception>
        /// <seealso cref="Height"/>
        [Pure]
        public int CalculateHeight()
        {
            var root = ThrowForNoElements(_root);
            Debug.Assert(root != null, "root != null");

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
            // We expect that the first check will be optimized away by the JITter.
            if (typeof(T).IsClass)
            {
                if (ReferenceEquals(item, null)) throw new ArgumentNullException(nameof(item));
            }

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
        /// Inserts a range of items.
        /// </summary>
        /// <param name="items">The items to insert</param>
        public void AddRange([ItemNotNull, InstantHandle] IEnumerable<T> items)
        {
            if (ReferenceEquals(items, null)) throw new ArgumentNullException(nameof(items));
            foreach (var item in items)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Determines whether the tree contains the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><see langword="true"/> if the tree contains the item; <see langword="false"/> otherwise.</returns>
        [Pure]
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
        [Pure]
        public IEnumerable<T> Traverse(TraversalMode mode) => TraverseNodes(_root, mode).Select(n => n.Value);

        /// <summary>
        /// Traverses the tree's items in the specified traversal order.
        /// </summary>
        /// <param name="mode">The traversal order.</param>
        /// <returns>An <see cref="IEnumerable{T}"/>.</returns>
        /// <seealso cref="Traverse"/>
        [Pure]
        public IEnumerable<T> TraverseRecursively(TraversalMode mode)
        {
            if (RecursiveTraversals.TryGetValue(mode, out var traversal))
            {
                return traversal.TraverseNodes(_root).Select(n => n.Value);
            }

            throw new ArgumentOutOfRangeException(nameof(mode), mode, $"Unhandled traversal mode: {mode}");
        }

        /// <summary>
        /// Traverses the tree's leaves in ascending order.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> enumerating the leaves.</returns>
        /// <seealso cref="TraversalMode.InOrder"/>
        /// <seealso cref="Traverse"/>
        [Pure]
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
        [Pure]
        public IEnumerator<T> GetEnumerator() => Traverse(TraversalMode.InOrder).GetEnumerator();

        /// <summary>
        /// Returns the smallest element inserted to the tree.
        /// </summary>
        /// <returns>The smallest element.</returns>
        /// <exception cref="InvalidOperationException">The tree has no elements.</exception>
        [Pure]
        public T GetSmallest()
        {
            var token = ThrowForNoElements(_root);
            Debug.Assert(token != null, "token != null");

            // ReSharper disable once PossibleNullReferenceException
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
        [Pure]
        public T GetLargest()
        {
            var token = ThrowForNoElements(_root);
            Debug.Assert(token != null, "token != null");

            // ReSharper disable once PossibleNullReferenceException
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
            var stack = new Stack<BinaryTreeNode<T>?>();
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
        [Pure]
        public T GetDeepestNode()
        {
            var root = ThrowForNoElements(_root);

            var maxDepth = 0;
            var deepest = root;

            var stack = new Stack<(int height, BinaryTreeNode<T> node)>();
            stack.Push((height: 0, node: root));

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

        /// <summary>
        /// Calculates the number of leaves of the tree by iterating it.
        /// </summary>
        /// <returns>The number of leaves of the tree.</returns>
        [Pure]
        public int CalculateNumberOfLeaves() => TraverseNodes(_root, TraversalMode.InOrder).Count(n => n.IsLeaf);

        /// <summary>
        /// Calculates the number of full nodes of the tree by iterating it.
        /// </summary>
        /// <returns>The number of full nodes of the tree.</returns>
        [Pure]
        public int CalculateNumberOfFullNodes() => TraverseNodes(_root, TraversalMode.InOrder).Count(n => n.IsFull);

        /// <summary>
        /// Calculates the number of half nodes of the tree by iterating it.
        /// </summary>
        /// <returns>The number of half nodes of the tree.</returns>
        [Pure]
        public int CalculateNumberOfHalfNodes() => TraverseNodes(_root, TraversalMode.InOrder).Count(n => n.IsHalf);

        /// <summary>
        /// Determines whether two trees are structurally similar.
        /// </summary>
        /// <remarks>
        /// Two trees are structurally similar if they have the same shape, but may have different values.
        /// </remarks>
        /// <param name="otherTree">The tree to compare with.</param>
        /// <returns><see langword="true"/> if the trees are structurally similar; <see langword="false"/> otherwise.</returns>
        public bool IsStructurallyIdenticalTo(NaiveBinaryTree<T>? otherTree)
        {
            // We're expanding the nodes in breadth-first order in order to
            // early stop when there are structural differences "up" in the tree.
            var explore = new Queue<(BinaryTreeNode<T>? root1, BinaryTreeNode<T>? root2)>();
            explore.Enqueue((_root, otherTree?._root));

            while (explore.Count > 0)
            {
                var (root1, root2) = explore.Dequeue();

                // If the two roots are reference identical, they're either the exact same
                // nodes, or they are both null. In any case, there's no point checking their
                // children, as they must be identical, too.
                if (ReferenceEquals(root1, root2)) continue;

                // If any of the nodes is null though while the other one isn't, the
                // trees differ in structure.
                if (ReferenceEquals(root1, null) || ReferenceEquals(root2, null)) return false;

                // Descend into the child nodes.
                explore.Enqueue((root1.LeftNode, root2.LeftNode));
                explore.Enqueue((root1.RightNode, root2.RightNode));
            }

            // At this point, all nodes were compared and no error occured.
            return true;
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
        /// <exception cref="InvalidOperationException">The tree has no elements.</exception>
        /// <seealso cref="ThrowForNoElements"/>
        [ContractAnnotation("value:null => halt; value:notnull => notnull")]
        [Pure]
        [return:NotNull]
        private static TValue ThrowForNoElements<TValue>([MaybeNull] TValue value)
        {
            if (value != null) return value;
            throw new InvalidOperationException("The tree needs to have at least one item.");
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if the tree has no elements.
        /// </summary>
        /// <remarks>
        /// This method implements a small hack to work around a compiler issue where methods
        /// won't get inlined if they contain a <see langword="throw"/> statement. By outsourcing
        /// the <see langword="throw"/> into a separate method, the calling code is available for inlining.
        /// </remarks>
        /// <returns>Never returns, but the return value is required for the compiler.</returns>
        /// <exception cref="InvalidOperationException">The tree has no elements.</exception>
        /// <seealso cref="ThrowForNoElements{T}"/>
        [ContractAnnotation("=> halt")]
        [Pure]
        private static int ThrowForNoElements() => throw new InvalidOperationException("The tree needs to have at least one item.");

        /// <summary>
        /// Calculates the number of items in the tree.
        /// </summary>
        /// <returns>The size of the tree.</returns>
        [Pure]
        private static int GetSizeRecursively(BinaryTreeNode<T>? node)
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
        [Pure]
        private static bool TryGetHeightRecursively(BinaryTreeNode<T>? node, out int height)
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

        /// <summary>
        /// Traverses the tree's items in the specified traversal order.
        /// </summary>
        /// <param name="node">The root node.</param>
        /// <param name="mode">The traversal order.</param>
        /// <returns>An <see cref="IEnumerable{T}"/>.</returns>
        [Pure]
        private static IEnumerable<BinaryTreeNode<T>> TraverseNodes(BinaryTreeNode<T>? node, TraversalMode mode)
        {
            if (Traversals.TryGetValue(mode, out var traversal))
            {
                return traversal.TraverseNodes(node);
            }

            throw new ArgumentOutOfRangeException(nameof(mode), mode, $"Unhandled traversal mode: {mode}");
        }

        /// <summary>
        /// Calculates the diameter (or width) of the tree.
        /// </summary>
        /// <remarks>
        ///    The diameter of a tree is the longest distance between two leaves.
        /// </remarks>
        /// <returns>The diameter of the tree.</returns>
        public int CalculateDiameter()
        {
            var diameter = 0;
            CalculateDiameter(_root, ref diameter);
            return diameter;
        }

        /// <inheritdoc cref="CalculateDiameter"/>
        /// <param name="root">The start node.</param>
        /// <param name="maxDiameter">The previously known maximum diameter.</param>
        /// <returns>The diameter of the subtree rooted at <paramref name="root"/>.</returns>
        private static int CalculateDiameter(BinaryTreeNode<T>? root, ref int maxDiameter)
        {
            if (root == null) return 0;

            // Calculate left and right sub-tree heights.
            var leftDiameter = CalculateDiameter(root.LeftNode, ref maxDiameter);
            var rightDiameter = CalculateDiameter(root.RightNode, ref maxDiameter);

            // Calculate diameter through the current node.
            var subtreeDiameter = leftDiameter + rightDiameter + 1;

            // Update maximum diameter.
            maxDiameter = Math.Max(maxDiameter, subtreeDiameter);

            // Return the height of the subtree rooted at current node.
            return Math.Max(leftDiameter, rightDiameter) + 1;
        }
    }
}