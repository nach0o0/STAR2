using Auth.Application.Features.Commands.RegisterUser;
using Auth.Application.Interfaces.Persistence;
using FluentValidation.TestHelper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.UnitTests.Features.Commands.RegisterUser
{
    public class RegisterUserCommandValidatorTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly RegisterUserCommandValidator _validator;

        public RegisterUserCommandValidatorTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _validator = new RegisterUserCommandValidator(_userRepositoryMock.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("not-an-email")]
        public void Validator_Should_HaveError_WhenEmailIsInvalid(string email)
        {
            // Arrange
            var command = new RegisterUserCommand(email, "Password123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Email);
        }

        [Fact]
        public async Task Validator_Should_HaveError_WhenEmailIsAlreadyTaken()
        {
            // Arrange
            var command = new RegisterUserCommand("taken@example.com", "Password123");
            _userRepositoryMock.Setup(r => r.UserExistsWithEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(true);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Email)
                  .WithErrorMessage("This email address is already in use.");
        }

        [Fact]
        public void Validator_Should_HaveError_WhenPasswordIsTooShort()
        {
            // Arrange
            var command = new RegisterUserCommand("test@example.com", "short");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Password);
        }

        [Fact]
        public async Task Validator_Should_NotHaveError_WhenCommandIsValid()
        {
            // Arrange
            var command = new RegisterUserCommand("unique@example.com", "ValidPassword123");
            _userRepositoryMock.Setup(r => r.UserExistsWithEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(false);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
