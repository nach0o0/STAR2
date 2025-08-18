using Attendance.Application.Interfaces.Persistence;
using Attendance.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.AssignWorkModelToEmployee
{
    public class AssignWorkModelToEmployeeCommandHandler : IRequestHandler<AssignWorkModelToEmployeeCommand, Guid>
    {
        private readonly IEmployeeWorkModelRepository _employeeWorkModelRepository;

        public AssignWorkModelToEmployeeCommandHandler(IEmployeeWorkModelRepository employeeWorkModelRepository)
        {
            _employeeWorkModelRepository = employeeWorkModelRepository;
        }

        public async Task<Guid> Handle(AssignWorkModelToEmployeeCommand command, CancellationToken cancellationToken)
        {
            // Finde eine bestehende, offene Zuweisung für diesen Mitarbeiter
            var existingAssignment = await _employeeWorkModelRepository.GetActiveAssignmentForEmployeeAsync(command.EmployeeId, command.ValidFrom, cancellationToken);

            if (existingAssignment != null)
            {
                // Beende die alte Zuweisung einen Tag vor Beginn der neuen
                existingAssignment.EndAssignment(command.ValidFrom.AddDays(-1));
            }

            var newAssignment = new EmployeeWorkModel(
                command.EmployeeId,
                command.WorkModelId,
                command.ValidFrom,
                command.ValidTo
            );

            await _employeeWorkModelRepository.AddAsync(newAssignment, cancellationToken);

            return newAssignment.Id;
        }
    }
}
