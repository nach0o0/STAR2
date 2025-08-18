using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeeById
{
    public record GetEmployeeByIdQueryResult(
        Guid Id,
        string FirstName,
        string LastName,
        Guid? UserId,
        Guid? OrganizationId
    );
}
