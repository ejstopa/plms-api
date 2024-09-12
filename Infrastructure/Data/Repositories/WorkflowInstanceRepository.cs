using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

        public async Task<WorkflowInstance> CreateWorkflowInstance(WorkflowInstance workflowInstance)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = @"INSERT INTO WorkflowInstances VALUES(
                    @WorkflowTemplateId, @ItemId, @ItemName, @ItemRevision, 
                    @UserId, @CurrentStepId, @PreviousStepId, @Status,
                    @Message, @ItemFamilyId);
                    SELECT CAST(SCOPE_IDENTITY() as INT)";

                int id = await connection.ExecuteScalarAsync<int>(sql, workflowInstance);

                return await connection.QueryFirstAsync<WorkflowInstance>("SELECT * FROM WorkflowInstances WHERE Id = @id", new { id });
            }
        }

        public async Task<List<WorkflowInstance>> GetWorkflowsByUserId(User user)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = "SELECT * FROM WorkflowInstances WHERE UserId = @userId";

                IEnumerable<WorkflowInstance> workflows = await connection.QueryAsync<WorkflowInstance>(sql, new { userId = user.Id });
                AddWorkflowsItemsWithModels(workflows.ToList(), user);

                return workflows.ToList();
            }
        }

        public async Task<List<WorkFlowStep>?> GetWorkflowInstancSteps(int workflowInstanceId)
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
                	WHERE WorkflowInstances.Id = @workflowInstanceId)";

                IEnumerable<WorkFlowStep> steps = await connection.QueryAsync<WorkFlowStep>(getWorkflowStepsSql, new { workflowInstanceId });

                foreach (WorkFlowStep step in steps)
                {

                    string getAttributesSql =
                    @"SELECT * 
                    FROM ItemAttributes 
                    WHERE Id IN(
                	SELECT DISTINCT WorkflowSteps_ItemAttributes.ItemAttributeId 
                	FROM WorkflowSteps_ItemAttributes 
                	WHERE WorkflowStepId = @stepId)";

                    IEnumerable<ItemAttribute> attributes = await connection.QueryAsync<ItemAttribute>(getAttributesSql, new { stepId = step.Id });
                    List<int> attributesIds = attributes.Select(attribute => attribute.Id).ToList();

                    string getAttributeOptionsSql = "SELECT * FROM ItemAttributeOptions WHERE ItemAttributeId IN @attributesIds";
                    IEnumerable<ItemAttributeOption> options = await connection.QueryAsync<ItemAttributeOption>(
                        getAttributeOptionsSql, new { attributesIds });

                    attributes.ToList().ForEach(attribute =>
                    {
                        List<ItemAttributeOption> attributeOptions = options.Where(option => option.ItemAttributeId == attribute.Id).ToList();
                        attribute.options = attributeOptions;
                    });

                    step.ItemAttributes = attributes.ToList();
                }

                return steps.ToList();
            }
        }

        private void AddWorkflowsItemsWithModels(List<WorkflowInstance> workflows, User user)
        {
            foreach (WorkflowInstance workflow in workflows)
            {
                string userWorkflowsDirectory = _userWorkflowFilesService.GetUserWorkflowsDirectory(user);
                List<UserFile> userFiles = _userWorkflowFilesService.GetUserUserWorkflowFiles(user, [".prt", ".asm", ".drw"]);

                List<UserFile> workflowFiles = [..
                userFiles.Where(file => file.Name == workflow.ItemName)
                .OrderBy(file => file.Extension != ".drw" ? 0 : 1)];

                Item item = new()
                {
                    Name = workflow.ItemName,
                    LastRevision = workflow.ItemRevision,
                    Status = ItemStatus.inWorkflow.ToString()
                };

                List<Model> models = [];

                workflowFiles.ForEach(file =>
                {
                    Model model = new Model()
                    {
                        Name = file.Name,
                        Type = file.Extension,
                        Revision = item.LastRevision,
                        FilePath = file.FullPath,
                    };

                    models.Add(model);
                });

                item.Models = models;
                workflow.Item = item;
            }

        }

        public async Task<WorkflowInstance?> UpdateWorkflowStep(int workflowInstanceId, int newStepId, int previousStepId)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql =
                @"UPDATE WorkflowInstances 
                SET CurrentStepId = @newStepId, PreviousStepId = @previousStepId 
                WHERE Id = @workflowInstanceId";

                await connection.ExecuteAsync(sql, new { newStepId, previousStepId, workflowInstanceId });

                return await connection.QueryFirstAsync<WorkflowInstance>(
                    "SELECT * FROM WorkflowInstances WHERE Id = @workflowInstanceId", new { workflowInstanceId });

            }
        }

        public async Task<WorkflowInstance?> GetWorkflowInstanceById(int workflowInstanceId)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = "SELECT * FROM WorkflowInstances WHERE Id = @workflowInstanceId";

                return await connection.QueryFirstOrDefaultAsync<WorkflowInstance>(sql, new { workflowInstanceId });
            }
        }

        public async Task<List<WorkflowInstance>> GetWorkflowInstanceByStepIds(List<int> stepIds )
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = "SELECT * FROM WorkflowInstances WHERE CurrentStepId IN @stepIdS";

                IEnumerable<WorkflowInstance> workflowInstances = await connection.QueryAsync<WorkflowInstance>(sql, new { stepIds });

                return workflowInstances.ToList();
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
    }


}
