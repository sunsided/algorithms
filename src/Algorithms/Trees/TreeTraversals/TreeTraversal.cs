using System.Collections.Generic;
using Widemeadows.Algorithms.Properties;

namespace Widemeadows.Algorithms.Trees.TreeTraversals
{
    /// <summary>
    /// Base class for tree traversals.
    /// </summary>
    /// <typeparam name="TNode">The type of the nodes to traverse.</typeparam>
    /// <typeparam name="TData">The type of the data to emit.</typeparam>
    // ReSharper disable once CA1710
    internal abstract class TreeTraversal<TNode, TData>
    {
        /// <summary>
        /// Traverses the nodes starting from the specified root <paramref name="node"/>.
        /// </summary>
        /// <param name="node">The root node to start traversal from.</param>
        /// <returns>An enumerable of all traversed data items.</returns>
        [NotNull]
        public abstract IEnumerable<TData> Traverse([CanBeNull] TNode node);
    }
}