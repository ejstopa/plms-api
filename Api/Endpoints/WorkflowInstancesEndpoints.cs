using Application.Features.WorkflowInstances;
using Application.Features.WorkflowInstances.Commands.CreateWorkflowInstance;
using Application.Features.WorkflowInstances.Commands.CreateWorkflowInstanceValue;
using Application.Features.WorkflowInstances.Commands.IncrementWorkflowStep;
using Application.Features.WorkflowInstances.Queries.GetWorkflowInstanceValue;
using Application.Features.WorkflowInstances.Queries.GetWorkFlowIstanceSteps;
using Application.Features.WorkflowInstances.Queries.GetWorkflowsByUser;
using Application.Features.WorkflowInstances.Queries.GetWorkflowInstancesByDepartment;
using Domain.Entities;
using Domain.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

            app.MapGet("departments/{departmentId}/workflow-Tasks", async (ISender sender, int departmentId) =>
            {
                Result<List<WorkflowInstanceResponseDto>> workflowsResult = await sender.Send(new GetWorkflowInstancesByDepartmentQuery { DepartmentId = departmentId });

                if (!workflowsResult.IsSuccess)
                {
                    return Results.Problem(workflowsResult.Error!.Message, null, workflowsResult.Error.StatusCode);
                }

                return Results.Ok(workflowsResult.Value);
            }).WithName("GetWorkflowInstancesByDepartment");


            app.MapGet("workflow-instances/{workflowId}/steps", async (ISender sender, int workflowId) =>
            {
                List<WorkflowStepResponseDto> steps = await sender.Send(new GetWorkFlowIstanceStepsQuery { WorkflowInsanceId = workflowId });

                if (steps is null)
                {
                    Results.Conflict("Ocorreu um erro ao tentar carregar as etapas do workflow");
                }

                return Results.Ok(steps);
            }).WithName("GetWorkFlowIstanceSteps");

            app.MapPost("workflow-instances/{id}/attibute-values", async (ISender sender, int id, CreateWorkflowInstanceValueCommand CreateWorkflowInstanceValueCommand) =>
            {
                Result<WorkflowInstanceValue?> result = await sender.Send(CreateWorkflowInstanceValueCommand);

                if (!result.IsSuccess)
                {
                    return Results.Problem(result.Error!.Message, null, result!.Error.StatusCode);
                }

                return Results.Ok(result.Value);

            }).WithName("CreateWorkflowInstanceValue");

            app.MapGet("workflow-instances/{id}/attibute-values", async (ISender sender, int id) =>
            {
                Result<List<WorkflowInstanceValue>> result = await sender.Send(new GetWorkflowInstanceValueQuery { WorkflowInstanceId = id });

                if (!result.IsSuccess)
                {
                    return Results.Problem(result.Error!.Message, null, result!.Error.StatusCode);
                }

                return Results.Ok(result.Value);

            }).WithName("GetWorkflowInstanceValue");

            app.MapPut("workflow-instances/{workflowId}", async (ISender sender, int workflowId, [FromQuery] bool incrementStep, IncrementWorkflowStepCommand incrementWorkflowStepCommand) =>
            {
                if (incrementStep)
                {
                    Result<WorkflowInstanceResponseDto?> result = await sender.Send(incrementWorkflowStepCommand);

                    if (!result.IsSuccess)
                    {
                        return Results.Problem(result.Error!.Message, null, result!.Error.StatusCode);
                    }

                    return Results.Ok(result.Value);
                }

                return Results.NoContent();

            }).WithName("UpdateWorkflowInstance");






        }
    }
}