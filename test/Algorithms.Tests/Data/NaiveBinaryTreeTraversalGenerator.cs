using System.Collections.Generic;
using Widemeadows.Algorithms.Tests.Model;
using Widemeadows.Algorithms.Trees;

namespace Widemeadows.Algorithms.Tests.Data
{
    /// <summary>
    /// Test case generator for <see cref="Widemeadows.Algorithms.Trees.NaiveBinaryTree{T}" /> tests.
    /// </summary>
    public sealed class NaiveBinaryTreeTraversalGenerator : TestCaseGeneratorBase
    {
        private readonly List<NumericalItem> _items;

        private readonly NumericalItem _a;
        private readonly NumericalItem _b;
        private readonly NumericalItem _d;
        private readonly NumericalItem _h;
        private readonly NumericalItem _k;
        private readonly NumericalItem _e;
        private readonly NumericalItem _c;
        private readonly NumericalItem _f;
        private readonly NumericalItem _i;
        private readonly NumericalItem _j;
        private readonly NumericalItem _g;

        /// <summary>
        /// Initializes a new instance of the <see cref="NaiveBinaryTreeTraversalGenerator"/> class.
        /// </summary>
        public NaiveBinaryTreeTraversalGenerator()
        {
            //                 A
            //        B                  C
            //   D         E       F           G
            //     H             I   J
            //       K

            _a = new NumericalItem(0);

            // Left subtree of A
            _b = new NumericalItem(-10);

            // Left subtree of B
            _d = new NumericalItem(-15);
            _h = new NumericalItem(-13);
            _k = new NumericalItem(-12);

            // Right subtree of B
            _e = new NumericalItem(-5);

            // Right subtree of A
            _c = new NumericalItem(10);

            // Left subtree of C
            _f = new NumericalItem(5);
            _i = new NumericalItem(3);
            _j = new NumericalItem(7);

            // Right subtree of C
            _g = new NumericalItem(15);

            // Construct the tree
            _items = new List<NumericalItem>
            {
                _a, _b, _c, _d, _e, _f, _g, _h, _i, _j, _k
            };
        }

        /// <inheritdoc cref="TestCaseGeneratorBase.GetEnumerator"/>
        public override IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                _items, TraversalMode.PreOrder,
                new[] { _a, _b, _d, _h, _k, _e, _c, _f, _i, _j, _g }
            };

            yield return new object[]
            {
                _items, TraversalMode.InOrder,
                new[] { _d, _h, _k, _b, _e, _a, _i, _f, _j, _c, _g }
            };

            yield return new object[]
            {
                _items, TraversalMode.PostOrder,
                new[] { _k, _h, _d, _e, _b, _i, _j, _f, _g, _c, _a }
            };

            yield return new object[]
            {
                _items, TraversalMode.LevelOrder,
                new[] { _a, _b, _c, _d, _e, _f, _g, _h, _i, _j, _k }
            };
        }
    }
}