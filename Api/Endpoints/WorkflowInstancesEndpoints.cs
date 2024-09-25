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
using Application.Features.WorkflowInstances.Commands.DeleteWorkflowInstance;
using Application.Features.WorkflowInstances.Commands.ReturnWorkflowStep;

namespace Api.Endpoints
{
    public static class WorkflowInstancesEndpoints
    {
        public static void AddWorkflowInstancesEndpoints(this WebApplication app)
        {
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

            app.MapGet("workflow-instances/{id}/attibute-values", async (ISender sender, int id) =>
            {
                Result<List<WorkflowInstanceValue>> result = await sender.Send(new GetWorkflowInstanceValueQuery { WorkflowInstanceId = id });

                if (!result.IsSuccess)
                {
                    return Results.Problem(result.Error!.Message, null, result!.Error.StatusCode);
                }

                return Results.Ok(result.Value);

            }).WithName("GetWorkflowInstanceValue");

            app.MapPost("workflow-instances", async (ISender sender, CreateWorkflowInstanceCommand workflowData) =>
            {
                Result<WorkflowInstanceResponseDto> workflowResult = await sender.Send(workflowData);

                if (!workflowResult.IsSuccess)
                {
                    return Results.Problem(workflowResult.Error!.Message, null, workflowResult.Error.StatusCode);
                }

                return Results.Ok(workflowResult.Value);

            }).WithName("CreateWorkflowInstance");

            app.MapPost("workflow-instances/{id}/attibute-values", async (ISender sender, int id, CreateWorkflowInstanceValueCommand CreateWorkflowInstanceValueCommand) =>
            {
                Result<WorkflowInstanceValue?> result = await sender.Send(CreateWorkflowInstanceValueCommand);

                if (!result.IsSuccess)
                {
                    return Results.Problem(result.Error!.Message, null, result!.Error.StatusCode);
                }

                return Results.Ok(result.Value);

            }).WithName("CreateWorkflowInstanceValue");

            app.MapPost("workflow-instances/{workflowId}/step-completions", async (ISender sender, int workflowId) =>
            {
                Result<WorkflowInstanceResponseDto?> result = await sender.Send(
                    new IncrementWorkflowStepCommand { WorkflowInstanceId = workflowId });

                if (!result.IsSuccess)
                {
                    return Results.Problem(result.Error!.Message, null, result!.Error.StatusCode);
                }

                return Results.Ok(result.Value);

            }).WithName("IncrementWorkflowInstanceStep");

            app.MapPost("workflow-instances/{workflowId}/step-returns", async (
                ISender sender, int workflowId, ReturnWorkflowStepCommand returnWorkflowStepCommand) =>
            {
                Result<bool> result = await sender.Send(returnWorkflowStepCommand);

                if (!result.IsSuccess)
                {
                    return Results.Problem(result.Error!.Message, null, result!.Error.StatusCode);
                }

                return Results.Ok();

            }).WithName("ReturnWorkflowStep");

            app.MapDelete("workflow-instances/{id}", async (ISender sender, int id) =>
            {
                Result<bool> result = await sender.Send(new DeleteWorkflowInstanceCommand { WorkflowInstanceId = id });

                if (!result.IsSuccess)
                {
                    return Results.Problem(result.Error!.Message, null, result.Error.StatusCode);
                }

                return Results.NoContent();

            }).WithName("DeleteWorkflowInstance");






        }
    }
}