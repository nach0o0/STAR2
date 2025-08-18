using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeeGroupsForEmployee
{
    public record GetEmployeeGroupsForEmployeeQueryResult(
        Guid EmployeeGroupId,
        string Name
    );
}
