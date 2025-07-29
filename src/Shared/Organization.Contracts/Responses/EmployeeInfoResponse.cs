using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Contracts.Responses
{
    public record EmployeeInfoResponse(
        Guid EmployeeId,
        Guid OrganizationId,
        List<Guid> EmployeeGroupIds
    );
}
