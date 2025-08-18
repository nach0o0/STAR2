using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Authorization;
using Organization.Domain.Entities;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Authorization;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeeById
{
    public class GetEmployeeByIdQueryAuthorizer : ICommandAuthorizer<GetEmployeeByIdQuery>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public GetEmployeeByIdQueryAuthorizer(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task AuthorizeAsync(GetEmployeeByIdQuery command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(command.EmployeeId, cancellationToken);
            if (employee is null || !employee.OrganizationId.HasValue)
            {
                // Wenn der Mitarbeiter nicht existiert oder keiner Organisation angehört, kann niemand darauf zugreifen.
                throw new NotFoundException(nameof(Employee), command.EmployeeId);
            }

            // Fall 1: Der anfragende Benutzer ist der Mitarbeiter selbst.
            if (currentUser.EmployeeId == command.EmployeeId)
            {
                return;
            }

            // Fall 2: Der anfragende Benutzer hat die Leseberechtigung in der Organisation des Mitarbeiters.
            var requiredPermission = EmployeePermissions.Read;
            var requiredScope = $"{PermittedScopeTypes.Organization}:{employee.OrganizationId.Value}";

            if (currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) &&
                permissions.Contains(requiredPermission))
            {
                return;
            }

            throw new ForbiddenAccessException("You are not authorized to view this employee's details.");
        }
    }
}
