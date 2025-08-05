using Organization.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Interfaces.Persistence
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Employee?> GetByIdWithGroupsAsync(Guid employeeId, CancellationToken cancellationToken = default);
        Task<Employee?> GetByUserIdWithGroupsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<List<Employee>> GetByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken = default);
        Task<List<Employee>> GetEmployeesByGroupIdAsync(Guid employeeGroupId, CancellationToken cancellationToken = default);
        Task<List<Employee>> GetEmployeesByHourlyRateIdAsync(Guid hourlyRateId, CancellationToken cancellationToken = default);
        Task AddAsync(Employee employee, CancellationToken cancellationToken = default);
        Task<Employee?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
