using Application.Abstractions.Repositories;
using Domain.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data.Repositories
{
    public class WorkflowInstanceValueRepository : IWorkflowInstanceValueRepository
    {
        private readonly string _connectionString;

        public WorkflowInstanceValueRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<WorkflowInstanceValue>> GetWorkflowInstanceValues(int workflowInstanceId)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = "SELECT * FROM WorkflowInstanceValues WHERE WorkflowInstanceId = @workflowInstanceId";

                IEnumerable<WorkflowInstanceValue> workflowInstanceValues = await connection.QueryAsync<WorkflowInstanceValue>(
                    sql, new{workflowInstanceId});
                
                return workflowInstanceValues.ToList();
            }
        }

        public async Task<WorkflowInstanceValue?> GetWorkflowInstanceValue(int WorkflowInstanceId, int attributeId)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = "SELECT * FROM WorkflowInstanceValues WHERE WorkflowInstanceId = @WorkflowInstanceId AND ItemAttributeId = @attributeId";

                WorkflowInstanceValue? attributeValue = await connection.QueryFirstOrDefaultAsync<WorkflowInstanceValue>(
                    sql, new { WorkflowInstanceId, attributeId });

                return attributeValue;
            };
        }

        public async Task<WorkflowInstanceValue?> CreateWorkflowInstanceValue(int WorkflowInstanceId, int attributeId, string value)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql =
                @"INSERT INTO WorkflowInstanceValues (WorkflowInstanceId, ItemAttributeId, ItemAttributeValueString) 
                VALUES (@WorkflowInstanceId, @attributeId, @value) 
                SELECT CAST(SCOPE_IDENTITY() as INT)";

                int id = await connection.ExecuteScalarAsync<int>(sql, new { WorkflowInstanceId, attributeId, value });

                return await connection.QueryFirstOrDefaultAsync<WorkflowInstanceValue>(
                    "SELECT * FROM WorkflowInstanceValues WHERE Id = @id", new { id });
            }
        }

        public async Task<WorkflowInstanceValue?> CreateWorkflowInstanceValue(int WorkflowInstanceId, int attributeId, double value)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql =
                @"INSERT INTO WorkflowInstanceValues (WorkflowInstanceId, ItemAttributeId, ItemAttributeValueNumber) 
                VALUES (@WorkflowInstanceId, @attributeId, @value) 
                SELECT CAST(SCOPE_IDENTITY() as INT)";

                int id = await connection.ExecuteScalarAsync<int>(sql, new { WorkflowInstanceId, attributeId, value });

                return await connection.QueryFirstOrDefaultAsync<WorkflowInstanceValue>(
                    "SELECT * FROM WorkflowInstanceValues WHERE Id = @id", new { id });
            }
        }

        public async Task<WorkflowInstanceValue?> UpdateWorkflowInstanceValue(int WorkflowInstanceId, int attributeId, string value)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql =
                @"UPDATE WorkflowInstanceValues 
                SET ItemAttributeValueString = @value 
                WHERE WorkflowInstanceId = @WorkflowInstanceId AND ItemAttributeId = @attributeId
                SELECT CAST(SCOPE_IDENTITY() as INT)";

                await connection.ExecuteScalarAsync<int>(sql, new { value, WorkflowInstanceId, attributeId });

                string selectWorkflowValueSql = 
                @"SELECT * FROM WorkflowInstanceValues 
                WHERE WorkflowInstanceId = @WorkflowInstanceId AND ItemAttributeId = @attributeId";

                return await connection.QueryFirstOrDefaultAsync<WorkflowInstanceValue>(selectWorkflowValueSql, new { value, WorkflowInstanceId, attributeId });
            }
        }

        public async Task<WorkflowInstanceValue?> UpdateWorkflowInstanceValue(int WorkflowInstanceId, int attributeId, double value)
        {
            using (SqlConnection connection = new(_connectionString))
            {               
                string sql =
                @"UPDATE WorkflowInstanceValues 
                SET ItemAttributeValueNumber = @value 
                WHERE WorkflowInstanceId = @WorkflowInstanceId AND ItemAttributeId = @attributeId";
            
                await connection.ExecuteAsync(sql, new { value, WorkflowInstanceId, attributeId });

                string selectWorkflowValueSql = 
                @"SELECT * FROM WorkflowInstanceValues 
                WHERE WorkflowInstanceId = @WorkflowInstanceId AND ItemAttributeId = @attributeId";

                return await connection.QueryFirstOrDefaultAsync<WorkflowInstanceValue>(selectWorkflowValueSql, new { value, WorkflowInstanceId, attributeId });
            }
        }

    }
}