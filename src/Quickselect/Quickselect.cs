using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace widemeadows.algorithms
{
    /// <summary>
    /// Class Quickselect.
    /// </summary>
    public sealed class Quickselect<TElement>
        where TElement : IComparable
    {
        /// <summary>
        /// The comparer
        /// </summary>
        [NotNull]
        private readonly IComparer<TElement> _comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Quickselect{TElement}"/> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        public Quickselect([CanBeNull] IComparer<TElement> comparer = null)
        {
            _comparer = comparer ?? Comparer<TElement>.Default;
        }

        /// <summary>
        /// Swaps the <paramref name="list"/> element at the <paramref name="sourceIndex"/> with the element
        /// at the <paramref name="targetIndex"/>.
        /// </summary>
        /// <typeparam name="TList">The type of the list.</typeparam>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="sourceIndex">Index of the source.</param>
        /// <param name="targetIndex">Index of the target.</param>
        private void SwapElementAt<TList>([NotNull] TList list, int sourceIndex, int targetIndex)
            where TList : IList<TElement>
        {
            var temp = list[targetIndex];
            list[targetIndex] = list[sourceIndex];
            list[sourceIndex] = temp;
        }

        /// <summary>
        /// Rearranges (i.e. groups) the <paramref name="list"/> into two parts, where the left part
        /// contains all elements smaller than the element at the <paramref name="pivotIndex"/> and the
        /// right part contains all elements greater than or equal to it.
        /// </summary>
        /// <typeparam name="TList">The type of the list.</typeparam>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="leftIndex">Start index in the list.</param>
        /// <param name="rightIndex">End index in the list.</param>
        /// <param name="pivotIndex">Index of the pivot element.</param>
        private int Partition<TList>([NotNull] TList list, int leftIndex, int rightIndex, int pivotIndex)
            where TList : IList<TElement>
        {
            Debug.Assert(pivotIndex >= 0 && pivotIndex < list.Count, "pivotIndex >= 0 && pivotIndex < list.Count");
            Debug.Assert(leftIndex >= 0 && leftIndex < list.Count,   "leftIndex >= 0 && leftIndex < list.Count");
            Debug.Assert(rightIndex >= 0 && rightIndex < list.Count, "rightIndex >= 0 && rightIndex < list.Count");

            // prefetch instance variables
            var comparer = _comparer;

            // fetch the pivot element and move it to the end of the list
            var pivotValue = list[pivotIndex];
            SwapElementAt(list, pivotIndex, rightIndex);

            // partially sort the list
            var storeIndex = leftIndex;
            for (int i = leftIndex; i < rightIndex; ++i)
            {
                var element = list[i];
                var elementIsSmallerThanPivot = comparer.Compare(element, pivotValue) < 0;
                if (!elementIsSmallerThanPivot) continue;

                SwapElementAt(list, storeIndex, i);
                ++storeIndex;
            }

            // move pivot to the final place and return the store index
            SwapElementAt(list, rightIndex, storeIndex);
            return storeIndex;
        }

        /// <summary>
        /// Selects a (random) pivot element between the <paramref name="leftIndex" /> and the <paramref name="rightIndex" />.
        /// </summary>
        /// <param name="leftIndex">Start index in the list.</param>
        /// <param name="rightIndex">End index in the list.</param>
        /// <returns>The pivot index.</returns>
        private int SelectPivotElement(int leftIndex, int rightIndex)
        {
            var random = new Random();
            return random.Next(leftIndex, rightIndex + 1);
        }

        /// <summary>
        /// Selects the (<paramref name="n" />+1)-th smallest element in the <paramref name="list" />.
        /// <para>
        /// As a side effect, returns a partially sorted list, where each element left of the element
        /// is smaller and each element right of it is greater or equal.
        /// </para>
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="n">The order (e.g. smallest element (<c>n=0</c>), 2nd smallest element (<c>n=1</c>), 3rd smallest (<c>n=2</c>), ...).</param>
        /// <returns>The index of the <c>(n+1)</c>-th smallest element.</returns>
        public int SelectRecursive<TList>([NotNull] TList list, int n)
            where TList : IList<TElement>
        {
            return SelectRecursive(list, 0, list.Count-1, n);
        }

        /// <summary>
        /// Selects the (<paramref name="n" />+1)-th smallest element in the <paramref name="list"/>
        /// between the <paramref name="leftIndex"/> and the <paramref name="rightIndex"/>.
        /// <para>
        /// As a side effect, returns a partially sorted list, where each element left of the element
        /// is smaller and each element right of it is greater or equal.
        /// </para>
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="leftIndex">Start index in the list.</param>
        /// <param name="rightIndex">End index in the list.</param>
        /// <param name="n">The order (e.g. smallest element (<c>n=0</c>), 2nd smallest element (<c>n=1</c>), 3rd smallest (<c>n=2</c>), ...).</param>
        /// <returns>The index of the <c>(n+1)</c>-th smallest element.</returns>
        private int SelectRecursive<TList>([NotNull] TList list, int leftIndex, int rightIndex, int n)
            where TList : IList<TElement>
        {
            Debug.Assert(n >= 0 && n < list.Count, "pivotIndex >= 0 && pivotIndex < list.Count");
            Debug.Assert(leftIndex >= 0 && leftIndex < list.Count, "leftIndex >= 0 && leftIndex < list.Count");
            Debug.Assert(rightIndex >= 0 && rightIndex < list.Count, "rightIndex >= 0 && rightIndex < list.Count");

            // when there only is one element, return it.
            if (leftIndex == rightIndex) return leftIndex;

            // select a pivot index and partition the list
            var pivotIndex = SelectPivotElement(leftIndex, rightIndex);
            pivotIndex = Partition(list, leftIndex, rightIndex, pivotIndex);

            // check if the pivot index is at the final position
            if (pivotIndex == n)
            {
                return pivotIndex;
            }

            // check if the pivot index is greater than the final position and
            // recurse into the left half, otherwise the pivot is smaller or equal
            // so we recurse into the right half.
            return n < pivotIndex
                ? SelectRecursive(list, leftIndex, pivotIndex - 1, n)
                : SelectRecursive(list, pivotIndex + 1, rightIndex, n);
        }

        /// <summary>
        /// Selects the (<paramref name="n" />+1)-th smallest element in the <paramref name="list" />.
        /// <para>
        /// As a side effect, returns a partially sorted list, where each element left of the element
        /// is smaller and each element right of it is greater or equal.
        /// </para>
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="n">The order (e.g. smallest element (<c>n=0</c>), 2nd smallest element (<c>n=1</c>), 3rd smallest (<c>n=2</c>), ...).</param>
        /// <returns>The index of the <c>(n+1)</c>-th smallest element.</returns>
        public int Select<TList>([NotNull] TList list, int n)
            where TList : IList<TElement>
        {
            return Select(list, 0, list.Count - 1, n);
        }

        /// <summary>
        /// Selects the (<paramref name="n" />+1)-th smallest element in the <paramref name="list"/>
        /// between the <paramref name="leftIndex"/> and the <paramref name="rightIndex"/>.
        /// <para>
        /// As a side effect, returns a partially sorted list, where each element left of the element
        /// is smaller and each element right of it is greater or equal.
        /// </para>
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="leftIndex">Start index in the list.</param>
        /// <param name="rightIndex">End index in the list.</param>
        /// <param name="n">The order (e.g. smallest element (<c>n=0</c>), 2nd smallest element (<c>n=1</c>), 3rd smallest (<c>n=2</c>), ...).</param>
        /// <returns>The index of the <c>(n+1)</c>-th smallest element.</returns>
        private int Select<TList>([NotNull] TList list, int leftIndex, int rightIndex, int n) where TList : IList<TElement>
        {
            while (true)
            {
                Debug.Assert(n >= 0 && n < list.Count, "pivotIndex >= 0 && pivotIndex < list.Count");
                Debug.Assert(leftIndex >= 0 && leftIndex < list.Count, "leftIndex >= 0 && leftIndex < list.Count");
                Debug.Assert(rightIndex >= 0 && rightIndex < list.Count, "rightIndex >= 0 && rightIndex < list.Count");

                // when there only is one element, return it.
                if (leftIndex == rightIndex) return leftIndex;

                // select a pivot index and partition the list
                var pivotIndex = SelectPivotElement(leftIndex, rightIndex);
                pivotIndex = Partition(list, leftIndex, rightIndex, pivotIndex);

                // check if the pivot index is at the final position
                if (pivotIndex == n)
                {
                    return pivotIndex;
                }

                // check if the pivot index is greater than the final position and
                // recurse into the left half
                if (n < pivotIndex)
                {
                    rightIndex = pivotIndex - 1;
                    continue;
                }

                // ... otherwise the pivot is smaller or equal so we recurse into the right half.
                leftIndex = pivotIndex + 1;
            }
        }
    }
}
