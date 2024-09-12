using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Errors;
using Domain.Results;
using MediatR;

namespace Application.Features.WorkflowInstances.Queries.GetWorkflowInstanceValue
{
    public class GetWorkflowInstanceValueHandler : IRequestHandler<GetWorkflowInstanceValueQuery, Result<List<WorkflowInstanceValue>>>
    {
        private readonly IWorkflowInstanceValueRepository _workflowInstanceValueRepository;

        public GetWorkflowInstanceValueHandler(IWorkflowInstanceValueRepository workflowInstanceValueRepository)
        {
            _workflowInstanceValueRepository = workflowInstanceValueRepository;
        }

        public async Task<Result<List<WorkflowInstanceValue>>> Handle(GetWorkflowInstanceValueQuery request, CancellationToken cancellationToken)
        {
            List<WorkflowInstanceValue> workflowInstanceValues = await _workflowInstanceValueRepository.GetWorkflowInstanceValues(request.WorkflowInstanceId);

            if (workflowInstanceValues.Count == 0)
            {
                return  Result<List<WorkflowInstanceValue>>.Failure(new Error(404, "Nenhum valor encontrado"));
            }

            return Result<List<WorkflowInstanceValue>>.Success(workflowInstanceValues);
        }
    }
}