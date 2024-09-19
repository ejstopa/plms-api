using System.Reflection;
using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IWorkflowInstanceValueRepository
    {
        public Task<List<WorkflowInstanceValue>> GetWorkflowInstanceValues(int workflowInstanceId);
        public Task<WorkflowInstanceValue?> GetWorkflowInstanceValue(int workflowInstanceId, int attributeId);
        public Task<WorkflowInstanceValue?> CreateWorkflowInstanceValue(int workflowInstanceId, int attributeId, string value);
        public Task<WorkflowInstanceValue?> CreateWorkflowInstanceValue(int workflowInstanceId, int attributeId, double value);
        public Task<WorkflowInstanceValue?> UpdateWorkflowInstanceValue(int workflowInstanceId, int attributeId, string value);
        public Task<WorkflowInstanceValue?> UpdateWorkflowInstanceValue(int workflowInstanceId, int attributeId, double value);
    }
}