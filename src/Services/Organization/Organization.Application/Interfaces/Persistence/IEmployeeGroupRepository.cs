using Organization.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Interfaces.Persistence
{
    public interface IEmployeeGroupRepository
    {
        Task<EmployeeGroup?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<EmployeeGroup>> GetByLeadingOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken = default);
        Task AddAsync(EmployeeGroup employeeGroup, CancellationToken cancellationToken = default);
        void Delete(EmployeeGroup employeeGroup);
        Task<bool> NameExistsInOrganizationAsync(string name, Guid organizationId, CancellationToken cancellationToken = default);
    }
}
