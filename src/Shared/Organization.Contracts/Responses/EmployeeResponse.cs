using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Contracts.Responses
{
    public record EmployeeResponse(
        Guid Id,
        string FirstName,
        string LastName,
        Guid? UserId,
        Guid? OrganizationId
    );
}
