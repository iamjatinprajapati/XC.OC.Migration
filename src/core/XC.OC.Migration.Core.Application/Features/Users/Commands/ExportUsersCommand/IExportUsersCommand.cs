using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XC.OC.Migration.Core.Application.Features.Users.Commands.ExportUsersCommand
{
    public interface IExportUsersCommand
    {
        Task<bool> Execute(ExportUsersRequest request);
    }
}
