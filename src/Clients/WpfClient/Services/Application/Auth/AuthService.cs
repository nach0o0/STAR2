using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WpfClient.Models;
using WpfClient.Security;
using WpfClient.Services.Api.AuthApi;
using WpfClient.Services.Api.SessionApi;
using WpfClient.Services.Application.MyEmployeeProfile;
using WpfClient.Services.Application.Navigation;

namespace WpfClient.Services.Application.Auth
{
    public partial class AuthService : ObservableObject, IAuthService
    {
        private readonly IAuthApiClient _authApiClient;
        private readonly ISessionApiClient _sessionApiClient;
        private readonly IMyEmployeeProfileService _myProfileService;

        [ObservableProperty]
        private bool _isLoggedIn;

        [ObservableProperty]
        private CurrentUser? _currentUser;

        private string? _accessToken;
        private DateTime _accessTokenExpiration;

        public AuthService(
            IAuthApiClient authApiClient,
            ISessionApiClient sessionApiClient,
            IMyEmployeeProfileService myProfileService)
        {
            _authApiClient = authApiClient;
            _sessionApiClient = sessionApiClient;
            _myProfileService = myProfileService;

            ClientAppMediator.ProfileCreated += HandleProfileCreatedAsync;
        }

        public async Task InitializeAsync()
        {
            if (await RefreshTokensAsync())
            {
                await _myProfileService.LoadProfileAsync();
            }
        }

        public async Task TryLoadSessionAsync()
        {
            await RefreshTokensAsync();
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var loginResponse = await _authApiClient.LoginAsync(new(email, password));
            if (loginResponse is null) return false;

            var sessionTokens = await _sessionApiClient.CreateSessionAsync(new(loginResponse.BasicToken));
            if (sessionTokens is not null)
            {
                StoreTokens(sessionTokens.AccessToken, sessionTokens.RefreshToken);
                IsLoggedIn = true;

                await ClientAppMediator.NotifyUserLoggedInAsync();

                return true;
            }
            return false;
        }

        public async Task LogoutAsync()
        {
            var refreshToken = ReadRefreshToken();
            if (refreshToken is not null)
            {
                // Versuch, die Session serverseitig zu beenden.
                // Fehler hier werden ignoriert, da der lokale Logout wichtiger ist.
                try { await _sessionApiClient.RevokeSessionAsync(new(refreshToken)); }
                catch { /* Ignorieren */ }
            }

            _accessToken = null;
            if (File.Exists("refresh.token"))
            {
                File.Delete("refresh.token");
            }
            ClientAppMediator.NotifyUserLoggedOut();
            CurrentUser = null;
            IsLoggedIn = false;
        }

        public async Task<bool> RegisterAsync(string email, string password)
        {
            var response = await _authApiClient.RegisterAsync(new(email, password));
            return response is not null;
        }

        public async Task ChangePasswordAsync(string oldPassword, string newPassword)
        {
            await _authApiClient.ChangePasswordAsync(new(oldPassword, newPassword));
        }

        public async Task DeleteMyAccountAsync(string password)
        {
            await _authApiClient.DeleteMyAccountAsync(new(password));
            // Nach erfolgreicher Löschung auf dem Server, führe den lokalen Logout durch.
            await LogoutAsync();
        }

        public string? GetAccessToken() => _accessToken;

        public async Task<bool> RefreshTokensAsync()
        {
            var refreshToken = ReadRefreshToken();
            if (string.IsNullOrEmpty(refreshToken))
            {
                await LogoutAsync();
                return false;
            }

            try
            {
                var newTokens = await _sessionApiClient.RefreshSessionAsync(new(refreshToken));
                if (newTokens is null)
                {
                    // Wenn der Refresh fehlschlägt, logge den Benutzer aus.
                    await LogoutAsync();
                    return false;
                }

                StoreTokens(newTokens.AccessToken, newTokens.RefreshToken);
                IsLoggedIn = true;
                return true;
            }
            catch
            {
                // Wenn ein Fehler auftritt (z.B. ungültiges Token), logge den Benutzer aus.
                await LogoutAsync();
                return false;
            }
        }

        private void StoreTokens(string accessToken, string refreshToken)
        {
            _accessToken = accessToken;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(accessToken);
            _accessTokenExpiration = jwtToken.ValidTo;

            // --- HIER PASSIERT DIE MAGIE ---
            // Extrahiere alle Claims aus dem Token und erstelle das CurrentUser-Objekt.
            var userId = Guid.Parse(jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value);
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value ?? string.Empty;
            var employeeIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.EmployeeId);
            var orgIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.OrganizationId);
            var groupClaims = jwtToken.Claims.Where(c => c.Type == CustomClaimTypes.EmployeeGroupId).Select(c => Guid.Parse(c.Value)).ToList();
            var permissionsClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.PermissionsByScope)?.Value;

            var permissions = string.IsNullOrEmpty(permissionsClaim)
                ? new Dictionary<string, List<string>>()
                : JsonSerializer.Deserialize<Dictionary<string, List<string>>>(permissionsClaim)!;

            CurrentUser = new CurrentUser(
                userId,
                email,
                employeeIdClaim is not null ? Guid.Parse(employeeIdClaim.Value) : null,
                orgIdClaim is not null ? Guid.Parse(orgIdClaim.Value) : null,
                groupClaims,
                permissions);

            var encryptedToken = ProtectedData.Protect(
                Encoding.UTF8.GetBytes(refreshToken),
                null,
                DataProtectionScope.CurrentUser);

            File.WriteAllBytes("refresh.token", encryptedToken);
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
            catch
            {
                // Wenn das Entschlüsseln fehlschlägt, ist das Token ungültig.
                return null;
            }
        }


        private async Task HandleProfileCreatedAsync()
        {
            // Führe die Token-Erneuerung durch.
            await RefreshTokensAsync();
        }

        public async Task<List<ActiveSessionModel>> GetMyActiveSessionsAsync()
        {
            var sessionDtos = await _sessionApiClient.GetMySessionsAsync();

            // Wandelt die DTOs in die client-seitigen Modelle um.
            return sessionDtos
                .Select(dto => new ActiveSessionModel
                {
                    SessionId = dto.SessionId,
                    CreatedAt = dto.CreatedAt,
                    ClientInfo = dto.ClientInfo
                })
                .ToList();
        }
    }
}
