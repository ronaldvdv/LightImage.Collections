using DynamicData;
using DynamicData.Binding;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace DynamicData
{
    public static partial class ObservableChangeSetExtensions
    {
        public static void EditDiffOrdered<T>(this IExtendedList<T> list, IEnumerable<T> items, bool fullReplace = false)
        {
            if (list.SequenceEqual(items))
            {
                return;
            }

            if (fullReplace)
            {
                list.Clear();
                list.AddRange(items);
            }
            else
            {
                var add = items.Except(list).ToArray();
                var remove = list.Except(items).ToArray();

                list.AddRange(add);
                list.RemoveMany(remove);
            }
        }

        public static void EditDiffOrdered<T>(this ISourceList<T> list, IEnumerable<T> items, bool fullReplace = false)
        {
            list.Edit(ext => ext.EditDiffOrdered(items, fullReplace));
        }

        public static IObservable<IChangeSet<TValue>> ToZeroOneChangeSet<TValue>(this IObservable<TValue> source, bool useReplacements = true)
        {
            return new ToZeroOneChangeSet<TValue>(source, useReplacements).Run();
        }

        public static IObservable<IChangeSet<TResult>> TransformMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, ReadOnlyObservableCollection<TResult>> expr)
        {
            return source.ToZeroOneChangeSet(false).TransformMany(expr);
        }

        public static IObservable<bool> WatchBoolean<TValue, TKey>(this IObservableCache<TValue, TKey> cache, TKey key, Func<TValue, bool> func)
        {
            return cache.Watch(key).Scan(false, (state, change) =>
            {
                switch (change.Reason)
                {
                    case ChangeReason.Remove:
                        return false;

                    default:
                        return func(change.Current);
                }
            });
        }

        public static IObservable<TResult> WatchProperty<TSource, TResult>(this IObservable<TSource> source, Expression<Func<TSource, TResult>> expr, TResult defaultResult = default)
                    where TSource : INotifyPropertyChanged
        {
            return source.Select(item => item != null ? item.WhenValueChanged(expr) : Observable.Return(defaultResult)).Switch().DistinctUntilChanged();
        }
    }
}