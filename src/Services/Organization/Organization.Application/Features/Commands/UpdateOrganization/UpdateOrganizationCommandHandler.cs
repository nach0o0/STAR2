using MediatR;
using Organization.Application.Interfaces.Persistence;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.UpdateOrganization
{
    public class UpdateOrganizationCommandHandler : IRequestHandler<UpdateOrganizationCommand>
    {
        private readonly IOrganizationRepository _organizationRepository;

        public UpdateOrganizationCommandHandler(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task Handle(UpdateOrganizationCommand command, CancellationToken cancellationToken)
        {
            var organization = await _organizationRepository.GetByIdAsync(command.OrganizationId, cancellationToken);

            if (organization is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Organization), command.OrganizationId);
            }

            // Ruft die spezifischen Methoden auf der Entität auf.
            if (command.Name is not null)
            {
                organization.UpdateName(command.Name);
            }

            if (command.Abbreviation is not null)
            {
                organization.UpdateAbbreviation(command.Abbreviation);
            }
        }
    }
}
