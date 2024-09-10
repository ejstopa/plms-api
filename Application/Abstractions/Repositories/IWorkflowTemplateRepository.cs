
using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IWorkflowTemplateRepository
    {
        public Task<List<WorkFlowStep>> GetWorkflowTemplateSteps(int workFlowTemplateId);
    }
}