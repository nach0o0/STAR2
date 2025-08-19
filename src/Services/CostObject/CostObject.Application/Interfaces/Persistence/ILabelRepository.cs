using CostObject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Interfaces.Persistence
{
    public interface ILabelRepository
    {
        Task<Label?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(Label label, CancellationToken cancellationToken = default);
        void Delete(Label label);
        Task<List<Label>> GetByGroupIdAsync(Guid employeeGroupId, CancellationToken cancellationToken = default);
    }
}
