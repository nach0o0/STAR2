using Microsoft.EntityFrameworkCore;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Entities;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Infrastructure.Persistence.Repositories
{
    public class InvitationRepository : IInvitationRepository
    {
        private readonly OrganizationDbContext _dbContext;

        public InvitationRepository(OrganizationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Invitation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Invitations.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<List<Invitation>> GetForOrganizationAsync(Guid organizationId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Invitations
                .Where(i => i.TargetEntityType == InvitationTargetEntityType.Organization && i.TargetEntityId == organizationId)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Invitation invitation, CancellationToken cancellationToken = default)
        {
            await _dbContext.Invitations.AddAsync(invitation, cancellationToken);
        }

        public void Delete(Invitation invitation)
        {
            _dbContext.Invitations.Remove(invitation);
        }

        public async Task<List<Invitation>> GetByTargetEntityAsync(InvitationTargetEntityType targetType, Guid targetId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Invitations
                .Where(i => i.TargetEntityType == targetType &&
                             i.TargetEntityId == targetId &&
                             i.Status == InvitationStatus.Pending)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Invitation>> GetForInviteeAsync(Guid inviteeEmployeeId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Invitations
                .Where(i => i.InviteeEmployeeId == inviteeEmployeeId && i.Status == InvitationStatus.Pending)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Invitation>> GetSentByInviterAsync(Guid inviterEmployeeId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Invitations
                .Where(i => i.InviterEmployeeId == inviterEmployeeId)
                .ToListAsync(cancellationToken);
        }
    }
}
