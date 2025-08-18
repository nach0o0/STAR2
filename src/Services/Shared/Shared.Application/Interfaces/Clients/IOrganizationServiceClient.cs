using Organization.Contracts.Requests;
using Organization.Contracts.Responses;

namespace Shared.Application.Interfaces.Infrastructure
{
    public interface IOrganizationServiceClient
    {
        Task<(Guid EmployeeId, Guid? OrganizationId, List<Guid> EmployeeGroupIds)?> GetEmployeeInfoByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<(Guid EmployeeId, Guid? OrganizationId, List<Guid> EmployeeGroupIds)?> GetEmployeeInfoByEmployeeIdAsync(
            Guid employeeId,
            CancellationToken cancellationToken = default);

        Task<List<EmployeeDetailsResponse>> GetEmployeesByUserIdsAsync(
            GetEmployeesByUserIdsRequest request,
            CancellationToken cancellationToken = default);

        Task<List<EmployeeResponse>> GetEmployeesByEmployeeGroupAsync(Guid employeeGroupId, CancellationToken cancellationToken = default);
    }
}
