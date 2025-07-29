using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Interfaces.Persistence
{
    public interface IOrganizationRepository
    {
        Task<Domain.Entities.Organization?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Domain.Entities.Organization>> GetSubOrganizationsAsync(Guid parentId, CancellationToken cancellationToken = default);
        Task AddAsync(Domain.Entities.Organization organization, CancellationToken cancellationToken = default);
        void Delete(Domain.Entities.Organization organization);
        Task<bool> NameExistsAsync(string name, Guid? parentOrganizationId = null, CancellationToken cancellationToken = default);
        Task<bool> IsDefaultGroupOfAnyOrganizationAsync(Guid employeeGroupId, CancellationToken cancellationToken = default);
    }
}
