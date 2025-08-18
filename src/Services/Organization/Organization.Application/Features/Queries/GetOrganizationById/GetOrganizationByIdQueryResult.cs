using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetOrganizationById
{
    public record GetOrganizationByIdQueryResult(
        Guid Id,
        string Name,
        string Abbreviation,
        Guid? ParentOrganizationId,
        Guid? DefaultEmployeeGroupId
    );
}
