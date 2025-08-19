using CostObject.Application.Interfaces.Persistence;
using MediatR;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.UpdateLabel
{
    public class UpdateLabelCommandHandler : IRequestHandler<UpdateLabelCommand>
    {
        private readonly ILabelRepository _labelRepository;

        public UpdateLabelCommandHandler(ILabelRepository labelRepository)
        {
            _labelRepository = labelRepository;
        }

        public async Task Handle(UpdateLabelCommand command, CancellationToken cancellationToken)
        {
            var label = await _labelRepository.GetByIdAsync(command.LabelId, cancellationToken);

            if (label is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Label), command.LabelId);
            }

            label.Update(command.Name);
        }
    }
}
