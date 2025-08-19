using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.ApproveCostObjectRequest
{
    public class ApproveCostObjectRequestCommandValidator : AbstractValidator<ApproveCostObjectRequestCommand>
    {
        public ApproveCostObjectRequestCommandValidator()
        {
            RuleFor(x => x.CostObjectRequestId).NotEmpty();
        }
    }
}
