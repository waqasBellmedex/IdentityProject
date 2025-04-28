using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace BackgroundTasks.Interface
{
    public interface IJobQueue<T>
    {
        void Enqueue(T job);
        ChannelReader<T> Reader { get; }
    }
}
