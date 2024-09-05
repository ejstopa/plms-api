using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Results;

namespace Application.Features.WorkflowInstances.CreateWorkflowInstance
{
    public interface ICreateWorkflowInstanceValidator
    {
         public Task<Result<bool>> Validate(CreateWorkflowInstanceCommand request);
    }
}