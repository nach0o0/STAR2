using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Features.Queries.GetUsersByIds
{
    public record GetUsersByIdsQueryResult(
        Guid UserId,
        string Email,
        string? FirstName,
        string? LastName
    );
}
