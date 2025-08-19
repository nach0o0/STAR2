using Organization.Contracts.Requests;
using Organization.Contracts.Responses;

namespace Shared.Application.Interfaces.Infrastructure
{
    public interface IOrganizationServiceClient
    {
        public record EmployeeDto(Guid Id, string FirstName, string LastName);

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

        Task<List<EmployeeDto>> GetEmployeesByIdsAsync(List<Guid> employeeIds, CancellationToken cancellationToken = default);
    }
}
