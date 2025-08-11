using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeesByUserIds
{
    public record GetEmployeesByUserIdsQueryResult(Guid UserId, Guid EmployeeId, string FirstName, string LastName);
}
