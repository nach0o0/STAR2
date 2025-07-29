using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Authorization
{
    public static class PermittedScopeTypes
    {
        public const string Global = "global";
        public const string Organization = "organization";
        public const string EmployeeGroup = "employee_group";
    }
}
