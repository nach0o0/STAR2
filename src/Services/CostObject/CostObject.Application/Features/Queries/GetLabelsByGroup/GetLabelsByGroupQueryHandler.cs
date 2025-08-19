using CostObject.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetLabelsByGroup
{
    public class GetLabelsByGroupQueryHandler : IRequestHandler<GetLabelsByGroupQuery, List<GetLabelsByGroupQueryResult>>
    {
        private readonly ILabelRepository _labelRepository;

        public GetLabelsByGroupQueryHandler(ILabelRepository labelRepository)
        {
            _labelRepository = labelRepository;
        }

        public async Task<List<GetLabelsByGroupQueryResult>> Handle(GetLabelsByGroupQuery query, CancellationToken cancellationToken)
        {
            var labels = await _labelRepository.GetByGroupIdAsync(query.EmployeeGroupId, cancellationToken);

            return labels.Select(label => new GetLabelsByGroupQueryResult(
                label.Id,
                label.Name
            )).ToList();
        }
    }
}
