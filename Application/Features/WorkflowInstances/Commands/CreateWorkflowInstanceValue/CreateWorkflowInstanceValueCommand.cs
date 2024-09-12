using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Results;
using MediatR;

namespace Application.Features.WorkflowInstances.Commands.CreateWorkflowInstanceValue
{
    public class CreateWorkflowInstanceValueCommand : IRequest<Result<WorkflowInstanceValue?>>
    {
        public int WorkflowInstanceId { get; set; }
        public int ItemAttributeId { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}