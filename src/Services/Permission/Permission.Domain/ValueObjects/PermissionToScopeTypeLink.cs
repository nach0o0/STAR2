using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Domain.ValueObjects
{
    public class PermissionToScopeTypeLink
    {
        public string PermissionId { get; private set; }
        public string ScopeType { get; private set; }

        private PermissionToScopeTypeLink() { }

        public PermissionToScopeTypeLink(string permissionId, string scopeType)
        {
            PermissionId = permissionId;
            ScopeType = scopeType;
        }
    }
}
