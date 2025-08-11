using Auth.Application.Interfaces.Persistence;
using MediatR;
using Shared.Application.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Features.Queries.GetUserByEmail
{
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, GetUserByEmailQueryResult?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrganizationServiceClient _organizationServiceClient;

        public GetUserByEmailQueryHandler(
            IUserRepository userRepository,
            IOrganizationServiceClient organizationServiceClient)
        {
            _userRepository = userRepository;
            _organizationServiceClient = organizationServiceClient;
        }

        public async Task<GetUserByEmailQueryResult?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            // 1. Hole den Benutzer aus der lokalen Auth-Datenbank.
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user is null)
            {
                return null;
            }

            // 2. Rufe den OrganizationService auf, um die Daten anzureichern.
            // Wir gehen davon aus, dass GetEmployeesByUserIdsAsync auch mit einer einzelnen ID umgehen kann.
            var employeeDetailsRequest = new Organization.Contracts.Requests.GetEmployeesByUserIdsRequest(new() { user.Id });
            var employeeDetailsList = await _organizationServiceClient.GetEmployeesByUserIdsAsync(employeeDetailsRequest, cancellationToken);
            var employeeDetails = employeeDetailsList.FirstOrDefault();

            // 3. Baue das vollständige Ergebnis-Objekt zusammen.
            return new GetUserByEmailQueryResult(
                user.Id,
                user.Email,
                employeeDetails?.FirstName,
                employeeDetails?.LastName
            );
        }
    }
}
