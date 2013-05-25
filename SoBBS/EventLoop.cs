using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Sobbs
{
    public class EventLoop
    {
        public EventLoop(CancellationToken cancellationToken)
        {
            LoopThread = new Thread(Loop);
            LoopThread.Start();
            _cancellationToken = cancellationToken;
        }

        private void Loop()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                Action current;
                if (_queue.TryDequeue(out current))
                    current();
                Thread.Sleep(1);
            }
        }

        private Thread LoopThread
        {
            get;
            set;
        }

        private void Enqueue(Action action)
        {
            _queue.Enqueue(action);
        }

        public void EnqueueLoop(Action iteration, int period = 0)
        {
            Enqueue(() =>
                {
                    iteration();
                    EnqueueLoop(iteration, period);
                });
        }

        private readonly ConcurrentQueue<Action> _queue = new ConcurrentQueue<Action>();
        private CancellationToken _cancellationToken;

        public void Join()
        {
            LoopThread.Join();
        }

        public void EnqueueCancelableLoop(Func<bool> iteration, int period = 0)
        {
            Enqueue(() =>
            {
                if (iteration())
                    EnqueueCancelableLoop(iteration, period);
            });
        }
    }
}

