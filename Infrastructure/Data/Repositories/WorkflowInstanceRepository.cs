using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.Repositories;
using Dapper;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data.Repositories
{
    public class WorkflowInstanceRepository : IWorkflowInstanceRepository
    {
        readonly string _connectionString;

        public WorkflowInstanceRepository(IConfiguration configuration )
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<WorkflowInstance> CreateWorkflowInstance(WorkflowInstance workflowInstance)
        {
            using (SqlConnection connection = new (_connectionString))
            {
                 string sql = @"INSERT INTO WorkflowInstances VALUES(
                    @WorkflowTemplateId, @ItemId, @ItemName, @ItemRevision, 
                    @UserId, @CurrentStepId, @PreviousStepId, @Status,
                    @Message);
                    SELECT CAST(SCOPE_IDENTITY() as INT)";

                int id = await connection.ExecuteScalarAsync<int>(sql, workflowInstance);

                return await connection.QueryFirstAsync<WorkflowInstance>("SELECT * FROM WorkflowInstances WHERE Id = @id", new {id});
            }
        }
    }
}