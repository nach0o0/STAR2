using Shared.Application.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Application.Interfaces.Security
{
    public interface ICommandAuthorizer<in TCommand>
    {
        Task AuthorizeAsync(TCommand command, CurrentUser currentUser, CancellationToken cancellationToken);
    }
}
