using Auth.Application.Features.Queries.GetUsersByIds;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.UnitTests.Features.Queries.GetUsersByIds
{
    public class GetUsersByIdsQueryValidatorTests
    {
        private readonly GetUsersByIdsQueryValidator _validator;

        public GetUsersByIdsQueryValidatorTests()
        {
            _validator = new GetUsersByIdsQueryValidator();
        }

        [Fact]
        public void Validator_Should_HaveError_WhenUserIdsIsEmpty()
        {
            // Arrange
            var query = new GetUsersByIdsQuery(new List<Guid>());

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(q => q.UserIds);
        }

        [Fact]
        public void Validator_Should_NotHaveError_WhenUserIdsIsNotEmpty()
        {
            // Arrange
            var query = new GetUsersByIdsQuery(new List<Guid> { Guid.NewGuid() });

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
