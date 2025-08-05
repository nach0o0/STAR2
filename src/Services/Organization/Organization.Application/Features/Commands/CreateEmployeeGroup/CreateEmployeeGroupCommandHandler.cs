using MediatR;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Entities;
using Shared.Application.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.CreateEmployeeGroup
{
    public class CreateEmployeeGroupCommandHandler : IRequestHandler<CreateEmployeeGroupCommand, Guid>
    {
        private readonly IEmployeeGroupRepository _employeeGroupRepository;
        private readonly IUserContext _userContext;

        public CreateEmployeeGroupCommandHandler(IEmployeeGroupRepository employeeGroupRepository, IUserContext userContext)
        {
            _employeeGroupRepository = employeeGroupRepository;
            _userContext = userContext;
        }

        public async Task<Guid> Handle(CreateEmployeeGroupCommand command, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser()!;

            var employeeGroup = new EmployeeGroup(command.Name, command.LeadingOrganizationId, currentUser.UserId);
            await _employeeGroupRepository.AddAsync(employeeGroup, cancellationToken);
            return employeeGroup.Id;
        }
    }
}
