using Attendance.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetWorkModelAssignmentsForEmployee
{
    public class GetWorkModelAssignmentsForEmployeeQueryHandler
        : IRequestHandler<GetWorkModelAssignmentsForEmployeeQuery, List<GetWorkModelAssignmentsForEmployeeQueryResult>>
    {
        private readonly IEmployeeWorkModelRepository _assignmentRepository;
        private readonly IWorkModelRepository _workModelRepository;

        public GetWorkModelAssignmentsForEmployeeQueryHandler(
            IEmployeeWorkModelRepository assignmentRepository,
            IWorkModelRepository workModelRepository)
        {
            _assignmentRepository = assignmentRepository;
            _workModelRepository = workModelRepository;
        }

        public async Task<List<GetWorkModelAssignmentsForEmployeeQueryResult>> Handle(
            GetWorkModelAssignmentsForEmployeeQuery request,
            CancellationToken cancellationToken)
        {
            var assignments = await _assignmentRepository.GetAssignmentsForEmployeeAsync(request.EmployeeId, cancellationToken);

            var result = new List<GetWorkModelAssignmentsForEmployeeQueryResult>();
            foreach (var assignment in assignments)
            {
                var workModel = await _workModelRepository.GetByIdAsync(assignment.WorkModelId, cancellationToken);
                result.Add(new GetWorkModelAssignmentsForEmployeeQueryResult(
                    assignment.Id,
                    assignment.WorkModelId,
                    workModel?.Name ?? "Unknown Model", // Fallback, falls Modell gelöscht wurde
                    assignment.ValidFrom,
                    assignment.ValidTo
                ));
            }

            return result;
        }
    }
}
