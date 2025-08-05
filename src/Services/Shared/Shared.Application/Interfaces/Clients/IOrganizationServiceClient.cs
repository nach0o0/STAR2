namespace Shared.Application.Interfaces.Infrastructure
{
    public interface IOrganizationServiceClient
    {
        Task<(Guid EmployeeId, Guid? OrganizationId, List<Guid> EmployeeGroupIds)?> GetEmployeeInfoByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
    }
}
