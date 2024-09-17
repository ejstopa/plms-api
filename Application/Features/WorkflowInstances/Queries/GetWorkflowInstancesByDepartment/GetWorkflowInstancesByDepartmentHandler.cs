
using Application.Abstractions.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Results;
using MediatR;

namespace Application.Features.WorkflowInstances.Queries.GetWorkflowInstancesByDepartment
{
    public class GetWorkflowInstancesByDepartment : IRequestHandler<GetWorkflowInstancesByDepartmentQuery, Result<List<WorkflowInstanceResponseDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IWorkflowInstanceRepository _workflowInstanceRepository;

        public GetWorkflowInstancesByDepartment(IMapper mapper, IWorkflowInstanceRepository workflowInstanceRepository)
        {
            _mapper = mapper;
            _workflowInstanceRepository = workflowInstanceRepository;
        }
        public async Task<Result<List<WorkflowInstanceResponseDto>>> Handle(GetWorkflowInstancesByDepartmentQuery request, CancellationToken cancellationToken)
        {
            List<WorkFlowStep>? steps = await _workflowInstanceRepository.GetWorkflowIStepsByDepartmentId(request.DepartmentId);
            List<int> stepIds = steps!.Select(step => step.Id).ToList();

            List<WorkflowInstance> workflowInstances = await _workflowInstanceRepository.GetWorkflowInstanceByStepIds(stepIds);

            return Result<List<WorkflowInstanceResponseDto>>.Success( _mapper.Map<List<WorkflowInstanceResponseDto>>(workflowInstances));



        }
    }
}