using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks.Schedulers;
using Sobbs.Log;

namespace Sobbs
{
    public class EventLoop
    {
        public EventLoop(CancellationToken cancellationToken)
        {
            var lcts = new LimitedConcurrencyLevelTaskScheduler(1);
            var factory = new TaskFactory(lcts);
            factory.StartNew(() => Update(cancellationToken), TaskCreationOptions.LongRunning);
            _cancellationToken = cancellationToken;
        }

        public void Enqueue(Action action)
        {
            _queue.Enqueue(action);
        }

        public void EnqueueLoop(Action iteration, int period = 1)
        {
            Enqueue(async delegate
            {
                while (true)
                {
                    Logger.Log(LogLevel.Debug, "Iteration on thread" + Thread.CurrentThread.ManagedThreadId);
                    iteration();
                    await Task.Delay(period);
                }
                // ReSharper disable FunctionNeverReturns
            });
            // ReSharper restore FunctionNeverReturns
        }

        private readonly ConcurrentQueue<Action> _queue = new ConcurrentQueue<Action>();
        private CancellationToken _cancellationToken;

        private async Task Update(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Logger.Log(LogLevel.Debug, "Mainloop on thread" + Thread.CurrentThread.ManagedThreadId);
                Action current;
                if (_queue.TryDequeue(out current))
                    current();
                Thread.Sleep(1); // Don't busy wait
                await Task.Yield();
            }
            Logger.Log(LogLevel.Debug, "Mainloop done on thread" + Thread.CurrentThread.ManagedThreadId);
        }

        public void Join()
        {
            while (!_cancellationToken.IsCancellationRequested)
                Thread.Sleep(10);
        }
    }
}

