using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IWorkflowStepForkRepository
    {
        public Task<List<WorkflowStepFork>> GetStepForkByWorkflowAndStep(int workflowTemplateId, int workflowStepId);
    }
}