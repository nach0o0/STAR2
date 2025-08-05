using Permission.Domain.Events.UserPermissionAssignments;
using Shared.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Domain.Entities
{
    public enum AssignmentType { ROLE, PERMISSION }

    public class UserPermissionAssignment : BaseEntity<Guid>
    {
        public Guid UserId { get; private set; }
        public string Scope { get; private set; }
        public AssignmentType AssignmentType { get; private set; }
        public Guid? RoleId { get; private set; }
        public string? PermissionId { get; private set; }

        private UserPermissionAssignment() { }

        // Fabrik-Methode, um eine Rolle zuzuweisen
        public static UserPermissionAssignment CreateForRole(Guid userId, string scope, Guid roleId)
        {
            var assignment = new UserPermissionAssignment
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Scope = scope,
                AssignmentType = AssignmentType.ROLE,
                RoleId = roleId,
                CreatedAt = DateTime.UtcNow
            };
            assignment.AddDomainEvent(new UserPermissionAssignmentCreatedEvent(assignment));
            return assignment;
        }

        // Fabrik-Methode, um eine direkte Berechtigung zuzuweisen
        public static UserPermissionAssignment CreateForPermission(Guid userId, string scope, string permissionId)
        {
            var assignment = new UserPermissionAssignment
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Scope = scope,
                AssignmentType = AssignmentType.PERMISSION,
                PermissionId = permissionId,
                CreatedAt = DateTime.UtcNow
            };
            assignment.AddDomainEvent(new UserPermissionAssignmentCreatedEvent(assignment));
            return assignment;
        }

        public void PrepareForDeletion()
        {
            AddDomainEvent(new UserPermissionAssignmentDeletedEvent(this));
        }
    }
}
