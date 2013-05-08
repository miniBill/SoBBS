using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;

namespace Sobbs
{
    public class EventLoop
    {
        public void Start()
        {
            new Task(() => AsyncPump.Run((Action)Update), TaskCreationOptions.LongRunning).Start();
        }

        public void Enqueue(Action action)
        {
            _queue.Enqueue(action);
        }

        public void EnqueueLoop(Action iteration, int period = 1)
        {
            Enqueue(async delegate
            {
                for (; ; )
                {
                    iteration();
                    await Task.Delay(period);
                }
                // ReSharper disable FunctionNeverReturns
            });
            // ReSharper restore FunctionNeverReturns
        }

        private readonly ConcurrentQueue<Action> _queue = new ConcurrentQueue<Action>();

        private async void Update()
        {
            for (; ; )
            {
                Action current;
                if (_queue.TryDequeue(out current))
                    current();
                Thread.Sleep(1); // Don't busy wait
                await Task.Delay(0); // Yield to continuations
            }
            // ReSharper disable FunctionNeverReturns
        }
        // ReSharper restore FunctionNeverReturns
    }
}

