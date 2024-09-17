using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Results;
using MediatR;

namespace Application.Features.WorkflowInstances.Queries.GetWorkflowInstancesByDepartment
{
    public class GetWorkflowInstancesByDepartmentQuery : IRequest<Result<List<WorkflowInstanceResponseDto>>>
    {
          public int DepartmentId {get; set;}
    }
}