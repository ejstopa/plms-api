using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Results;
using MediatR;

namespace Application.Features.WorkflowInstances.Commands.IncrementWorkflowStep
{
    public class IncrementWorkflowStepCommand : IRequest<Result<WorkflowInstanceResponseDto?>>
    {
        public int WorkflowInstanceId {get; set;}
    }
}