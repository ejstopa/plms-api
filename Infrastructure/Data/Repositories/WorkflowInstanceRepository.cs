using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.FileSystem;
using Application.Abstractions.Repositories;
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
                    @Message);
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
    }


}
