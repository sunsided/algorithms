using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace Widemeadows.Algorithms
{
    /// <summary>
    /// This class implements a regular binary search tree.
    /// </summary>
    public sealed class BinaryTree<T>
    {
        /// <summary>
        /// The instance used to compare items of the tree. 
        /// </summary>
        [NotNull] private readonly IComparer<T> _comparer;

        /// <summary>
        /// The root node.
        /// </summary>
        [CanBeNull] private Node _root;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryTree{T}"/> class.
        /// </summary>
        /// <param name="comparer">The comparer to use.</param>
        public BinaryTree([CanBeNull] IComparer<T> comparer = null)
        {
            _comparer = comparer ?? Comparer<T>.Default;
        }

        /// <summary>
        /// Gets the size of the tree.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Determines if the specified <paramref name="item"/> exists in the tree.
        /// </summary>
        /// <param name="item">The item to test for.</param>
        /// <returns><see langword="true"/> if the item exists in the tree; <see langword="false"/> otherwise.</returns>
        public bool Contains([CanBeNull] T item)
        {
            return FindItem(item) != null;
        }

        /// <summary>
        /// Adds the specified <paramref name="items"/> to the tree.
        /// </summary>
        /// <param name="items">The items to add.</param>
        /// <seealso cref="Add"/>
        public void AddRange([CanBeNull, ItemCanBeNull] IEnumerable<T> items)
        {
            if (items == null) return;
            foreach (var item in items)
            {
                Add(item);
            }
        }
        
        /// <summary>
        /// Adds the specified <paramref name="item"/> to the tree.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add([CanBeNull] T item)
        {
            Count += 1;

            // If no root exists we create it.
            if (_root == null)
            {
                _root = new Node(item);
                return;
            }

            // We now find the correct node to insert the item in (if it is a duplicate)
            // or a location at which to create a new node.
            var token = _root;
            while (token != null)
            {
                var ordering = Compare(item, token);
                if (ordering == 0)
                {
                    token.Items.Add(item);
                    return;
                }

                // If the item is smaller than the current node, it will have
                // to be added somewhere to the left. If no left child exists,
                // we create a new one and stop, otherwise we recurse deeper.
                if (ordering < 0)
                {
                    if (token.LeftChild == null)
                    {
                        token.LeftChild = new Node(item, parent: token);
                        return;
                    }

                    token = token.LeftChild;
                    continue;
                }

                // At this point the item is greater than the current node.
                // As before, we create a new node if no child exists.
                Debug.Assert(ordering > 0, "ordering > 0");
                if (token.RightChild == null)
                {
                    token.RightChild = new Node(item, parent: token);
                    return;
                }

                token = token.RightChild;
            }
        }
        
        /// <summary>
        /// Removes the specified <paramref name="item"/> from the tree.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns><see langword="true"/> if the item was removed; <see langword="false"/> otherwise.</returns>
        public bool Remove([CanBeNull] T item)
        {
            if (_root == null) return false;

            // Attempt to find the node containing the item.
            var node = FindItem(item);
            if (node == null) return false;
            Count -= 1;
            
            // Remove the item from the node and return if it is not the last one.
            Debug.Assert(node.Items.Count > 0, "node.Items.Count > 0");
            node.Items.RemoveAt(0);
            if (node.Items.Count > 0) return true;
            
            // At this point the node has to be removed. We will need to check and
            // update the parent as well.
            var parent = node.Parent;
            
            // If it is a leaf, we can just remove it from the tree.
            if (node.LeftChild == null && node.RightChild == null)
            {
                // If the node has no parent, we're on the root and the tree is now empty,
                // otherwise we'll remove the item from the parent.
                if (parent == null)
                {
                    _root = null;
                }
                else if (parent.LeftChild == node)
                {
                    parent.LeftChild = null;
                }
                else
                {
                    parent.RightChild = null;
                }
            }            
            // If the node has exactly one child, we can replace it with that child.
            else if (node.LeftChild != null ^ node.RightChild != null)
            {
                var replacement = node.LeftChild ?? node.RightChild;
                replacement.Parent = node.Parent;
                
                // If the current node has no parent, we're on the root and the tree is now replaced
                // with our subtree; if the node has a parent, we'll replace it directly.
                if (parent == null)
                {
                    _root = replacement;
                }
                else if (parent.LeftChild == node)
                {
                    parent.LeftChild = replacement;
                }
                else
                {
                    parent.RightChild = replacement;
                }
            }
            // If the node has two childs, we re going to replace it with the next-smallest item in the subtree.
            // For this, the smallest item on the right subtree is removed from its location
            // and replaces the current node. By doing so, we ensure all items on the left subtree
            // are smaller while all items on the right subtree are greater.
            else
            {
                Debug.Assert(node.LeftChild != null && node.RightChild != null, "node.LeftChild != null && node.RightChild != null");

                throw new NotImplementedException();
            }
            
            // Finally, we'll clean up the references so that the node can
            // be easily GC collected.
            Debug.Assert(node.Items.Count == 0, "node.Items.Count == 0");
            node.Parent = null;
            node.LeftChild = null;
            node.RightChild = null;

            return true;
        }

        /// <summary>
        /// Enumerates the items in ascending order.
        /// </summary>
        [NotNull]
        public IEnumerable<T> Enumerate()
        {          
            if (_root == null) yield break;

            var yieldSelfAndDescendRight = new Stack<Node>();
            var token = _root;
            do
            {
                // Descend into the leftmost leaf, registering the nodes that need
                // to yield their values and have their right child explored.
                while (token != null)
                {
                    yieldSelfAndDescendRight.Push(token);
                    token = token.LeftChild;
                }

                // We now take the deepest node from the stack and yield.
                Debug.Assert(yieldSelfAndDescendRight.Count > 0, "yieldSelfAndDescendRight.Count > 0");
                token = yieldSelfAndDescendRight.Pop();
                foreach (var item in token.Items)
                {
                    yield return item;
                }

                // Now we make the right child the new token in order to explore it.
                // The right child might be null, in which the next iteration would
                // pop a new item from the stack.
                token = token.RightChild;
            } while (yieldSelfAndDescendRight.Count > 0);
        }

        /// <summary>
        /// Recursively enumerates the items in ascending order-
        /// </summary>
        [NotNull]
        public IEnumerable<T> EnumerateRecursive()
        {
            if (_root == null) yield break;
            foreach (var item in EnumerateRecursive(_root))
            {
                yield return item;
            }
        }
        
        /// <summary>
        /// Recursively enumerates the items in ascending order.
        /// </summary>
        [NotNull]
        private IEnumerable<T> EnumerateRecursive([NotNull] Node root)
        {
            if (_root == null) yield break;
            
            // Enumerate from the left child.
            if (root.LeftChild != null)
            {
                foreach (var item in EnumerateRecursive(root.LeftChild))
                {
                    yield return item;
                }
            }
            
            // Enumerate the current items.
            foreach (var item in root.Items)
            {
                yield return item;
            }
            
            // Enumerate from the right child.
            if (root.RightChild != null)
            {
                foreach (var item in EnumerateRecursive(root.RightChild))
                {
                    yield return item;
                }
            }
        }
        
        /// <summary>
        /// Descends into the leftmost node starting from the specified <paramref name="root"/>.
        /// </summary>
        /// <param name="root">The node to start from.</param>
        /// <returns>The leftmost node.</returns>
        [NotNull]
        private Node DescendLeft([NotNull] Node root)
        {
            while (root.LeftChild != null)
            {
                root = root.LeftChild;
            }
            return root;
        }
        
        /// <summary>
        /// Helper function to compare a given search <paramref name="key"/>
        /// with the item(s) in a specified <paramref name="node"/>.
        /// </summary>
        /// <remarks>
        /// We use this helper function since a node can contain more than one (identical) item.
        /// A node cannot have zero items though, as this implies the node should not exist
        /// in the first place.
        /// </remarks>
        /// <param name="key">The search key.</param>
        /// <param name="node">The node to compare with.</param>
        /// <returns>
        /// <c>0</c> if the <paramref name="key"/> is contained in the specified <paramref name="node"/>,
        /// negative if the <paramref name="key"/> is smaller than the <paramref name="node"/>'s value and
        /// positive if the <paramref name="key"/> is bigger.</returns>
        private int Compare([CanBeNull] T key, [NotNull] Node node)
        {
            Debug.Assert(node.Items.Count > 0, "token.Items.Count > 0");
            return _comparer.Compare(key, node.Items.First());   
        }
        
        /// <summary>
        /// Attempts to find the node containing the given <paramref name="key"/> item.
        /// </summary>
        /// <param name="key">The key to search for.</param>
        /// <returns>
        /// The <see cref="Node"/> instance containing the item that equals the search <paramref name="key"/>
        /// or <see langword="null"/> if no node was found.
        /// </returns>
        [CanBeNull]
        private Node FindItem([CanBeNull] T key)
        {           
            // Starting from the root node, we iteratively go through either
            // the left or right child depending on the comparison outcome.
            // If the current comparison yields equality, we return the matching node.
            // If no node matched (or no node existed), we return null.
            var token = _root;
            while (token != null)
            {
                var ordering = Compare(key, token);
                if (ordering == 0) return token;
                token = ordering < 0 ? token.LeftChild : token.RightChild;
            }

            // At this point no item was not found.
            return null;
        }
        
        /// <summary>
        /// This struct implements a node in the tree.
        /// </summary>
        private sealed class Node
        {
            /// <summary>
            /// A reference to the parent node.
            /// </summary>
            [NotNull]
            private WeakReference<Node> _parent;

            /// <summary>
            /// Initializes a new instance of the <see cref="Node"/> class.
            /// </summary>
            /// <param name="data">The data payload.</param>
            /// <param name="parent">The parent of the node.</param>
            public Node([CanBeNull] T data, [CanBeNull] Node parent = null)
            {
                Items = new Collection<T> {data};
                _parent = new WeakReference<Node>(parent);
            }

            /// <summary>
            /// Gets the left child node.
            /// </summary>
            [CanBeNull]
            public Node LeftChild { get; set; }
            
            /// <summary>
            /// Gets the right child node.
            /// </summary>
            [CanBeNull]
            public Node RightChild { get; set; }
            
            /// <summary>
            /// Gets or sets the parent node.
            /// </summary>
            [CanBeNull]
            public Node Parent 
            {
                get => _parent.TryGetTarget(out var parent) ? parent : null;
                set => _parent = new WeakReference<Node>(value);
            }
                        
            /// <summary>
            /// The data payload.
            /// </summary>
            [NotNull]
            public Collection<T> Items { get; }
        }
    }
}