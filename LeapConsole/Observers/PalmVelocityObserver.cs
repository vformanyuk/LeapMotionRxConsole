using System;
using System.Reactive.Subjects;

namespace LeapConsole.Observers
{
    public class PalmVelocityObserver : IObserver<VelocityInfo>
    {
        private const int ConfidenceThreshold = 1550;

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

        public void OnNext(VelocityInfo value)
        {
            if (value.HorizontalVelocity == 0 && value.VerticalVelocity == 0) return;
            if (_isCompleted) return;

            var absXVelocity = Math.Abs(value.HorizontalVelocity);
            var absYVelocity = Math.Abs(value.VerticalVelocity);
            var absZVelocity = Math.Abs(value.ZVelocity);

            // user putting hand back to keyboard or mouse
            if (absZVelocity > absXVelocity && absZVelocity > absYVelocity)
            {
#if DEBUG
                Console.WriteLine($"Z velocity {value.ZVelocity}");
#endif
                return;
            }

            // horizontal motion
            if (absXVelocity > absYVelocity)
            {
#if DEBUG
                Console.WriteLine($"Horizontal velocity {value.HorizontalVelocity}");
#endif
                // uncertain motion
                if (absXVelocity < ConfidenceThreshold) return;

                WindowsInput.ShowSwitchApplications();
                _modeSwitcher.OnNext(Mode.Selection);
            }
            // vertical motion
            else
            {
#if DEBUG
                Console.WriteLine($"Vertical velocity {value.VerticalVelocity}");
#endif
                // uncertain motion
                if (absYVelocity < ConfidenceThreshold) return;

                if (value.VerticalVelocity < 0) // towards controller
                {
                    // Win + D
                    WindowsInput.MinimizeAll();
                    // reset all observers
                    _modeSwitcher.OnNext(Mode.Command);
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
