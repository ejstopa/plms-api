using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Results;
using MediatR;

namespace Application.Features.WorkflowInstances.Queries.GetWorkflowsByUser
{
    public class GetWorkflowsByUserQuery : IRequest<Result<List<WorkflowInstanceResponseDto>>>
    {
        public int UserId {get; set;}
    }
}