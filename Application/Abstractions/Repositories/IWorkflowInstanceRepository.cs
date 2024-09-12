using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.WorkflowInstances;
using Application.Features.WorkflowInstances.Commands.CreateWorkflowInstance;
using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IWorkflowInstanceRepository
    {
        public Task<WorkflowInstance> CreateWorkflowInstance(WorkflowInstance workflowInstance);
        public Task<List<WorkflowInstance>> GetWorkflowsByUserId(User user);
        public Task<List<WorkFlowStep>?> GetWorkflowInstancSteps(int workflowInstanceId);
        public Task<WorkflowInstance?> UpdateWorkflowStep(int workflowInstanceId, int newStepId, int previousStepId);
        public Task<WorkflowInstance?> GetWorkflowInstanceById(int workflowInstanceId);
        public Task<List<WorkflowInstance>> GetWorkflowInstanceByStepIds(List<int> stepIds);
        public Task<List<WorkFlowStep>?> GetWorkflowIStepsByDepartmentId(int departmentId);
    }
}