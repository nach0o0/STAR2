using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Models;

namespace WpfClient.Services.Application.Auth
{
    public interface IAuthService
    {
        bool IsLoggedIn { get; }
        CurrentUser? CurrentUser { get; }
        Task TryLoadSessionAsync();
        Task<bool> LoginAsync(string email, string password);
        Task LogoutAsync();
        Task<bool> RegisterAsync(string email, string password);
        Task ChangePasswordAsync(string oldPassword, string newPassword);
        Task DeleteMyAccountAsync(string password); 
        string? GetAccessToken();
        Task<bool> RefreshTokensAsync();
        Task InitializeAsync();
        Task<List<ActiveSessionModel>> GetMyActiveSessionsAsync();
    }
}
