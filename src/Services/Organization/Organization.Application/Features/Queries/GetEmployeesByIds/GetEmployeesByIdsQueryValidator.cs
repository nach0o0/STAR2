using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeesByIds
{
    public class GetEmployeesByIdsQueryValidator : AbstractValidator<GetEmployeesByIdsQuery>
    {
        public GetEmployeesByIdsQueryValidator()
        {
            RuleFor(x => x.EmployeeIds)
                .NotEmpty().WithMessage("The list of employee IDs cannot be empty.")
                .Must(ids => ids != null && ids.Any()).WithMessage("You must provide at least one employee ID.");
        }
    }
}
