using CostObject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Interfaces.Persistence
{
    public interface ICostObjectRequestRepository
    {
        Task<CostObjectRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(CostObjectRequest costObjectRequest, CancellationToken cancellationToken = default);
        void Delete(CostObjectRequest costObjectRequest);
        Task<List<CostObjectRequest>> GetByGroupIdAsync(Guid employeeGroupId, CancellationToken cancellationToken = default);
        Task<List<CostObjectRequest>> GetPendingByRequesterAsync(Guid requesterEmployeeId, Guid employeeGroupId, CancellationToken cancellationToken = default);
        Task<List<CostObjectRequest>> GetByRequesterAndGroupAsync(Guid requesterEmployeeId, Guid employeeGroupId, CancellationToken cancellationToken = default);
    }
}
