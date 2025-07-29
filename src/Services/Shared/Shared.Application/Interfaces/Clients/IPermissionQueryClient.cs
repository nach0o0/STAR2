﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Application.Interfaces.Clients
{
    public interface IPermissionQueryClient
    {
        Task<Dictionary<string, List<string>>?> GetPermissionsForUserAsync(
            Guid userId,
            IEnumerable<string> scopes,
            CancellationToken cancellationToken = default);
    }
}
