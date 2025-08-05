using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Features.Queries.GetUserById
{
    public record GetUserByIdQueryResult(Guid UserId, string Email);
}
