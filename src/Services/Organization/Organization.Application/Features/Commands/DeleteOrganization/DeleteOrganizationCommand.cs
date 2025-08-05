using MediatR;
using Organization.Domain.Authorization;
using Shared.Application.Interfaces.Messaging;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.DeleteOrganization
{
    public record DeleteOrganizationCommand(
        Guid OrganizationId,
        bool DeleteSubOrganizations = false
    ) : ICommand;
}
