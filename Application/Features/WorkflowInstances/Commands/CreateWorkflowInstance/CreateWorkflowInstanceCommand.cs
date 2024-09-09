using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Results;
using MediatR;

namespace Application.Features.WorkflowInstances.Commands.CreateWorkflowInstance
{
    public class CreateWorkflowInstanceCommand : IRequest<Result<WorkflowInstanceResponseDto>>
    {
        public int UserId {get; set;}
        public string ItemName {get; set;} = string.Empty;
    }
}