using Attendance.Application.Interfaces.Persistence;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.DeleteAttendanceType
{
    public class DeleteAttendanceTypeCommandValidator : AbstractValidator<DeleteAttendanceTypeCommand>
    {
        public DeleteAttendanceTypeCommandValidator(IAttendanceEntryRepository attendanceEntryRepository)
        {
            RuleFor(x => x.AttendanceTypeId).NotEmpty();

            // Geschäftsregel: Ein Anwesenheitstyp kann nicht gelöscht werden, wenn er verwendet wird.
            RuleFor(x => x.AttendanceTypeId)
                .MustAsync(async (id, cancellationToken) => !await attendanceEntryRepository.IsAttendanceTypeInUseAsync(id, cancellationToken))
                .WithMessage("This attendance type cannot be deleted because it is currently in use.");
        }
    }
}
