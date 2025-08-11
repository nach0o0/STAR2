using Auth.Application.Interfaces.Persistence;
using MediatR;
using Organization.Contracts.Requests;
using Shared.Application.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Features.Queries.GetUsersByIds
{
    public class GetUsersByIdsQueryHandler : IRequestHandler<GetUsersByIdsQuery, List<GetUsersByIdsQueryResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrganizationServiceClient _organizationServiceClient;

        public GetUsersByIdsQueryHandler(IUserRepository userRepository, IOrganizationServiceClient organizationServiceClient)
        {
            _userRepository = userRepository;
            _organizationServiceClient = organizationServiceClient;
        }

        public async Task<List<GetUsersByIdsQueryResult>> Handle(GetUsersByIdsQuery request, CancellationToken cancellationToken)
        {
            // 1. Hole Basis-Benutzerdaten aus der Auth-DB
            var users = await _userRepository.GetByIdsAsync(request.UserIds, cancellationToken);
            if (!users.Any()) return new List<GetUsersByIdsQueryResult>();

            // 2. Rufe den OrganizationService auf, um die Daten anzureichern
            var employeeDetailsRequest = new GetEmployeesByUserIdsRequest(users.Select(u => u.Id).ToList());
            var employeeDetails = await _organizationServiceClient.GetEmployeesByUserIdsAsync(employeeDetailsRequest, cancellationToken);
            var employeeDetailsDict = employeeDetails.ToDictionary(e => e.UserId);

            // 3. Kombiniere die Daten
            return users.Select(u => {
                employeeDetailsDict.TryGetValue(u.Id, out var details);
                return new GetUsersByIdsQueryResult(
                    u.Id,
                    u.Email,
                    details?.FirstName,
                    details?.LastName
                );
            }).ToList();
        }
    }
}
