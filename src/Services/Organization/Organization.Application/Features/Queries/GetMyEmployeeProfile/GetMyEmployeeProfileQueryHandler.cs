using MediatR;
using Organization.Application.Interfaces.Persistence;
using Shared.Application.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetMyEmployeeProfile
{
    public class GetMyEmployeeProfileQueryHandler : IRequestHandler<GetMyEmployeeProfileQuery, GetMyEmployeeProfileQueryResult?>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserContext _userContext;

        public GetMyEmployeeProfileQueryHandler(IEmployeeRepository employeeRepository, IUserContext userContext)
        {
            _employeeRepository = employeeRepository;
            _userContext = userContext;
        }

        public async Task<GetMyEmployeeProfileQueryResult?> Handle(GetMyEmployeeProfileQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser()!;
            if (!currentUser.EmployeeId.HasValue)
            {
                return null;
            }

            var employee = await _employeeRepository.GetByIdAsync(currentUser.EmployeeId.Value, cancellationToken);
            if (employee is null)
            {
                return null;
            }

            return new GetMyEmployeeProfileQueryResult(employee.Id, employee.FirstName, employee.LastName);
        }
    }
}
