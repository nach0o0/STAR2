using Auth.Application.Features.Queries.GetUserById;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.UnitTests.Features.Queries.GetUserById
{
    public class GetUserByIdQueryValidatorTests
    {
        private readonly GetUserByIdQueryValidator _validator;

        public GetUserByIdQueryValidatorTests()
        {
            _validator = new GetUserByIdQueryValidator();
        }

        [Fact]
        public void Validator_Should_HaveError_WhenUserIdIsEmpty()
        {
            // Arrange
            var query = new GetUserByIdQuery(Guid.Empty);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(q => q.UserId);
        }

        [Fact]
        public void Validator_Should_NotHaveError_WhenUserIdIsValid()
        {
            // Arrange
            var query = new GetUserByIdQuery(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
