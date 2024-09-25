using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Results;
using MediatR;

namespace Application.Features.WorkflowInstances.Commands.DeleteWorkflowInstance
{
    public class DeleteWorkflowInstanceCommand : IRequest<Result<bool>>
    {
        public int WorkflowInstanceId {get; set;}
    }
}