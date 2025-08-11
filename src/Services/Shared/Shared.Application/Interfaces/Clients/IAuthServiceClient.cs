using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Application.Interfaces.Clients
{
    public interface IAuthServiceClient
    {
        Task<(Guid UserId, string Email, string? FirstName, string? LastName)?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<(Guid UserId, string Email)?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
