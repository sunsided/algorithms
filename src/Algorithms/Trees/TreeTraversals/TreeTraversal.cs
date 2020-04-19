using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Widemeadows.Algorithms.Trees.TreeTraversals
{
    /// <summary>
    /// Base class for tree traversals.
    /// </summary>
    /// <typeparam name="TNode">The type of the nodes to traverse.</typeparam>
    internal abstract class TreeTraversal<TNode>
    {
        /// <summary>
        /// Traverses the nodes starting from the specified root <paramref name="node"/>.
        /// </summary>
        /// <param name="node">The root node to start traversal from.</param>
        /// <returns>An enumerable of all traversed data items.</returns>
        public abstract IEnumerable<TNode> TraverseNodes([AllowNull] TNode node);
    }
}