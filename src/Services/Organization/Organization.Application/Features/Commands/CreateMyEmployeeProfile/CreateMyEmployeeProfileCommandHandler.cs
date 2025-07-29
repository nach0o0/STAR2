using MediatR;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Entities;
using Shared.Application.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.CreateMyEmployeeProfile
{
    public class CreateMyEmployeeProfileCommandHandler : IRequestHandler<CreateMyEmployeeProfileCommand, Guid>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserContext _userContext;

        public CreateMyEmployeeProfileCommandHandler(IEmployeeRepository employeeRepository, IUserContext userContext)
        {
            _employeeRepository = employeeRepository;
            _userContext = userContext;
        }

        public async Task<Guid> Handle(CreateMyEmployeeProfileCommand command, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser()!; // Ist durch die Pipeline sichergestellt

            // Erstelle eine neue Employee-Entität.
            var employee = new Employee(command.FirstName, command.LastName);

            // Verknüpfe sie direkt mit der UserId des angemeldeten Benutzers.
            employee.AssignUser(currentUser.UserId);

            await _employeeRepository.AddAsync(employee, cancellationToken);

            return employee.Id;
        }
    }
}
