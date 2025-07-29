using MediatR;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.CreateOrganization
{
    public class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand, Guid>
    {
        private readonly IOrganizationRepository _organizationRepository;

        public CreateOrganizationCommandHandler(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<Guid> Handle(CreateOrganizationCommand command, CancellationToken cancellationToken)
        {
            var organization = new Domain.Entities.Organization(command.Name, command.Abbreviation, command.ParentOrganizationId);

            await _organizationRepository.AddAsync(organization, cancellationToken);

            return organization.Id;
        }
    }
}
