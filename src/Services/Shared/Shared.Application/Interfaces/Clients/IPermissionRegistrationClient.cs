using Permission.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Application.Interfaces.Clients
{
    public interface IPermissionRegistrationClient
    {
        Task RegisterPermissionsAsync(
            IEnumerable<(string Id, string Description)> permissions,
            CancellationToken cancellationToken = default);
    }
}
