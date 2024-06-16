using Timer = System.Timers.Timer;

namespace Service
{
    public abstract class BaseService
    {
        protected const int TimerInterval = 10000;
        private Timer timer = new(TimerInterval);
        public static event Action NetworkExceptionOccurred;

        public BaseService() 
        {
            timer.Elapsed += CheckForChanges;
            timer.Start();
        }

        protected virtual void CheckForChanges(object sender, EventArgs e) { }

        protected void NetworkExceptionHandler()
        {
            NetworkExceptionOccurred?.Invoke();
        }
    }
}