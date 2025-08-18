using Attendance.Application.Interfaces.Persistence;
using Attendance.Domain.Entities;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.UpdateEmployeeWorkModelAssignment
{
    public class UpdateEmployeeWorkModelAssignmentCommandHandler : IRequestHandler<UpdateEmployeeWorkModelAssignmentCommand>
    {
        private readonly IEmployeeWorkModelRepository _assignmentRepository;

        public UpdateEmployeeWorkModelAssignmentCommandHandler(IEmployeeWorkModelRepository assignmentRepository)
        {
            _assignmentRepository = assignmentRepository;
        }

        public async Task Handle(UpdateEmployeeWorkModelAssignmentCommand command, CancellationToken cancellationToken)
        {
            var assignmentToUpdate = await _assignmentRepository.GetByIdAsync(command.AssignmentId, cancellationToken);
            if (assignmentToUpdate is null)
            {
                throw new NotFoundException(nameof(EmployeeWorkModel), command.AssignmentId);
            }

            // Geschäftsregel: Überlappungen prüfen
            var allAssignmentsForEmployee = await _assignmentRepository.GetAssignmentsForEmployeeAsync(assignmentToUpdate.EmployeeId, cancellationToken);
            var otherAssignments = allAssignmentsForEmployee.Where(a => a.Id != command.AssignmentId).ToList();

            var newValidFrom = command.ValidFrom?.Date ?? assignmentToUpdate.ValidFrom;
            var newValidTo = command.ValidTo?.Date;

            if (newValidTo.HasValue && newValidFrom > newValidTo.Value)
            {
                throw new ValidationException(new Dictionary<string, string[]> {
                    { "ValidTo", new[] { "ValidTo cannot be before ValidFrom." } }
                });
            }

            foreach (var other in otherAssignments)
            {
                // Prüfe auf Überlappung
                if (newValidFrom < (other.ValidTo ?? DateTime.MaxValue) && (newValidTo ?? DateTime.MaxValue) > other.ValidFrom)
                {
                    throw new ValidationException(new Dictionary<string, string[]> {
                        { "DateRange", new[] { "The new date range overlaps with an existing assignment." } }
                    });
                }
            }

            assignmentToUpdate.Update(newValidFrom, newValidTo);
        }
    }
}
