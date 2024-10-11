using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Application.Abstractions.FileSystem;
using Application.Abstractions.Repositories;
using Application.Features.WorkflowInstances;
using Dapper;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data.Repositories
{
    public class WorkflowInstanceRepository : IWorkflowInstanceRepository
    {
        readonly string _connectionString;
        private readonly IUserWorkflowFilesService _userWorkflowFilesService;

        public WorkflowInstanceRepository(IConfiguration configuration, IUserWorkflowFilesService userWorkflowFilesService)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
            _userWorkflowFilesService = userWorkflowFilesService;
        }

        public async Task<List<WorkflowInstance>> GetAllWorkflowInstances()
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = "SELECT * FROM WorkflowInstances WHERE Status = 'InWork'";

                List<WorkflowInstance> workflows = (await connection.QueryAsync<WorkflowInstance>(sql)).ToList();
                await AddWorkflowsItemsWithModels(workflows, connection);

                return workflows;
            }
        }
        
        public async Task<WorkflowInstance> CreateWorkflowInstance(WorkflowInstance workflowInstance)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = @"INSERT INTO WorkflowInstances VALUES(
                    @WorkflowTemplateId, @ItemId, @ItemName, @ItemRevision, 
                    @UserId, @CurrentStepId, @PreviousStepId, @Status,
                    @Message, @ItemFamilyId, @ReturnedBy);
                    SELECT CAST(SCOPE_IDENTITY() as INT)";

                int id = await connection.ExecuteScalarAsync<int>(sql, workflowInstance);

                return await connection.QueryFirstAsync<WorkflowInstance>("SELECT * FROM WorkflowInstances WHERE Id = @id", new { id });
            }
        }

        public async Task<List<WorkflowInstance>> GetWorkflowsByUserId(User user, bool onlyActiveWorkflows)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = "SELECT * FROM WorkflowInstances WHERE UserId = @userId";

                if (onlyActiveWorkflows)
                {
                    sql = $"{sql} AND Status = '{WorkflowStatus.InWork.ToString()}'";
                }

                IEnumerable<WorkflowInstance> workflows = await connection.QueryAsync<WorkflowInstance>(sql, new { userId = user.Id });
                await AddWorkflowsItemsWithModels(workflows.ToList(), connection);

                return workflows.ToList();
            }
        }

        public async Task<List<WorkFlowStep>> GetWorkflowInstancSteps(int workflowInstanceId, int itemFamilyId)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();

                string getWorkflowStepsSql =
                @"SELECT WorkflowSteps.*, WorkflowTemplates_WorkflowSteps.StepOrder
                FROM WorkflowSteps LEFT JOIN WorkflowTemplates_WorkflowSteps 
                ON WorkflowTemplates_WorkflowSteps.WorkflowStepId = WorkflowSteps.Id 
                WHERE WorkflowTemplates_WorkflowSteps.WorkflowTemplateId = (
                	SELECT WorkflowTemplates.Id 
                	FROM WorkflowInstances LEFT JOIN WorkflowTemplates 
                	ON WorkflowTemplates.Id = WorkflowInstances.WorkflowTemplateId 
                	WHERE WorkflowInstances.Id = @workflowInstanceId) 
                    ORDER BY WorkflowTemplates_WorkflowSteps.StepOrder";

                IEnumerable<WorkFlowStep> steps = await connection.QueryAsync<WorkFlowStep>(getWorkflowStepsSql, new { workflowInstanceId });

                foreach (WorkFlowStep step in steps)
                {
                    string getAttributesSql =
                    @"SELECT * 
                    FROM ItemAttributes 
                    WHERE Id IN(
                	SELECT DISTINCT WorkflowSteps_ItemAttributes.ItemAttributeId 
                	FROM WorkflowSteps_ItemAttributes 
                	WHERE WorkflowStepId = @stepId AND (ItemFamilyId IS NULL OR ItemFamilyId = @itemFamilyId))";

                    IEnumerable<ItemAttribute> attributes = await connection.QueryAsync<ItemAttribute>(
                        getAttributesSql, new { stepId = step.Id, itemFamilyId });
                    List<int> attributesIds = attributes.Select(attribute => attribute.Id).ToList();

                    string getAttributeOptionsSql = "SELECT * FROM ItemAttributeOptions WHERE ItemAttributeId IN @attributesIds";
                    IEnumerable<ItemAttributeOption> options = await connection.QueryAsync<ItemAttributeOption>(
                        getAttributeOptionsSql, new { attributesIds });

                    attributes.ToList().ForEach(attribute =>
                    {
                        List<ItemAttributeOption> attributeOptions = options.Where(option => option.ItemAttributeId == attribute.Id).ToList();
                        attribute.Options = attributeOptions;
                    });

                    step.ItemAttributes = attributes.ToList();
                }

                return steps.ToList();
            }
        }

        public async Task<WorkflowInstance?> UpdateWorkflowStep(
            int workflowInstanceId, int? newStepId, int? previousStepId, string? message = null, int? returnedBy = null)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql =
                @"UPDATE WorkflowInstances 
                SET CurrentStepId = @newStepId, PreviousStepId = @previousStepId, Message = @message, ReturnedBy = @returnedBy
                WHERE Id = @workflowInstanceId";

                await connection.ExecuteAsync(sql, new { newStepId, previousStepId, workflowInstanceId, message, returnedBy });

                return await connection.QueryFirstAsync<WorkflowInstance>(
                    "SELECT * FROM WorkflowInstances WHERE Id = @workflowInstanceId", new { workflowInstanceId });

            }
        }

        public async Task<WorkflowInstance?> GetWorkflowInstanceById(int workflowInstanceId, bool getWorkflowItem = false)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = "SELECT * FROM WorkflowInstances WHERE Id = @workflowInstanceId";

                WorkflowInstance? workflowInstance = await connection.QueryFirstOrDefaultAsync<WorkflowInstance>(sql, new { workflowInstanceId });

                if (workflowInstance is null)
                {
                    return null;
                }

                List<WorkflowInstance> workflows = [workflowInstance];

                if (getWorkflowItem)
                {
                    await AddWorkflowsItemsWithModels(workflows, connection);
                }

                return workflows[0];
            }
        }

        public async Task<List<WorkflowInstance>> GetWorkflowInstanceByStepIds(List<int> stepIds)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = "SELECT * FROM WorkflowInstances WHERE CurrentStepId IN @stepIdS";

                IEnumerable<WorkflowInstance> workflows = await connection.QueryAsync<WorkflowInstance>(sql, new { stepIds });
                await AddWorkflowsItemsWithModels(workflows.ToList(), connection);

                return workflows.ToList();
            }
        }

        public async Task<List<WorkFlowStep>?> GetWorkflowIStepsByDepartmentId(int departmentId)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = "SELECT * FROM WorkflowSteps WHERE DepartmentId = @departmentId";

                IEnumerable<WorkFlowStep> steps = await connection.QueryAsync<WorkFlowStep>(sql, new { departmentId });

                return steps.ToList();
            }
        }

        public async Task<WorkflowInstance?> SetWorkflowInstanceStatus(int workflowInstanceId, WorkflowStatus workflowStatus)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = @"UPDATE WorkflowInstances 
                            SET Status = @workflowStatus 
                            WHERE Id = @workflowInstanceId";

                int rowsEdited = await connection.ExecuteAsync(sql, new { workflowInstanceId, workflowStatus = workflowStatus.ToString() });

                if (rowsEdited == 0)
                {
                    return null;
                }

                return await connection.QueryFirstOrDefaultAsync<WorkflowInstance>("SELECT * FROM WorkflowInstances WHERE Id = @workflowInstanceId", new { workflowInstanceId });
            }
        }

        public async Task<bool> DeleteWorkflowInstance(int workflowInstanceId)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = "DELETE FROM WorkflowInstances WHERE Id = @workflowInstanceId";

                int rowsDeleted = await connection.ExecuteAsync(sql, new { workflowInstanceId });

                return rowsDeleted != 0;
            }
        }

        private async Task AddWorkflowsItemsWithModels(List<WorkflowInstance> workflows, SqlConnection connection)
        {
            foreach (WorkflowInstance workflow in workflows)
            {
                string getUserSql = "SELECT * FROM Users WHERE Id = @userId";
                User? user = await connection.QueryFirstOrDefaultAsync<User>(getUserSql, new { userId = workflow.UserId });

                if (user is null)
                {
                    return;
                }

                string userWorkflowsDirectory = _userWorkflowFilesService.GetUserWorkflowsDirectory(user);
                List<UserFile> userFiles = _userWorkflowFilesService.GetUserUserWorkflowFiles(user, [".prt", ".asm", ".drw", ".JPG"]);

                List<UserFile> workflowFiles = [..
                userFiles.Where(file => file.Name == workflow.ItemName)
                .OrderBy(file => file.Extension != ".drw" ? 0 : 1)];

                Item item = new()
                {
                    Name = workflow.ItemName,
                    Revision = workflow.ItemRevision,
                    Status = ItemStatus.inWorkflow.ToString()
                };

                List<Model> models = [];

                workflowFiles.ForEach(file =>
                {
                    Model model = new Model()
                    {
                        Name = file.Name,
                        Type = file.Extension,
                        Revision = item.Revision,
                        Version = file.Version,
                        FilePath = file.FullPath,
                    };

                    models.Add(model);
                });

                item.Models = models;
                workflow.Item = item;
            }

        }


    }


}
