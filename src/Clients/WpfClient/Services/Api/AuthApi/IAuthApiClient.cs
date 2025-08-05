using Auth.Contracts.Requests;
using Auth.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Services.Api.AuthApi
{
    public interface IAuthApiClient
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest request);
        Task<MyProfileResponse> GetMyProfileAsync();
        Task ChangePasswordAsync(ChangePasswordRequest request);
        Task DeleteMyAccountAsync(DeleteMyAccountRequest request);
        Task PrivilegedResetPasswordAsync(Guid userId, PrivilegedResetPasswordRequest request);
    }
}
