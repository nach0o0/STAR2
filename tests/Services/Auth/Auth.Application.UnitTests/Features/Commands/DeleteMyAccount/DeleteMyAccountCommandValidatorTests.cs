using Auth.Application.Features.Commands.DeleteMyAccount;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.UnitTests.Features.Commands.DeleteMyAccount
{
    public class DeleteMyAccountCommandValidatorTests
    {
        private readonly DeleteMyAccountCommandValidator _validator;

        public DeleteMyAccountCommandValidatorTests()
        {
            _validator = new DeleteMyAccountCommandValidator();
        }

        [Fact]
        public void Validator_Should_HaveError_WhenPasswordIsEmpty()
        {
            // Arrange
            var command = new DeleteMyAccountCommand("");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Password);
        }

        [Fact]
        public void Validator_Should_NotHaveError_WhenPasswordIsProvided()
        {
            // Arrange
            var command = new DeleteMyAccountCommand("some_password");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
