using Leap;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeapConsole
{
    public class FramesObservable : IObservable<Frame>
    {
        HashSet<IObserver<Frame>> _observers = new HashSet<IObserver<Frame>>();

        public void Emit(Frame frame)
        {
            Parallel.ForEach<IObserver<Frame>>(_observers, o => o.OnNext(frame));
        }

        public IDisposable Subscribe(IObserver<Frame> observer)
        {
            if (observer == null) return null;

            _observers.Add(observer);
            return new SubscriptionToken<Frame>(observer, this.Unsubscribe);
        }

        public void Unsubscribe(IObserver<Frame> observer)
        {
            if (observer != null && _observers.Contains(observer))
            {
                observer.OnCompleted();
                _observers.Remove(observer);
            }
        }

        public class SubscriptionToken<T> : IDisposable
        {
            public IObserver<T> Observer { get; }

            private readonly Action<IObserver<T>> _clearAction;

            public SubscriptionToken(IObserver<T> observer, Action<IObserver<T>> clearAction)
            {
                this.Observer = observer;
                this._clearAction = clearAction;
            }

            public void Dispose() => _clearAction.Invoke(Observer);
        }
    }
}
