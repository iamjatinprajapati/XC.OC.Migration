using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XC.OC.Migration.Core.Application.Abstractions.Infrastructure;

namespace XC.OC.Migration.Core.Application.Models
{
    public class DeleteOrderCloudMigratedUsersMessage : IQueueMessage
    {
        public int Iterations { get; set; }
    }
}
