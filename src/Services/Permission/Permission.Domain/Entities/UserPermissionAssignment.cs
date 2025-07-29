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
            return new UserPermissionAssignment
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Scope = scope,
                AssignmentType = AssignmentType.ROLE,
                RoleId = roleId,
                CreatedAt = DateTime.UtcNow
            };
        }

        // Fabrik-Methode, um eine direkte Berechtigung zuzuweisen
        public static UserPermissionAssignment CreateForPermission(Guid userId, string scope, string permissionId)
        {
            return new UserPermissionAssignment
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Scope = scope,
                AssignmentType = AssignmentType.PERMISSION,
                PermissionId = permissionId,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
