using Auth.Application.Features.Commands.ChangePassword;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.UnitTests.Features.Commands.ChangePassword
{
    public class ChangePasswordCommandValidatorTests
    {
        private readonly ChangePasswordCommandValidator _validator;

        public ChangePasswordCommandValidatorTests()
        {
            _validator = new ChangePasswordCommandValidator();
        }

        [Fact]
        public void Validator_Should_HaveError_WhenOldPasswordIsEmpty()
        {
            // Arrange
            var command = new ChangePasswordCommand("", "NewPassword123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.OldPassword);
        }

        [Fact]
        public void Validator_Should_HaveError_WhenNewPasswordIsEmpty()
        {
            // Arrange
            var command = new ChangePasswordCommand("OldPassword123", "");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.NewPassword);
        }

        [Fact]
        public void Validator_Should_HaveError_WhenNewPasswordIsTooShort()
        {
            // Arrange
            var command = new ChangePasswordCommand("OldPassword123", "short");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.NewPassword)
                  .WithErrorMessage("New password must be at least 8 characters long.");
        }

        [Fact]
        public void Validator_Should_NotHaveError_WhenCommandIsValid()
        {
            // Arrange
            var command = new ChangePasswordCommand("OldPassword123", "NewValidPassword123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
