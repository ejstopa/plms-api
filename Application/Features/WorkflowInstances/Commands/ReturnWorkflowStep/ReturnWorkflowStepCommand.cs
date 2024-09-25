using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Results;
using MediatR;

namespace Application.Features.WorkflowInstances.Commands.ReturnWorkflowStep
{
    public class ReturnWorkflowStepCommand : IRequest<Result<bool>>
    {
        public int WorkflowInstanceId {get; set;}
        public int NewStepId {get; set;}
        public string Message {get;set;} = string.Empty;
    }
}