using Leap;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace LeapConsole
{
    public static class Extensions
    {
        public static IObservable<Frame> EnsureOneHande(this IObservable<Frame> observable)
        {
            return observable.Where(f =>
            {
                return f.Hands.Count == 1 && f.Hands[0].Confidence > 0.85;
            });
        }
    }
}
