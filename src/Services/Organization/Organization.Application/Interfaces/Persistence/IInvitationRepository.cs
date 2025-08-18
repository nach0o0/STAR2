using Organization.Domain.Entities;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Interfaces.Persistence
{
    public interface IInvitationRepository
    {
        Task<Invitation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Invitation>> GetForOrganizationAsync(Guid organizationId, CancellationToken cancellationToken = default);
        Task AddAsync(Invitation invitation, CancellationToken cancellationToken = default);
        void Delete(Invitation invitation);
        Task<List<Invitation>> GetByTargetEntityAsync(InvitationTargetEntityType targetType, Guid targetId, CancellationToken cancellationToken = default);
        Task<List<Invitation>> GetForInviteeAsync(Guid inviteeEmployeeId, CancellationToken cancellationToken = default);
        Task<List<Invitation>> GetSentByInviterAsync(Guid inviterEmployeeId, CancellationToken cancellationToken = default);
    }
}
