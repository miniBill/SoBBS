using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks.Schedulers;

namespace Sobbs.Loop
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

        private void Enqueue(Action action)
        {
            _queue.Enqueue(action);
        }

        public void EnqueueLoop(Action iteration, int period = 0)
        {
            EnqueueCancelableLoop(() =>
            {
                iteration();
                return true;
            }, period);
        }

        private readonly ConcurrentQueue<Action> _queue = new ConcurrentQueue<Action>();
        private CancellationToken _cancellationToken;

        private async Task Update(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Action current;
                if (_queue.TryDequeue(out current))
                    current();
                Thread.Sleep(1); // Don't busy wait
                await Task.Yield();
            }
        }

        public void Join()
        {
            while (!_cancellationToken.IsCancellationRequested)
                Thread.Sleep(10);
        }

        public void EnqueueCancelableLoop(Func<bool> iteration, int period = 0)
        {
            Enqueue(async delegate
            {
                while (true)
                {
                    if (!iteration())
                        break;
                    if (period > 0)
                        await Task.Delay(period);
                    else
                        await Task.Yield();
                }
            });
        }
    }
}

