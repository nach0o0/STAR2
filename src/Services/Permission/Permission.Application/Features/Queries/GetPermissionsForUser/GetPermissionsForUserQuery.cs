using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Queries.GetPermissionsForUser
{
    public record GetPermissionsForUserQuery(Guid UserId, IEnumerable<string> Scopes)
        : IRequest<Dictionary<string, List<string>>>;
}
