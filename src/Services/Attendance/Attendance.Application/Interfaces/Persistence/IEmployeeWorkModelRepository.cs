using Attendance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Interfaces.Persistence
{
    public interface IEmployeeWorkModelRepository
    {
        Task<EmployeeWorkModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(EmployeeWorkModel assignment, CancellationToken cancellationToken = default);
        void Delete(EmployeeWorkModel assignment);
        Task<List<EmployeeWorkModel>> GetByWorkModelIdAsync(Guid workModelId, CancellationToken cancellationToken = default);
        Task<EmployeeWorkModel?> GetActiveAssignmentForEmployeeAsync(Guid employeeId, DateTime date, CancellationToken cancellationToken = default);
        Task<List<EmployeeWorkModel>> GetAssignmentsForEmployeeAsync(Guid employeeId, CancellationToken cancellationToken = default);
        Task<List<EmployeeWorkModel>> GetActiveAssignmentsForEmployeeAsync(Guid employeeId, CancellationToken cancellationToken = default);
        Task<List<EmployeeWorkModel>> GetAssignmentsByWorkModelIdsAsync(IEnumerable<Guid> workModelIds, CancellationToken cancellationToken = default);
        Task<List<EmployeeWorkModel>> GetAssignmentsForEmployeeByDateRangeAsync(Guid employeeId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    }
}
