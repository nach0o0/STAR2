using Auth.Application.Features.Commands.PrivilegedResetPassword;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.UnitTests.Features.Commands.PrivilegedResetPassword
{
    public class PrivilegedResetPasswordCommandValidatorTests
    {
        private readonly PrivilegedResetPasswordCommandValidator _validator;

        public PrivilegedResetPasswordCommandValidatorTests()
        {
            _validator = new PrivilegedResetPasswordCommandValidator();
        }

        [Fact]
        public void Validator_Should_NotHaveError_ForAnyValidCommand()
        {
            // Arrange
            var command = new PrivilegedResetPasswordCommand(Guid.NewGuid(), "NewPassword123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
