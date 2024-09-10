using Application.Abstractions.Repositories;
using Dapper;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data.Repositories
{
    public class WorkflowTemplateRepository : IWorkflowTemplateRepository
    {
         readonly string _connectionString;

        public WorkflowTemplateRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }
        public async Task<List<WorkFlowStep>> GetWorkflowTemplateSteps(int workFlowTemplateId)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = 
                @"SELECT WorkflowSteps.* 
                FROM WorkflowTemplates_WorkflowSteps 
                LEFT JOIN WorkflowSteps 
                ON WorkflowTemplates_WorkflowSteps.WorkflowStepId = WorkflowSteps.Id 
                WHERE WorkflowTemplateId = @workFlowTemplateId
                ORDER BY StepOrder";

                IEnumerable<WorkFlowStep> steps = await connection.QueryAsync<WorkFlowStep>(sql, new {workFlowTemplateId});

                return steps.ToList();
            };
        }
    }
}