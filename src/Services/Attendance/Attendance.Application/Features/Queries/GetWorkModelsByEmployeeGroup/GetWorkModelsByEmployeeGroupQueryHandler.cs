using Attendance.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetWorkModelsByEmployeeGroup
{
    public class GetWorkModelsByEmployeeGroupQueryHandler
        : IRequestHandler<GetWorkModelsByEmployeeGroupQuery, List<GetWorkModelsByEmployeeGroupQueryResult>>
    {
        private readonly IWorkModelRepository _workModelRepository;

        public GetWorkModelsByEmployeeGroupQueryHandler(IWorkModelRepository workModelRepository)
        {
            _workModelRepository = workModelRepository;
        }

        public async Task<List<GetWorkModelsByEmployeeGroupQueryResult>> Handle(
            GetWorkModelsByEmployeeGroupQuery request,
            CancellationToken cancellationToken)
        {
            var workModels = await _workModelRepository.GetByEmployeeGroupIdAsync(request.EmployeeGroupId, cancellationToken);

            return workModels
                .Select(wm => new GetWorkModelsByEmployeeGroupQueryResult(
                    wm.Id,
                    wm.Name,
                    wm.MondayHours,
                    wm.TuesdayHours,
                    wm.WednesdayHours,
                    wm.ThursdayHours,
                    wm.FridayHours,
                    wm.SaturdayHours,
                    wm.SundayHours))
                .ToList();
        }
    }
}
