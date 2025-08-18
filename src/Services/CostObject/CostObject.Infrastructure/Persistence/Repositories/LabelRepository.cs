using CostObject.Application.Interfaces.Persistence;
using CostObject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Infrastructure.Persistence.Repositories
{
    public class LabelRepository : ILabelRepository
    {
        private readonly CostObjectDbContext _dbContext;

        public LabelRepository(CostObjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Label?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Labels.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task AddAsync(Label label, CancellationToken cancellationToken = default)
        {
            await _dbContext.Labels.AddAsync(label, cancellationToken);
        }

        public void Delete(Label label)
        {
            _dbContext.Labels.Remove(label);
        }
    }
}
