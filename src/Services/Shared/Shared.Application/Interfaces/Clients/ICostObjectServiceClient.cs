using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Application.Interfaces.Clients
{
    public record CostObjectDto(Guid Id, string Name, Guid EmployeeGroupId);

    public interface ICostObjectServiceClient
    {
        Task<CostObjectDto?> GetByIdAsync(Guid costObjectId, CancellationToken cancellationToken = default);
        Task<List<CostObjectDto>> GetByIdsAsync(List<Guid> costObjectIds, CancellationToken cancellationToken = default);
    }
}
