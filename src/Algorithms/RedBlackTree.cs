using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Widemeadows.Algorithms
{
#if false
    /// <summary>
    /// This class implements a red-black tree, i.e. a self-balancing
    /// binary search tree.
    /// </summary>
    public sealed class RedBlackTree<T>
    {
        /// <summary>
        /// The instance used to compare items of the tree. 
        /// </summary>
        [NotNull] 
        private readonly IComparer<T> _comparer;

        /// <summary>
        /// The root node.
        /// </summary>
        [CanBeNull]
        private Node _root;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedBlackTree{T}"/> class.
        /// </summary>
        /// <param name="comparer">The comparer to use.</param>
        public RedBlackTree([CanBeNull] IComparer<T> comparer = null)
        {
            _comparer = comparer ?? Comparer<T>.Default;
        }

        /// <summary>
        /// Gets the size of the tree.
        /// </summary>
        public int Count { get; private set; }
        
        /// <summary>
        /// Adds the specified <paramref name="item"/> to the tree.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add([NotNull] T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            Count += 1;
            
            // The root of the tree is always black.
            if (_root == null)
            {
                _root = new Node(item)
                {
                    Color = NodeColor.Black;
                };
                return;
            }
            
            // 
        }
        
        /// <summary>
        /// This struct implements a node in the tree.
        /// </summary>
        private sealed class Node
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Node"/> class.
            /// </summary>
            /// <param name="data">The data payload.</param>
            public Node([NotNull] T data)
            {
                Data = data;
            }

            /// <summary>
            /// Determines whether this node is red (if <see cref="NodeColor.Red"/>)
            /// or black (if <see cref="NodeColor.Black"/>).
            /// </summary>
            public NodeColor Color { get; set; }

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
            /// Gets the parent node.
            /// </summary>
            [CanBeNull]
            public WeakReference<Node> Parent { get; set; }
                        
            /// <summary>
            /// The data payload.
            /// </summary>
            [NotNull]
            public T Data { get; }
        }

        /// <summary>
        /// Describes the color a <see cref="Node"/>.
        /// </summary>
        private enum NodeColor
        {
            /// <summary>
            /// Indicates that the node is painted black.
            /// </summary>
            Black = 0,
            
            /// <summary>
            /// Indicates that the node is painted red.
            /// </summary>
            Red = 1
        }
    }
#endif
}