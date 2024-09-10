using Application.Features.WorkflowInstances;
using Application.Features.WorkflowInstances.Commands.CreateWorkflowInstance;
using Application.Features.WorkflowInstances.Queries.GetWorkFlowIstanceSteps;
using Application.Features.WorkflowInstances.Queries.GetWorkflowsByUser;
using Domain.Results;
using MediatR;

namespace Api.Endpoints
{
    public static class WorkflowInstancesEndpoints
    {
        public static void AddWorkflowInstancesEndpoints(this WebApplication app)
        {
            app.MapPost("workflow-instances", async (ISender sender, CreateWorkflowInstanceCommand workflowData) =>
            {
                Result<WorkflowInstanceResponseDto> workflowResult = await sender.Send(workflowData);

                if (!workflowResult.IsSuccess)
                {
                    return Results.Problem(workflowResult.Error!.Message, null, workflowResult.Error.StatusCode);
                }

                return Results.Ok(workflowResult.Value);

            }).WithName("CreateWorkflowInstance");

            app.MapGet("users/{userId}/workflow-instances", async (ISender sender, int userId) =>
            {
                Result<List<WorkflowInstanceResponseDto>> workflowsResult = await sender.Send(new GetWorkflowsByUserQuery { UserId = userId });

                if (!workflowsResult.IsSuccess)
                {
                    return Results.Problem(workflowsResult.Error!.Message, null, workflowsResult.Error.StatusCode);
                }

                return Results.Ok(workflowsResult.Value);
            }).WithName("GetWorkflowsByUser");

            app.MapGet("workflow-instances/{workflowId}/steps", async (ISender sender, int workflowId) =>
            {
                List<WorkflowStepResponseDto> steps = await sender.Send(new GetWorkFlowIstanceStepsQuery { WorkflowInsanceId = workflowId });

                if (steps is null)
                {
                    Results.Conflict("Ocorreu um erro ao tentar carregar as etapas do workflow");
                }

                return Results.Ok(steps);
            }).WithName("GetWorkFlowIstanceSteps");

            app.MapPut("workflow-instances/{workflowId}/steps", (ISender sender, int workflowId ) =>
            {

            });




        }
    }
}