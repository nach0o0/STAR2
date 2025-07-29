using Organization.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Interfaces.Persistence
{
    public interface IHourlyRateRepository
    {
        Task<HourlyRate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<HourlyRate>> GetByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken = default);
        Task AddAsync(HourlyRate hourlyRate, CancellationToken cancellationToken = default);
        void Delete(HourlyRate hourlyRate);
    }
}
