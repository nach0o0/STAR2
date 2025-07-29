using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Contracts.Requests
{
    public record CreateOrganizationRequest(
        string Name,
        string Abbreviation,
        Guid? ParentOrganizationId
    );
}
