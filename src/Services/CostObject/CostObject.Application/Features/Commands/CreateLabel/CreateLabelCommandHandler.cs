using CostObject.Application.Interfaces.Persistence;
using CostObject.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.CreateLabel
{
    public class CreateLabelCommandHandler : IRequestHandler<CreateLabelCommand, Guid>
    {
        private readonly ILabelRepository _labelRepository;

        public CreateLabelCommandHandler(ILabelRepository labelRepository)
        {
            _labelRepository = labelRepository;
        }

        public async Task<Guid> Handle(CreateLabelCommand command, CancellationToken cancellationToken)
        {
            var label = new Label(command.Name, command.EmployeeGroupId);

            await _labelRepository.AddAsync(label, cancellationToken);

            return label.Id;
        }
    }
}
