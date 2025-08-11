using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Contracts.Responses
{
    public record FoundUserResponse(
        Guid UserId,
        string Email,
        string? FirstName,
        string? LastName
    );
}
