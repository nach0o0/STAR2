using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Contracts.Responses
{
    public record OrganizationDetailsResponse(
        Guid Id,
        string Name,
        string Abbreviation,
        Guid? ParentOrganizationId,
        Guid? DefaultEmployeeGroupId
    );
}
