using Attendance.Application.Interfaces.Persistence;
using Attendance.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.CreateWorkModel
{
    public class CreateWorkModelCommandHandler : IRequestHandler<CreateWorkModelCommand, Guid>
    {
        private readonly IWorkModelRepository _workModelRepository;

        public CreateWorkModelCommandHandler(IWorkModelRepository workModelRepository)
        {
            _workModelRepository = workModelRepository;
        }

        public async Task<Guid> Handle(CreateWorkModelCommand command, CancellationToken cancellationToken)
        {
            var workModel = new WorkModel(
                command.Name,
                command.EmployeeGroupId,
                command.MondayHours,
                command.TuesdayHours,
                command.WednesdayHours,
                command.ThursdayHours,
                command.FridayHours,
                command.SaturdayHours,
                command.SundayHours
            );

            await _workModelRepository.AddAsync(workModel, cancellationToken);

            return workModel.Id;
        }
    }
}
