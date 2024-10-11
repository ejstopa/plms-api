using Domain.Results;
using MediatR;

namespace Application.Features.WorkflowInstances.Commands.RejectWorkflowReturn
{
    public class RejectWorkflowReturnCommand : IRequest<Result<WorkflowInstanceResponseDto>>
    {
        public int UserId { get; set; }
        public int WorkflowInstanceId { get; set; }
        public string Message {get; set; } = string.Empty;
        
    }
}