using Application.Abstractions.Repositories;
using Dapper;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data.Repositories
{
    public class WorkflowStepForkRepository : IWorkflowStepForkRepository
    {
        private readonly string _connectionString;

        public WorkflowStepForkRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<WorkflowStepFork>> GetStepForkByWorkflowAndStep(int workflowTemplateId, int workflowStepId)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = 
                @"SELECT * FROM StepForks 
                WHERE WorkflowTemplateId = @workflowTemplateId AND WorkflowStepId = @workflowStepId";

                IEnumerable<WorkflowStepFork> stepFork = await connection.QueryAsync<WorkflowStepFork>(sql, new {workflowTemplateId, workflowStepId});

                return stepFork.ToList();
            }
        }
    }
}