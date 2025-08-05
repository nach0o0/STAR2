using MediatR;
using Organization.Application.Interfaces.Persistence;
using Shared.Application.Interfaces.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.UpdateMyEmployeeProfile
{
    public class UpdateMyEmployeeProfileCommandHandler : IRequestHandler<UpdateMyEmployeeProfileCommand>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserContext _userContext;

        public UpdateMyEmployeeProfileCommandHandler(IEmployeeRepository employeeRepository, IUserContext userContext)
        {
            _employeeRepository = employeeRepository;
            _userContext = userContext;
        }

        public async Task Handle(UpdateMyEmployeeProfileCommand command, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser()!;

            if (!currentUser.EmployeeId.HasValue)
            {
                throw new ForbiddenAccessException("User does not have an employee profile to update.");
            }

            var employee = await _employeeRepository.GetByIdAsync(currentUser.EmployeeId.Value, cancellationToken);
            if (employee is null)
            {
                throw new NotFoundException("Employee", currentUser.EmployeeId);
            }

            // Ruft eine Update-Methode auf der Entität auf.
            employee.UpdateDetails(command.FirstName, command.LastName);
        }
    }
}
