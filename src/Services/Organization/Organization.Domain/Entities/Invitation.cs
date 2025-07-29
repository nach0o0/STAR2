using Organization.Domain.Events.Invitations;
using Shared.Domain.Abstractions;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Entities
{
    public enum InvitationStatus { Pending, Accepted, Declined, Expired }

    public class Invitation : BaseEntity<Guid>
    {
        public Guid InviterEmployeeId { get; private set; }
        public Guid InviteeEmployeeId { get; private set; }
        public InvitationTargetEntityType TargetEntityType { get; private set; }
        public Guid TargetEntityId { get; private set; }
        public InvitationStatus Status { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        private Invitation() { }

        public Invitation(Guid inviterEmployeeId, Guid inviteeEmployeeId, InvitationTargetEntityType targetType, Guid targetId, int expiresInDays)
        {
            Id = Guid.NewGuid();
            InviterEmployeeId = inviterEmployeeId;
            InviteeEmployeeId = inviteeEmployeeId;
            TargetEntityType = targetType;
            TargetEntityId = targetId;
            Status = InvitationStatus.Pending;
            ExpiresAt = DateTime.UtcNow.AddDays(expiresInDays);
            CreatedAt = DateTime.UtcNow;
            AddDomainEvent(new InvitationCreatedEvent(this));
        }

        public void Accept()
        {
            if (Status != InvitationStatus.Pending || IsExpired)
            {
                throw new InvalidOperationException("Invitation cannot be accepted.");
            }
            Status = InvitationStatus.Accepted;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new InvitationAcceptedEvent(this));
        }

        public void Decline()
        {
            if (Status != InvitationStatus.Pending || IsExpired)
            {
                throw new InvalidOperationException("Invitation cannot be declined.");
            }
            Status = InvitationStatus.Declined;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new InvitationDeclinedEvent(this));
        }

        public void Revoke()
        {
            if (Status != InvitationStatus.Pending)
            {
                throw new InvalidOperationException("Only pending invitations can be revoked.");
            }
            AddDomainEvent(new InvitationRevokedEvent(this));
        }
    }
}
