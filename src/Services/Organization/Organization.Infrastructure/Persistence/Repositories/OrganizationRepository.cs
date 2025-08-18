using Microsoft.EntityFrameworkCore;
using Organization.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Infrastructure.Persistence.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly OrganizationDbContext _dbContext;

        public OrganizationRepository(OrganizationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Domain.Entities.Organization?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Organizations.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<List<Domain.Entities.Organization>> GetSubOrganizationsAsync(Guid parentId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Organizations.Where(o => o.ParentOrganizationId == parentId).ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Domain.Entities.Organization organization, CancellationToken cancellationToken = default)
        {
            await _dbContext.Organizations.AddAsync(organization, cancellationToken);
        }

        public void Delete(Domain.Entities.Organization organization)
        {
            _dbContext.Organizations.Remove(organization);
        }

        public async Task<bool> NameExistsAsync(string name, Guid? parentOrganizationId = null, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Organizations
                .AnyAsync(org => org.Name == name && org.ParentOrganizationId == parentOrganizationId, cancellationToken);
        }

        public async Task<bool> IsDefaultGroupOfAnyOrganizationAsync(Guid employeeGroupId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Organizations
                .AnyAsync(o => o.DefaultEmployeeGroupId == employeeGroupId, cancellationToken);
        }

        public async Task<List<Domain.Entities.Organization>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Organizations
                .Where(o => ids.Contains(o.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Domain.Entities.Organization>> GetTopLevelOrganizationsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Organizations
                .Where(o => o.ParentOrganizationId == null)
                .ToListAsync(cancellationToken);
        }

        public async Task<Domain.Entities.Organization?> GetOrganizationHierarchyAsync(Guid organizationId, CancellationToken cancellationToken = default)
        {
            var root = await _dbContext.Organizations.FindAsync(new object[] { organizationId }, cancellationToken);
            if (root is null)
            {
                return null;
            }

            await LoadSubOrganizationsRecursiveAsync(root, cancellationToken);
            return root;
        }





        // Private Hilfsmethoden
        private async Task LoadSubOrganizationsRecursiveAsync(Domain.Entities.Organization organization, CancellationToken cancellationToken)
        {
            var children = await _dbContext.Organizations
                .Where(o => o.ParentOrganizationId == organization.Id)
                .ToListAsync(cancellationToken);

            foreach (var child in children)
            {
                // Rekursiver Aufruf für jedes Kind
                await LoadSubOrganizationsRecursiveAsync(child, cancellationToken);
            }
        }
    }
}
