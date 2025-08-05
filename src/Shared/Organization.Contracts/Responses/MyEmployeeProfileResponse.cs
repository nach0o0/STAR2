using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Contracts.Responses
{
    public record MyEmployeeProfileResponse(
        Guid EmployeeId,
        string FirstName,
        string LastName
    );
}
