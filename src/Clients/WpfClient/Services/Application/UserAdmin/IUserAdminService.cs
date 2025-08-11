using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Models;

namespace WpfClient.Services.Application.UserAdmin
{
    public interface IUserAdminService
    {
        Task<List<UserModel>> GetRelevantUsersForScopeAsync(string scope);
        Task<UserModel?> FindUserByEmailAsync(string email, string scope);
        Task PrivilegedResetPasswordAsync(Guid userId, string newPassword);
    }
}
