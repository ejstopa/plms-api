
using Application.Abstractions.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.WorkflowInstances.Queries.GetWorkFlowIstanceSteps
{
    public class GetWorkFlowIstanceStepsHanlder : IRequestHandler<GetWorkFlowIstanceStepsQuery, List<WorkflowStepResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IWorkflowInstanceRepository _workflowInstanceRepository;
        
        public GetWorkFlowIstanceStepsHanlder(IMapper mapper, IWorkflowInstanceRepository workflowInstanceRepository)
        {
            _mapper = mapper;
            _workflowInstanceRepository = workflowInstanceRepository;
            
        }
        public async Task<List<WorkflowStepResponseDto>> Handle(GetWorkFlowIstanceStepsQuery request, CancellationToken cancellationToken)
        {
            List<WorkFlowStep>? steps = await _workflowInstanceRepository.GetWorkflowInstancSteps(request.WorkflowInsanceId);

            return _mapper.Map<List<WorkflowStepResponseDto>>(steps);
        }
    }
}