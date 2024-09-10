using MediatR;

namespace Application.Features.WorkflowInstances.Queries.GetWorkFlowIstanceSteps
{
    public class GetWorkFlowIstanceStepsQuery : IRequest<List<WorkflowStepResponseDto>>
    {
        public int WorkflowInsanceId {get; set;}
    }
}