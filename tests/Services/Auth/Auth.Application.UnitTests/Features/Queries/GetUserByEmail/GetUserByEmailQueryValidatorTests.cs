using Auth.Application.Features.Queries.GetUserByEmail;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.UnitTests.Features.Queries.GetUserByEmail
{
    public class GetUserByEmailQueryValidatorTests
    {
        private readonly GetUserByEmailQueryValidator _validator;

        public GetUserByEmailQueryValidatorTests()
        {
            _validator = new GetUserByEmailQueryValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("not-a-valid-email")]
        [InlineData("test@.com")]
        public void Validator_Should_HaveError_WhenEmailIsInvalid(string email)
        {
            // Arrange
            var query = new GetUserByEmailQuery(email);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(q => q.Email);
        }

        [Fact]
        public void Validator_Should_NotHaveError_WhenEmailIsValid()
        {
            // Arrange
            var query = new GetUserByEmailQuery("valid.email@example.com");

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
