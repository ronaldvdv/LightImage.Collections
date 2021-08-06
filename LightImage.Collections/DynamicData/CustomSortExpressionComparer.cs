using DynamicData.Binding;
using System;
using System.Collections.Generic;

namespace DynamicData
{
    public class CustomSortExpressionComparer<T> : List<IComparer<T>>, IComparer<T>
    {
        // TODO Create pull request to update DynamicData SortExpressionComparer
        public static CustomSortExpressionComparer<T> Ascending<U>(Func<T, U> expression, IComparer<U> comparer = null)
        {
            return new CustomSortExpressionComparer<T> { new CustomSortExpression<U>(expression, comparer: comparer) };
        }

        public static CustomSortExpressionComparer<T> Descending<U>(Func<T, U> expression, IComparer<U> comparer = null)
        {
            return new CustomSortExpressionComparer<T> { new CustomSortExpression<U>(expression, SortDirection.Descending, comparer: comparer) };
        }

        public int Compare(T x, T y)
        {
            foreach (var item in this)
            {
                var result = item.Compare(x, y);
                if (result == 0)
                {
                    continue;
                }
                return result;
            }
            return 0;
        }

        public CustomSortExpressionComparer<T> ThenByAscending<U>(Func<T, U> expression, IComparer<U> comparer = null)
        {
            Add(new CustomSortExpression<U>(expression, comparer: comparer));
            return this;
        }

        public CustomSortExpressionComparer<T> ThenByDescending<U>(Func<T, U> expression, IComparer<U> comparer = null)
        {
            Add(new CustomSortExpression<U>(expression, SortDirection.Descending, comparer: comparer));
            return this;
        }

        private class CustomSortExpression<U> : IComparer<T>
        {
            private readonly IComparer<U> _comparer;
            private readonly SortDirection _direction;
            private readonly Func<T, U> _expression;

            public CustomSortExpression(Func<T, U> expression, SortDirection direction = SortDirection.Ascending, IComparer<U> comparer = null)
            {
                _expression = expression;
                _comparer = comparer ?? Comparer<U>.Default;
                _direction = direction;
            }

            public int Compare(T x, T y)
            {
                var valueX = _expression(x);
                var valueY = _expression(y);
                var result = _comparer.Compare(valueX, valueY);
                return _direction == SortDirection.Ascending ? result : -result;
            }
        }
    }
}