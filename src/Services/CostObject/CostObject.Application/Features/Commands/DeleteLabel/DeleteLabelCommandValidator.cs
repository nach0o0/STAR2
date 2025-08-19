using CostObject.Application.Interfaces.Persistence;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.DeleteLabel
{
    public class DeleteLabelCommandValidator : AbstractValidator<DeleteLabelCommand>
    {
        public DeleteLabelCommandValidator(ICostObjectRepository costObjectRepository)
        {
            RuleFor(x => x.LabelId).NotEmpty();
        }
    }
}
