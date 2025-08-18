using Auth.Application.Features.Queries.Login;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.UnitTests.Features.Queries.Login
{
    public class LoginQueryValidatorTests
    {
        private readonly LoginQueryValidator _validator;

        public LoginQueryValidatorTests()
        {
            _validator = new LoginQueryValidator();
        }

        [Fact]
        public void Validator_Should_HaveError_WhenEmailIsEmpty()
        {
            // Arrange
            var query = new LoginQuery("", "password");

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(q => q.Email);
        }

        [Fact]
        public void Validator_Should_HaveError_WhenEmailIsInvalid()
        {
            // Arrange
            var query = new LoginQuery("not-an-email", "password");

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(q => q.Email)
                  .WithErrorMessage("A valid email is required.");
        }

        [Fact]
        public void Validator_Should_HaveError_WhenPasswordIsEmpty()
        {
            // Arrange
            var query = new LoginQuery("test@example.com", "");

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(q => q.Password);
        }

        [Fact]
        public void Validator_Should_NotHaveError_WhenQueryIsValid()
        {
            // Arrange
            var query = new LoginQuery("test@example.com", "password123");

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
