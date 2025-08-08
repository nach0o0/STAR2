using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Contracts.Responses
{
    public record DirectPermissionResponse(
        string PermissionId,
        string Description
    );
}
