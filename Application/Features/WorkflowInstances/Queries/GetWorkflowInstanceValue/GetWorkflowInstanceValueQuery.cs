using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Results;
using MediatR;

namespace Application.Features.WorkflowInstances.Queries.GetWorkflowInstanceValue
{
    public class GetWorkflowInstanceValueQuery : IRequest<Result<List<WorkflowInstanceValue>>>
    {
        public int WorkflowInstanceId {get; set;}
    }
}