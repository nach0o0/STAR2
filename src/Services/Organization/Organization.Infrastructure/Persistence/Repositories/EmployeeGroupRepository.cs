using Microsoft.EntityFrameworkCore;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Infrastructure.Persistence.Repositories
{
    public class EmployeeGroupRepository : IEmployeeGroupRepository
    {
        private readonly OrganizationDbContext _dbContext;

        public EmployeeGroupRepository(OrganizationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<EmployeeGroup?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.EmployeeGroups.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<List<EmployeeGroup>> GetByLeadingOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.EmployeeGroups.Where(eg => eg.LeadingOrganizationId == organizationId).ToListAsync(cancellationToken);
        }

        public async Task AddAsync(EmployeeGroup employeeGroup, CancellationToken cancellationToken = default)
        {
            await _dbContext.EmployeeGroups.AddAsync(employeeGroup, cancellationToken);
        }

        public void Delete(EmployeeGroup employeeGroup)
        {
            _dbContext.EmployeeGroups.Remove(employeeGroup);
        }

        public async Task<bool> NameExistsInOrganizationAsync(string name, Guid organizationId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.EmployeeGroups
                .AnyAsync(eg =>
                    eg.Name == name &&
                    eg.LeadingOrganizationId == organizationId,
                    cancellationToken);
        }
    }
}
