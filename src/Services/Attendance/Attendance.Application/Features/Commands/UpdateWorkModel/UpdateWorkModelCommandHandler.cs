using Attendance.Application.Interfaces.Persistence;
using Attendance.Domain.Entities;
using MediatR;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.UpdateWorkModel
{
    public class UpdateWorkModelCommandHandler : IRequestHandler<UpdateWorkModelCommand>
    {
        private readonly IWorkModelRepository _workModelRepository;

        public UpdateWorkModelCommandHandler(IWorkModelRepository workModelRepository)
        {
            _workModelRepository = workModelRepository;
        }

        public async Task Handle(UpdateWorkModelCommand command, CancellationToken cancellationToken)
        {
            var workModel = await _workModelRepository.GetByIdAsync(command.WorkModelId, cancellationToken);
            if (workModel is null)
            {
                throw new NotFoundException(nameof(WorkModel), command.WorkModelId);
            }

            workModel.Update(
                command.Name,
                command.MondayHours,
                command.TuesdayHours,
                command.WednesdayHours,
                command.ThursdayHours,
                command.FridayHours,
                command.SaturdayHours,
                command.SundayHours
            );
        }
    }
}
