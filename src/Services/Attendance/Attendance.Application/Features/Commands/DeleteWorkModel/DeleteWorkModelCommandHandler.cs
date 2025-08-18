using Attendance.Application.Interfaces.Persistence;
using Attendance.Domain.Entities;
using MediatR;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.DeleteWorkModel
{
    public class DeleteWorkModelCommandHandler : IRequestHandler<DeleteWorkModelCommand>
    {
        private readonly IWorkModelRepository _workModelRepository;

        public DeleteWorkModelCommandHandler(IWorkModelRepository workModelRepository)
        {
            _workModelRepository = workModelRepository;
        }

        public async Task Handle(DeleteWorkModelCommand command, CancellationToken cancellationToken)
        {
            var workModel = await _workModelRepository.GetByIdAsync(command.WorkModelId, cancellationToken);
            if (workModel is null)
            {
                throw new NotFoundException(nameof(WorkModel), command.WorkModelId);
            }

            workModel.PrepareForDeletion();
            _workModelRepository.Delete(workModel);
        }
    }
}
