using Auth.Application.Features.Queries.GetMyProfile;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.UnitTests.Features.Queries.GetMyProfile
{
    public class GetMyProfileQueryValidatorTests
    {
        private readonly GetMyProfileQueryValidator _validator;

        public GetMyProfileQueryValidatorTests()
        {
            _validator = new GetMyProfileQueryValidator();
        }

        [Fact]
        public void Validator_Should_NotHaveError_ForValidQuery()
        {
            // Arrange
            var query = new GetMyProfileQuery();

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
