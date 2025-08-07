using CommunityToolkit.Mvvm.Messaging;
using Refit;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using WpfClient.Messages;
using WpfClient.Models;
using WpfClient.Security;
using WpfClient.Services.Api.Interfaces;

namespace WpfClient.Services.Application.Auth
{
    public partial class AuthService : IAuthService
    {
        private readonly IAuthApiClient _authApiClient;
        private readonly ISessionApiClient _sessionApiClient;
        private readonly IMessenger _messenger;

        private string? _accessToken;

        public AuthService(
            IAuthApiClient authApiClient,
            ISessionApiClient sessionApiClient,
            IMessenger messenger)
        {
            _authApiClient = authApiClient;
            _sessionApiClient = sessionApiClient;
            _messenger = messenger;
        }

        public string? GetAccessToken() => _accessToken;

        public async Task<bool> TryInitializeSessionAsync()
        {
            var refreshToken = ReadRefreshToken();
            if (string.IsNullOrEmpty(refreshToken))
            {
                return false;
            }

            try
            {
                var newTokens = await _sessionApiClient.RefreshSessionAsync(new(refreshToken));
                StoreAndBroadcastTokens(newTokens.AccessToken, newTokens.RefreshToken);
                return true;
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized || ex.StatusCode == HttpStatusCode.Forbidden)
            {
                await LogoutAsync();
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            // 1. Basis-Token vom Auth-Service holen
            var loginResponse = await _authApiClient.LoginAsync(new(email, password));
            if (loginResponse is null) return false;

            // 2. Session erstellen und angereicherte Tokens erhalten
            var sessionTokens = await _sessionApiClient.CreateSessionAsync(new(loginResponse.BasicToken));
            if (sessionTokens is null) return false;

            // 3. Tokens lokal speichern und Benutzerzustand initialisieren
            StoreAndBroadcastTokens(sessionTokens.AccessToken, sessionTokens.RefreshToken);
            return true;
        }

        public async Task LogoutAsync()
        {
            var refreshToken = ReadRefreshToken();
            if (refreshToken != null)
            {
                try { await _sessionApiClient.RevokeSessionAsync(new(refreshToken)); }
                catch { /* Ignore errors */ }
            }

            _accessToken = null;
            if (File.Exists("refresh.token"))
            {
                File.Delete("refresh.token");
            }
            _messenger.Send(new UserLoggedOutMessage());
        }

        public async Task<bool> RegisterAsync(string email, string password)
        {
            var response = await _authApiClient.RegisterAsync(new(email, password));

            if (response != null)
            {
                _messenger.Send(new LoginInfoMessage("Registration successful! Please log in."));
                return true;
            }

            return false;
        }

        public async Task ChangePasswordAsync(string oldPassword, string newPassword)
        {
            await _authApiClient.ChangePasswordAsync(new(oldPassword, newPassword));
            _messenger.Send(new LoginInfoMessage("Password changed successfully. Please log in again."));
            await LogoutAsync();
        }

        public async Task DeleteMyAccountAsync(string password)
        {
            await _authApiClient.DeleteMyAccountAsync(new(password));
            _messenger.Send(new LoginInfoMessage("Your account has been successfully deleted."));
            await LogoutAsync();
        }

        public async Task<List<ActiveSessionModel>> GetMyActiveSessionsAsync()
        {
            var sessionDtos = await _sessionApiClient.GetMySessionsAsync();
            return sessionDtos.Select(dto => new ActiveSessionModel
            {
                SessionId = dto.SessionId,
                CreatedAt = dto.CreatedAt,
                ClientInfo = dto.ClientInfo
            }).ToList();
        }

        // --- Private Helper-Methoden ---

        private void StoreAndBroadcastTokens(string accessToken, string refreshToken)
        {
            _accessToken = accessToken;
            var user = DecodeToken(accessToken);

            var encryptedToken = ProtectedData.Protect(Encoding.UTF8.GetBytes(refreshToken), null, DataProtectionScope.CurrentUser);
            File.WriteAllBytes("refresh.token", encryptedToken);

            if (user != null)
            {
                _messenger.Send(new UserLoggedInMessage(user));
            }
        }

        private CurrentUser? DecodeToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(accessToken);

            var userId = Guid.Parse(jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value);
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value ?? string.Empty;
            var employeeIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.EmployeeId);
            var orgIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.OrganizationId);
            var groupClaims = jwtToken.Claims.Where(c => c.Type == CustomClaimTypes.EmployeeGroupId).Select(c => Guid.Parse(c.Value)).ToList();
            var permissionsClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.PermissionsByScope)?.Value;

            var permissions = string.IsNullOrEmpty(permissionsClaim)
                ? new Dictionary<string, List<string>>()
                : JsonSerializer.Deserialize<Dictionary<string, List<string>>>(permissionsClaim)!;

            return new CurrentUser(
                userId,
                email,
                employeeIdClaim != null ? Guid.Parse(employeeIdClaim.Value) : null,
                orgIdClaim != null ? Guid.Parse(orgIdClaim.Value) : null,
                groupClaims,
                permissions);
        }

        private string? ReadRefreshToken()
        {
            if (!File.Exists("refresh.token")) return null;
            try
            {
                var encryptedToken = File.ReadAllBytes("refresh.token");
                var tokenBytes = ProtectedData.Unprotect(encryptedToken, null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(tokenBytes);
            }
            catch { return null; }
        }
    }
}