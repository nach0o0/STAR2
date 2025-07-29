using MediatR;
using Organization.Domain.Authorization;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Exceptions;
using System.Security.Claims;
using System.Text.Json;

namespace Organization.Application.Features.Commands.CreateOrganization
{
    public record CreateOrganizationCommand(
        string Name,
        string Abbreviation,
        Guid? ParentOrganizationId
    ) : IRequest<Guid>;
}
