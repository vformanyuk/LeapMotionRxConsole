using System;

namespace LeapConsole.Observers
{
    public class PalmMoveObserver : IObserver<TotalVelocity>
    {
        private const int ConfidenceThreshold = 115;

        private bool _isCompleted = false;

        public void OnCompleted()
        {
            _isCompleted = true;
#if DEBUG
            Console.WriteLine($"PalmMoveObserver completed");
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

            if (Math.Abs(value.HorizontalVelocity) >= ConfidenceThreshold)
            {
                if (value.HorizontalVelocity < 0)
                {
#if DEBUG
                    Console.WriteLine("Move left");
#endif
                    WindowsInput.Left();
                }
                else if (value.HorizontalVelocity > 0)
                {
#if DEBUG
                    Console.WriteLine("Move right");
#endif
                    WindowsInput.Right();
                }
            }

            if (Math.Abs(value.VerticalVelocity) >= ConfidenceThreshold)
            {
                if (value.VerticalVelocity < 0)
                {
#if DEBUG
                    Console.WriteLine("Move down");
#endif
                    WindowsInput.Down();
                }
                else if (value.VerticalVelocity > 0)
                {
#if DEBUG
                    Console.WriteLine("Move up");
#endif
                    WindowsInput.Up();
                }
            }
        }
    }
}
