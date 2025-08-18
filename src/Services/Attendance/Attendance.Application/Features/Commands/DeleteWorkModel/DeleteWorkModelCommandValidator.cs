using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.DeleteWorkModel
{
    public class DeleteWorkModelCommandValidator : AbstractValidator<DeleteWorkModelCommand>
    {
        public DeleteWorkModelCommandValidator()
        {
            RuleFor(x => x.WorkModelId).NotEmpty();
        }
    }
}
