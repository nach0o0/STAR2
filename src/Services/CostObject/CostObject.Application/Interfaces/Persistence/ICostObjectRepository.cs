using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Interfaces.Persistence
{
    public interface ICostObjectRepository
    {
        Task<Domain.Entities.CostObject?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(Domain.Entities.CostObject costObject, CancellationToken cancellationToken = default);
        void Delete(Domain.Entities.CostObject costObject);
    }
}
