using Timer = System.Timers.Timer;

namespace Service
{
    public abstract class BaseService
    {
        private const int TimerInterval = 10000;
        private Timer timer = new(TimerInterval);

        public BaseService()
        {
            timer.Elapsed += CheckForChanges;
            timer.Start();
        }

        protected abstract void CheckForChanges(object sender, EventArgs e);
    }
}