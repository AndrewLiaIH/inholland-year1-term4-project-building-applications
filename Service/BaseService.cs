using Timer = System.Timers.Timer;
using DAL;

namespace Service
{
    public abstract class BaseService
    {
        protected const int TimerInterval = 10000;
        private Timer timer = new(TimerInterval);

        public BaseService() 
        {
            timer.Elapsed += CheckForChanges;
            timer.Elapsed += UpdateNetwork;
            timer.Start();
        }

        protected virtual void CheckForChanges(object sender, EventArgs e) { }
        protected virtual void UpdateNetwork(object sender, EventArgs e) { }

        public bool ConnectionAvalible<T>() where T : BaseDao, new()
        {
            return new T().ConnectionAvalible();
        }
    }
}