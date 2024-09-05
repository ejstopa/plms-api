using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.WorkflowInstances;
using Application.Features.WorkflowInstances.CreateWorkflowInstance;
using Domain.Results;
using MediatR;

namespace Api.Endpoints
{
    public static class WorkflowInstancesEndpoints
    {
        public static void AddWorkflowInstancesEndpoints(this WebApplication app)
        {
            var baseUrl = app.MapGroup("workflow-instances");

            baseUrl.MapPost("", async (ISender sender, CreateWorkflowInstanceCommand workflowData) =>
            {
                Result<WorkflowInstanceResponseDto> workflowResult = await sender.Send(workflowData);

                if (!workflowResult.IsSuccess)
                {
                    return Results.Problem(workflowResult.Error!.Message, null, workflowResult.Error.StatusCode);
                }

                return Results.Ok(workflowResult.Value);

            }).WithName("CreateWorkflowInstance");
        }
    }
}