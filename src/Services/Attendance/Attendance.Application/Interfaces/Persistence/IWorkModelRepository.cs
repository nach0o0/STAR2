using Attendance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Interfaces.Persistence
{
    public interface IWorkModelRepository
    {
        Task<WorkModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(WorkModel model, CancellationToken cancellationToken = default);
        void Delete(WorkModel model);
        Task<List<WorkModel>> GetByEmployeeGroupIdAsync(Guid employeeGroupId, CancellationToken cancellationToken = default);
        Task<List<WorkModel>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    }
}
