using MediatR;
using Organization.Application.Interfaces.Persistence;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.DeleteOrganization
{
    public class DeleteOrganizationCommandHandler : IRequestHandler<DeleteOrganizationCommand>
    {
        private readonly IOrganizationRepository _organizationRepository;

        public DeleteOrganizationCommandHandler(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task Handle(DeleteOrganizationCommand command, CancellationToken cancellationToken)
        {
            var organization = await _organizationRepository.GetByIdAsync(command.OrganizationId, cancellationToken);
            if (organization is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Organization), command.OrganizationId);
            }

            // 1. Ruft die Methode auf der Entität auf, um das Event auszulösen.
            organization.PrepareForDeletion(command.DeleteSubOrganizations);

            // 2. Weist das Repository an, die Entität aus der Datenbank zu entfernen.
            _organizationRepository.Delete(organization);

            // Die UnitOfWork-Pipeline speichert die Änderungen und veröffentlicht das Event.
        }
    }
}
