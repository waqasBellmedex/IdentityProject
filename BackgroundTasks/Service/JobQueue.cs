using BackgroundTasks.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace BackgroundTasks.Service
{
    public class JobQueue<T> : IJobQueue<T> where T : class
    {
        private readonly Channel<T> _channel;
        public JobQueue() {
            _channel = Channel.CreateUnbounded<T>();
        }
        public void Enqueue(T item)
        {
            _channel.Writer.TryWrite(item);
        }
        public ChannelReader<T> Reader => _channel.Reader;

    }
}
