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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly OrganizationDbContext _dbContext;

        public EmployeeRepository(OrganizationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Employees.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<Employee?> GetByIdWithGroupsAsync(Guid employeeId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Employees
                .Include(e => e.EmployeeGroupLinks)
                .FirstOrDefaultAsync(e => e.Id == employeeId, cancellationToken);
        }

        public async Task<Employee?> GetByUserIdWithGroupsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Employees
                .Include(e => e.EmployeeGroupLinks)
                .FirstOrDefaultAsync(e => e.UserId == userId, cancellationToken);
        }

        public async Task<List<Employee>> GetByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Employees.Where(e => e.OrganizationId == organizationId).ToListAsync(cancellationToken);
        }

        public async Task<List<Employee>> GetEmployeesByGroupIdAsync(Guid employeeGroupId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Employees
                .Include(e => e.EmployeeGroupLinks)
                .Where(e => e.EmployeeGroupLinks.Any(l => l.EmployeeGroupId == employeeGroupId))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Employee>> GetEmployeesByHourlyRateIdAsync(Guid hourlyRateId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Employees
                .Include(e => e.EmployeeGroupLinks)
                .Where(e => e.EmployeeGroupLinks.Any(l => l.HourlyRateId == hourlyRateId))
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            await _dbContext.Employees.AddAsync(employee, cancellationToken);
        }

        public async Task<Employee?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Employees
                .FirstOrDefaultAsync(e => e.UserId == userId, cancellationToken);
        }
    }
}
