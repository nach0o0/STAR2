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
    }
}
