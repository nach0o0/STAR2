using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Contracts.Requests
{
    public record CreateLabelRequest(
        string Name,
        Guid EmployeeGroupId
    );
}
