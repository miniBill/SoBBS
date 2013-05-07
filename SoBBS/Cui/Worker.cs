using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Sobbs.Cui
{
    public class Worker
    {
        private BlockingCollection<Action> _queue = new BlockingCollection<Action>();

        public Worker()
        {
            var thread = new Thread(
                () =>
                {
                while (true)
                {
                    Action action = _queue.Take();
                    action();

                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

        public void QueueWorkItem(Action action)
        {
            _queue.Add(action);
        }
    }
}

