using MediatR;
using Organization.Application.Interfaces.Persistence;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.ReassignOrganizationParent
{
    public class ReassignOrganizationToParentCommandHandler : IRequestHandler<ReassignOrganizationToParentCommand>
    {
        private readonly IOrganizationRepository _organizationRepository;

        public ReassignOrganizationToParentCommandHandler(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task Handle(ReassignOrganizationToParentCommand command, CancellationToken cancellationToken)
        {
            var organization = await _organizationRepository.GetByIdAsync(command.OrganizationId, cancellationToken);
            if (organization is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Organization), command.OrganizationId);
            }

            organization.ReassignToParent(command.NewParentId);
        }
    }
}
