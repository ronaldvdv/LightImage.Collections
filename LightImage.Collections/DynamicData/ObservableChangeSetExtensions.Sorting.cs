using LightImage.Collections.Sorting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;

namespace DynamicData
{
    public static partial class ObservableChangeSetExtensions
    {
        /// <summary>
        /// Sorts a <paramref name="source"/> sequence by comparing the items by a specific <paramref name="property"/> and optionally using a specific <paramref name="comparer"/>.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <typeparam name="U">The value of the property which is used for comparing items.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="property">The property to sort by.</param>
        /// <param name="comparer">The comparer to be used for property values.</param>
        /// <returns>The sorted sequence.</returns>
        public static IObservable<IChangeSet<T>> SortBy<T, U>(this IObservable<IChangeSet<T>> source, Expression<Func<T, U>> property, IComparer<U> comparer = null)
            where T : INotifyPropertyChanged
        {
            var resort = source.WhenPropertyChanged(property).Select(_ => Unit.Default);
            var sorter = CustomSortExpressionComparer<T>.Ascending(property.Compile(), comparer);
            return source.Sort(sorter, resort: resort);
        }

        /// <summary>
        /// Sorts a <paramref name="source"/> sequence by comparng the items by a specific string <paramref name="property"/> with natural sorting using <see cref="NaturalSorter"/>.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="property">The property to sort by.</param>
        /// <returns>The sorted sequence.</returns>
        public static IObservable<IChangeSet<T>> SortNaturalBy<T>(this IObservable<IChangeSet<T>> source, Expression<Func<T, string>> property)
            where T : INotifyPropertyChanged
        {
            return source.SortBy(property, NaturalSorter.Instance);
        }
    }
}
