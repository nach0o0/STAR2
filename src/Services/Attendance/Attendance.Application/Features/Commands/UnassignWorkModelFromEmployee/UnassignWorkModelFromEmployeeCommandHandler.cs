using Attendance.Application.Interfaces.Persistence;
using Attendance.Domain.Entities;
using MediatR;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.UnassignWorkModelFromEmployee
{
    public class UnassignWorkModelFromEmployeeCommandHandler : IRequestHandler<UnassignWorkModelFromEmployeeCommand>
    {
        private readonly IEmployeeWorkModelRepository _employeeWorkModelRepository;

        public UnassignWorkModelFromEmployeeCommandHandler(IEmployeeWorkModelRepository employeeWorkModelRepository)
        {
            _employeeWorkModelRepository = employeeWorkModelRepository;
        }

        public async Task Handle(UnassignWorkModelFromEmployeeCommand command, CancellationToken cancellationToken)
        {
            var assignment = await _employeeWorkModelRepository.GetByIdAsync(command.EmployeeWorkModelId, cancellationToken);
            if (assignment is null)
            {
                throw new NotFoundException(nameof(EmployeeWorkModel), command.EmployeeWorkModelId);
            }

            // Ruft die Domain-Logik auf, um die Zuweisung zu beenden
            assignment.EndAssignment(command.EndDate);
        }
    }
}
