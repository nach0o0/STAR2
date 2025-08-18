using Auth.Domain.Entities;
using Auth.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Moq;
using Shared.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.UnitTests.Authentication
{
    public class JwtTokenGeneratorTests
    {
        private readonly JwtTokenGenerator _tokenGenerator;
        private readonly JwtOptions _jwtOptions;
        private readonly User _testUser;

        public JwtTokenGeneratorTests()
        {
            _jwtOptions = new JwtOptions
            {
                Secret = "this_is_a_super_secret_key_for_testing_12345",
                Issuer = "test_issuer",
                Audience = "test_audience"
            };

            // Wir erstellen ein Mock-Objekt für IOptions<JwtOptions>, das unsere Test-Konfiguration zurückgibt.
            var optionsMonitorMock = new Mock<IOptions<JwtOptions>>();
            optionsMonitorMock.Setup(o => o.Value).Returns(_jwtOptions);

            _tokenGenerator = new JwtTokenGenerator(optionsMonitorMock.Object);
            _testUser = new User("test.user@example.com", "any_hash");
        }

        [Fact]
        public void GenerateToken_Should_ReturnValidJwtTokenString()
        {
            // Act
            var tokenString = _tokenGenerator.GenerateToken(_testUser);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(tokenString));
        }

        [Fact]
        public void GenerateToken_Should_ContainCorrectClaimsForUser()
        {
            // Act
            var tokenString = _tokenGenerator.GenerateToken(_testUser);

            // Assert
            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(tokenString);

            var subClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            var emailClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);
            var jtiClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);

            Assert.NotNull(subClaim);
            Assert.Equal(_testUser.Id.ToString(), subClaim.Value);

            Assert.NotNull(emailClaim);
            Assert.Equal(_testUser.Email, emailClaim.Value);

            Assert.NotNull(jtiClaim); // Stellt sicher, dass jeder Token eine einzigartige ID hat
        }

        [Fact]
        public void GenerateToken_Should_HaveCorrectIssuerAndAudience()
        {
            // Act
            var tokenString = _tokenGenerator.GenerateToken(_testUser);

            // Assert
            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(tokenString);

            Assert.Equal(_jwtOptions.Issuer, decodedToken.Issuer);
            Assert.Equal(_jwtOptions.Audience, decodedToken.Audiences.First());
        }

        [Fact]
        public void GenerateToken_Should_NotContainSensitiveInformation()
        {
            // Arrange
            // Der Testbenutzer ist bereits im Konstruktor initialisiert.

            // Act
            var tokenString = _tokenGenerator.GenerateToken(_testUser);

            // Assert
            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(tokenString);

            // Der wichtigste Test hier: Sicherstellen, dass der PasswordHash NIEMALS im Token enthalten ist.
            var passwordHashClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == "password_hash");
            Assert.Null(passwordHashClaim);
        }

        [Fact]
        public void GenerateToken_Should_UseCorrectSigningAlgorithm()
        {
            // Arrange
            // Der Testbenutzer ist bereits im Konstruktor initialisiert.

            // Act
            var tokenString = _tokenGenerator.GenerateToken(_testUser);

            // Assert
            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(tokenString);

            // Überprüft, ob der Header des Tokens den erwarteten Algorithmus (HS256) enthält.
            Assert.Equal("HS256", decodedToken.Header.Alg);
        }
    }
}
