using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Threading;
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
            queue.Enqueue(action);
        }

        public void EnqueueLoop(Action iteration, int period = 1)
        {
            Enqueue(async delegate
            {
                for (;;)
                {
                    iteration();
                    await Task.Delay(period);
                }
            }
            );
        }

        private readonly ConcurrentQueue<Action> queue = new ConcurrentQueue<Action>();

        public async void Update()
        {
            for (;;)
            {
                Action current;
                if (queue.TryDequeue(out current))
                    current();
                Thread.Sleep(1); // Don't busy wait
                await Task.Delay(0); // Yield to continuations
            }
        }
    }
}

