using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.WorkflowInstances.CreateWorkflowInstance;
using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IWorkflowInstanceRepository
    {
        public Task<WorkflowInstance> CreateWorkflowInstance(WorkflowInstance workflowInstance);
    }
}