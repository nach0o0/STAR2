using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeeInfoByUserId
{
    public record GetEmployeeInfoByUserIdQueryResult(
        Guid EmployeeId,
        Guid? OrganizationId,
        List<Guid> EmployeeGroupIds
    );
}
