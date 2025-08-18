using CostObject.Domain.Enums;
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
    }
}
