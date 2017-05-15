using System;
using System.Reactive.Subjects;

namespace LeapConsole.Observers
{
    public class PinchObserver : IObserver<float>
    {
        private readonly ISubject<Mode> _modeSwitcher;

        public PinchObserver(ISubject<Mode> modeSwitcher)
        {
            _modeSwitcher = modeSwitcher;
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(float value)
        {
            if (value > 0.95)
            {
#if DEBUG
                Console.WriteLine("Pinched!");
#endif

                // hit enter
                WindowsInput.Enter();
                WindowsInput.CloseSwitchApplication();
                _modeSwitcher.OnNext(Mode.Command);
            }
        }
    }
}
