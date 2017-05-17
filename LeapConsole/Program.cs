using Leap;
using LeapConsole.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace LeapConsole
{
    class Program
    {
        private const double CommandSensetivity = 350; // mm/sec2
        private const double SelectionHorizontalSensitivity = 75;
        private const double SelectionVerticalSensitivity = 30;

        private const double CommandAngleSensetivity = 1; // rad
        private const double SelectionAngleSensetivity = 1.35;

        private const double PalmRotationSensetivity = 0.25;
        private const double PalmRotationThreshold = 0.65;

        private const double PalGrabSensetivity = 0.35;
        private const double PalGrabThreshold = 0.98;

        private static readonly TimeSpan BufferingWindow = TimeSpan.FromMilliseconds(750);
        private static readonly TimeSpan SamplingWindow = TimeSpan.FromMilliseconds(200);

        /// <summary>
        /// "Hot" observable that will emit frames as soon as frames stream starts
        /// </summary>
        private static FramesObservable _leapObservable = new FramesObservable();

        /// <summary>
        /// Subject that controls switch between selection and command mode
        /// It is observable and observer at the same time
        /// </summary>
        private static Subject<Mode> _modeSwitchSubject = new Subject<Mode>();

        private static bool _switchingMode = false;

        private static bool _tabReactionStoped = false;
        private static bool _delReactionStoped = false;

        private static readonly object SyncKey = new object();

        static void Main(string[] args)
        {
            List<IDisposable> subscriptionTokens = new List<IDisposable>();
            CreateCommandModeObservers(subscriptionTokens);

            _modeSwitchSubject.Subscribe(m =>
            {
                lock (SyncKey)
                {
                    _switchingMode = true;
#if DEBUG
                    Console.WriteLine($"New mode is {m}");
#endif
                    foreach (var disposable in subscriptionTokens)
                    {
                        disposable.Dispose();
                    }
                    subscriptionTokens.Clear();

                    if (m == Mode.Command)
                    {
                        CreateCommandModeObservers(subscriptionTokens);
                    }
                    else
                    {
                        CreateSelectionModeObservers(subscriptionTokens);
                    }
                    _switchingMode = false;
                }
            });

            using (var controller = new Controller())
            {
                //Observable.FromEvent<FrameEventArgs>()
                controller.FrameReady += OnFrameReady;
                Console.WriteLine("Press 'e' to exit...");
                while (Console.ReadKey().KeyChar != 'e') { };
            }

            subscriptionTokens.ForEach(t => t.Dispose());
            subscriptionTokens.Clear();
        }

        private static void OnFrameReady(object sender, FrameEventArgs e)
        {
            if (_switchingMode) return;

            _leapObservable.Emit(e.frame);
        }

        private static void CreateCommandModeObservers(List<IDisposable> observers)
        {
            observers.Add(_leapObservable
            .EnsureOneHande()
            .Where(f =>
            {
                var hand = f.Hands[0];
                var avgAngl = hand.Fingers.Average(fi => fi.Direction.AngleTo(hand.Direction));
                return avgAngl <= CommandAngleSensetivity &&
                       (Math.Abs(hand.PalmVelocity.y) >= CommandSensetivity || Math.Abs(hand.PalmVelocity.x) >= CommandSensetivity);
            })
            .Buffer(BufferingWindow)
            .Select(b => new TotalVelocity(b.Select(f => f.Hands[0].PalmVelocity.x).Sum(),
                                            b.Select(f => f.Hands[0].PalmVelocity.y).Sum()))
            .Subscribe(new PalmVelocityObserver(_modeSwitchSubject)));
        }

        private static void CreateSelectionModeObservers(List<IDisposable> observers)
        {
            observers.Add(_leapObservable
            .EnsureOneHande()
            .Select(f => f.Hands[0].PinchStrength)
            .Subscribe(new PinchObserver(_modeSwitchSubject)));

            observers.Add(_leapObservable
            .EnsureOneHande()
            .Where(f =>
            {
                var hand = f.Hands[0];
                var avgAngl = hand.Fingers.Average(fi => fi.Direction.AngleTo(hand.Direction));
                var palmRotation = Math.Abs(hand.Rotation.z);
                return avgAngl <= SelectionAngleSensetivity && palmRotation < PalmRotationSensetivity &&
                       (Math.Abs(hand.PalmVelocity.y) >= SelectionVerticalSensitivity ||
                        Math.Abs(hand.PalmVelocity.x) >= SelectionHorizontalSensitivity);
            })
            .Sample(SamplingWindow)
            .Select(f => new TotalVelocity(f.Hands[0].PalmVelocity.x, f.Hands[0].PalmVelocity.y))
            .Subscribe(new PalmMoveObserver()));

            observers.Add(_leapObservable
            .EnsureOneHande()
            .Where(f =>
            {
                var rotation = Math.Abs(f.Hands[0].Rotation.z);
                if (_tabReactionStoped && rotation < PalmRotationSensetivity)
                {
                    // lets react on palm rotations (tab hits) again.
                    _tabReactionStoped = false;
                }
                return rotation > PalmRotationThreshold && !_tabReactionStoped;
            })
            .Subscribe(r =>
            {
                // stop reacting on next to avoid multiple tab "hits"
                _tabReactionStoped = true;
                WindowsInput.Tab();
            }));

            observers.Add(_leapObservable
            .EnsureOneHande()
            .Where(f =>
            {
                var grab = f.Hands[0].GrabStrength;
                if (_delReactionStoped && grab < PalGrabSensetivity)
                {
                    _delReactionStoped = false;
                }
                return grab > PalGrabThreshold && !_delReactionStoped;
            })
            .Subscribe(g =>
            {
                _delReactionStoped = true;
                WindowsInput.Del();
            }));
        }
    }
}
