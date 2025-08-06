using Auth.Contracts.Requests;
using Auth.Contracts.Responses;
using Refit;

namespace WpfClient.Services.Api.Interfaces
{
    public interface IAuthApiClient
    {
        [Post("/api/authentication/login")]
        Task<LoginResponse> LoginAsync([Body] LoginRequest request);

        [Post("/api/authentication/register")]
        Task<RegisterUserResponse> RegisterAsync([Body] RegisterUserRequest request);

        [Get("/api/users/me")]
        Task<MyProfileResponse> GetMyProfileAsync();

        [Post("/api/users/me/change-password")]
        Task ChangePasswordAsync([Body] ChangePasswordRequest request);

        [Delete("/api/users/me")]
        Task DeleteMyAccountAsync([Body] DeleteMyAccountRequest request);

        [Post("/api/users/{userId}/privileged-reset-password")]
        Task PrivilegedResetPasswordAsync(Guid userId, [Body] PrivilegedResetPasswordRequest request);
    }
}
