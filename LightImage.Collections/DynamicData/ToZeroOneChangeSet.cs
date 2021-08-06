using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace DynamicData
{
    internal class ToZeroOneChangeSet<T>
    {
        private readonly IObservable<T> _source;
        private readonly bool _useReplacements;

        public ToZeroOneChangeSet(IObservable<T> source, bool useReplacements = true)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _useReplacements = useReplacements;
        }

        public IObservable<IChangeSet<T>> Run()
        {
            return Observable.Create<IChangeSet<T>>(observer =>
            {
                var comparer = EqualityComparer<T>.Default;

                var latest = default(T);
                return _source
                .Where(value => !comparer.Equals(latest, value))
                .Select(value =>
                {
                    var changes = new ChangeSet<T>();
                    if (_useReplacements && latest != null && value != null)
                    {
                        changes.Add(new Change<T>(ListChangeReason.Replace, value, latest, 0));
                    }
                    else
                    {
                        if (latest != null)
                            changes.Add(new Change<T>(ListChangeReason.Remove, latest, 0));
                        if (value != null)
                            changes.Add(new Change<T>(ListChangeReason.Add, value, 0));
                    }
                    latest = value;
                    return changes;
                }).SubscribeSafe(observer);
            });
        }
    }
}