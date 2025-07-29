using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.RemoveEmployeeFromOrganization
{
    public record RemoveEmployeeFromOrganizationCommand(Guid EmployeeId) : IRequest;
}
