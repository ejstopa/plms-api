using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.WorkflowInstances.Commands.CreateWorkflowInstance;
using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IWorkflowInstanceRepository
    {
        public Task<WorkflowInstance> CreateWorkflowInstance(WorkflowInstance workflowInstance);
        public Task<List<WorkflowInstance>> GetWorkflowsByUserId(User user);
    }
}