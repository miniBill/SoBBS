using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Threading;

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

        private readonly ConcurrentQueue<Action> queue = new ConcurrentQueue<Action>();

        public async void Update()
        {
            for (;;)
            {
                Action current;
                if (queue.TryDequeue(out current))
                    current();
                await Task.Delay(0);
            }
        }
    }
}

