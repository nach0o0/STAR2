using CostObject.Domain.Enums;
using CostObject.Domain.Events.CostObjectRequests;
using Shared.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Domain.Entities
{
    public class CostObjectRequest : BaseEntity<Guid>
    {
        public Guid CostObjectId { get; private set; }
        public Guid RequesterEmployeeId { get; private set; }
        public Guid EmployeeGroupId { get; private set; }
        public ApprovalStatus Status { get; private set; }
        public string? RejectionReason { get; private set; }
        public Guid? ApproverEmployeeId { get; private set; }
        public DateTime? ResolvedAt { get; private set; }
        public Guid? ReassignmentCostObjectId { get; private set; }

        private CostObjectRequest() { }

        public CostObjectRequest(Guid costObjectId, Guid requesterEmployeeId, Guid employeeGroupId)
        {
            Id = Guid.NewGuid();
            CostObjectId = costObjectId;
            RequesterEmployeeId = requesterEmployeeId;
            EmployeeGroupId = employeeGroupId;
            Status = ApprovalStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        public void Approve(Guid approverId)
        {
            if (Status != ApprovalStatus.Pending)
            {
                throw new InvalidOperationException("Only pending requests can be approved.");
            }
            Status = ApprovalStatus.Approved;
            ApproverEmployeeId = approverId;
            ResolvedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new CostObjectRequestApprovedEvent(this));
        }

        public void Reject(Guid approverId, string reason, Guid? reassignmentCostObjectId = null)
        {
            if (Status != ApprovalStatus.Pending)
            {
                throw new InvalidOperationException("Only pending requests can be rejected.");
            }
            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new ArgumentException("A rejection reason must be provided.", nameof(reason));
            }
            if (reassignmentCostObjectId == this.CostObjectId)
            {
                throw new InvalidOperationException("A cost object cannot be reassigned to itself.");
            }

            Status = ApprovalStatus.Rejected;
            ApproverEmployeeId = approverId;
            RejectionReason = reason;
            ReassignmentCostObjectId = reassignmentCostObjectId;
            ResolvedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new CostObjectRequestRejectedEvent(this, this.ReassignmentCostObjectId));
        }
    }
}
