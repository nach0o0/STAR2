using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.CreateEmployeeGroup
{
    public record CreateEmployeeGroupCommand(
        string Name,
        Guid LeadingOrganizationId
    ) : IRequest<Guid>;
}
