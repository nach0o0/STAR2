using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Contracts.Responses
{
    public record UserDetailsResponse(
        Guid UserId, 
        string Email,
        string? FirstName,
        string? LastName
    );
}
