using System;
using System.Reactive.Subjects;
using System.Threading;

namespace LeapConsole.Observers
{
    public class PalmVelocityObserver : IObserver<TotalVelocity>
    {
        private const int ConfidenceThreshold = 1750;

        private readonly ISubject<Mode> _modeSwitcher;

        private bool _isCompleted;

        public PalmVelocityObserver(ISubject<Mode> modeSwitcher)
        {
            _modeSwitcher = modeSwitcher;
            _isCompleted = false;
        }

        public void OnCompleted()
        {
            _isCompleted = true;
#if DEBUG
            Console.WriteLine($"PalmVelocityObserver completed");
#endif
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(TotalVelocity value)
        {
            if (value.HorizontalVelocity == 0 && value.VerticalVelocity == 0) return;
            if (_isCompleted) return;

            // horizontal motion
            if (Math.Abs(value.HorizontalVelocity) > Math.Abs(value.VerticalVelocity))
            {
#if DEBUG
                Console.WriteLine($"Horizontal velocity {value.HorizontalVelocity}");
#endif

                // uncertain motion
                if (Math.Abs(value.HorizontalVelocity) < ConfidenceThreshold) return;

                WindowsInput.ShowSwitchApplications();

                //if (value.HorizontalVelocity < 0) //left
                //else //right
                _modeSwitcher.OnNext(Mode.Selection);
            }
            // vertical motion
            else
            {
#if DEBUG
                Console.WriteLine($"Vertical velocity {value.VerticalVelocity}");
#endif

                // uncertain motion
                if (Math.Abs(value.VerticalVelocity) < ConfidenceThreshold) return;

                if (value.VerticalVelocity < 0) // towards controller
                {
                    // Win + D
                    WindowsInput.MinimizeAll();
                }
                else
                {
                    // if all windows are minimized - Win+D

                    // else Win+Tab
                    WindowsInput.SwitchDesktops();
                    _modeSwitcher.OnNext(Mode.Selection);
                }
            }
        }
    }
}
