using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XC.OC.Migration.Core.Application.Abstractions.Infrastructure
{
    public interface IQueueMessage<T> where T : class
    {
        string QueueName { get; }

        T Message { get; set; }
    }
}
