using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Contracts.Requests
{
    public record PrivilegedResetPasswordRequest(string NewPassword);
}
