using CostObject.Application.Interfaces.Persistence;
using MediatR;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.DeleteLabel
{
    public class DeleteLabelCommandHandler : IRequestHandler<DeleteLabelCommand>
    {
        private readonly ILabelRepository _labelRepository;

        public DeleteLabelCommandHandler(ILabelRepository labelRepository)
        {
            _labelRepository = labelRepository;
        }

        public async Task Handle(DeleteLabelCommand command, CancellationToken cancellationToken)
        {
            var label = await _labelRepository.GetByIdAsync(command.LabelId, cancellationToken);

            if (label is null)
            {
                // Sollte durch den Authorizer abgedeckt sein, aber als Sicherheitsnetz.
                throw new NotFoundException(nameof(Domain.Entities.Label), command.LabelId);
            }

            label.PrepareForDeletion();
            _labelRepository.Delete(label);

            // Speichern und Event-Auslösung werden von der UnitOfWork-Pipeline übernommen.
        }
    }
}
