using Auth.Application.Features.Commands.PrivilegedResetPassword;
using Moq;
using Shared.Application.Interfaces.Infrastructure;
using Shared.Application.Security;
using Shared.Domain.Authorization;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.UnitTests.Features.Commands.PrivilegedResetPassword
{
    public class PrivilegedResetPasswordCommandAuthorizerTests
    {
        private readonly Mock<IOrganizationServiceClient> _orgServiceClientMock;
        private readonly PrivilegedResetPasswordCommandAuthorizer _authorizer;

        public PrivilegedResetPasswordCommandAuthorizerTests()
        {
            _orgServiceClientMock = new Mock<IOrganizationServiceClient>();
            _authorizer = new PrivilegedResetPasswordCommandAuthorizer(_orgServiceClientMock.Object);
        }

        [Fact]
        public async Task AuthorizeAsync_Should_Succeed_WhenAdminHasPermissionInOrganizationScope()
        {
            // Arrange
            var targetUserId = Guid.NewGuid();
            var targetOrgId = Guid.NewGuid();
            var command = new PrivilegedResetPasswordCommand(targetUserId, "new_password");

            // Admin hat die Berechtigung im Scope der Organisation des Ziel-Benutzers
            var adminUser = new CurrentUser(
                Guid.NewGuid(), null, null, new List<Guid>(),
                new Dictionary<string, List<string>>
                {
                    { $"{PermittedScopeTypes.Organization}:{targetOrgId}", new List<string> { "user:privileged-reset-password" } }
                });

            // Der Ziel-Benutzer gehört zu dieser Organisation
            _orgServiceClientMock.Setup(c => c.GetEmployeeInfoByUserIdAsync(targetUserId, It.IsAny<CancellationToken>()))
                                 .ReturnsAsync((Guid.NewGuid(), targetOrgId, new List<Guid>()));

            // Act & Assert
            await _authorizer.AuthorizeAsync(command, adminUser, CancellationToken.None); // Sollte keine Exception werfen
        }

        [Fact]
        public async Task AuthorizeAsync_Should_Succeed_WhenAdminHasPermissionInGlobalScope_ForUserWithoutOrganization()
        {
            // Arrange
            var targetUserId = Guid.NewGuid();
            var command = new PrivilegedResetPasswordCommand(targetUserId, "new_password");

            // Admin hat die Berechtigung im globalen Scope
            var adminUser = new CurrentUser(
                Guid.NewGuid(), null, null, new List<Guid>(),
                new Dictionary<string, List<string>>
                {
                    { PermittedScopeTypes.Global, new List<string> { "user:privileged-reset-password" } }
                });

            // Der Ziel-Benutzer hat kein Mitarbeiterprofil oder keine Organisation
            _orgServiceClientMock.Setup(c => c.GetEmployeeInfoByUserIdAsync(targetUserId, It.IsAny<CancellationToken>()))
                                 .ReturnsAsync((ValueTuple<Guid, Guid?, List<Guid>>?)null);

            // Act & Assert
            await _authorizer.AuthorizeAsync(command, adminUser, CancellationToken.None); // Sollte keine Exception werfen
        }

        [Fact]
        public async Task AuthorizeAsync_Should_ThrowForbiddenAccessException_WhenAdminLacksPermission()
        {
            // Arrange
            var targetUserId = Guid.NewGuid();
            var targetOrgId = Guid.NewGuid();
            var command = new PrivilegedResetPasswordCommand(targetUserId, "new_password");

            // Admin hat KEINE relevante Berechtigung
            var adminUser = new CurrentUser(
                Guid.NewGuid(), null, null, new List<Guid>(), new Dictionary<string, List<string>>());

            // Der Ziel-Benutzer gehört zu einer Organisation
            _orgServiceClientMock.Setup(c => c.GetEmployeeInfoByUserIdAsync(targetUserId, It.IsAny<CancellationToken>()))
                                 .ReturnsAsync((Guid.NewGuid(), targetOrgId, new List<Guid>()));

            // Act & Assert
            await Assert.ThrowsAsync<ForbiddenAccessException>(() => _authorizer.AuthorizeAsync(command, adminUser, CancellationToken.None));
        }
    }
}
