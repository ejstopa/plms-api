using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Results;
using MediatR;

namespace Application.Features.WorkflowInstances.Commands.IncrementWorkflowStep
{
    public class IncrementWorkflowStepHandler : IRequestHandler<IncrementWorkflowStepCommand, Result<WorkflowInstanceResponseDto?>>
    {
        private readonly IMapper _mapper;
        private readonly IWorkflowInstanceRepository _workflowInstanceRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IWorkflowStepForkRepository _workflowStepForkRepository;
        private readonly IWorkflowInstanceValueRepository _workflowInstanceValueRepository;

        public IncrementWorkflowStepHandler(IMapper mapper,
        IWorkflowInstanceRepository workflowInstanceRepository,
        IItemRepository itemRepository,
        IWorkflowStepForkRepository workflowStepForkRepository,
        IWorkflowInstanceValueRepository workflowInstanceValueRepository)
        {
            _mapper = mapper;
            _workflowInstanceRepository = workflowInstanceRepository;
            _itemRepository = itemRepository;
            _workflowStepForkRepository = workflowStepForkRepository;
            _workflowInstanceValueRepository = workflowInstanceValueRepository;
        }
        public async Task<Result<WorkflowInstanceResponseDto?>> Handle(IncrementWorkflowStepCommand request, CancellationToken cancellationToken)
        {
            WorkflowInstance? workflow = await _workflowInstanceRepository.GetWorkflowInstanceById(request.WorkflowInstanceId);

            if (workflow is null)
            {
                return Result<WorkflowInstanceResponseDto?>.Failure(new Error(404, "Workflow não encontrado"));
            }

            List<WorkFlowStep>? workFlowSteps = await _workflowInstanceRepository.GetWorkflowInstancSteps(request.WorkflowInstanceId, workflow.ItemFamilyId);

            if (workFlowSteps is null || workFlowSteps.Count == 0)
            {
                return Result<WorkflowInstanceResponseDto?>.Failure(new Error(409, "Ocorreu um erro encontrar as etapas do workflow"));
            }

            int currentStepIndex = workFlowSteps.IndexOf(workFlowSteps.FirstOrDefault(step => step.Id == workflow.CurrentStepId)!);

            if (currentStepIndex < workFlowSteps.Count - 1)
            {
                return await FinalizeWorkflowStep(workflow, workFlowSteps, currentStepIndex);
            }
            else
            {
                if (workflow.ItemId != null)
                {
                    bool itemUpdated = await UpdateItem((int)workflow.ItemId!, workflow.UserId);

                    if (!itemUpdated)
                    {
                        return Result<WorkflowInstanceResponseDto?>.Failure(new Error(409, "Ocorreu atualizar o status do item do workflow"));
                    }
                }

                return await FinalizeWorkflowLastStep(workflow);
            }

        }

        private async Task<Result<WorkflowInstanceResponseDto?>> FinalizeWorkflowStep(
            WorkflowInstance workflow, List<WorkFlowStep> workFlowSteps, int currentStepIndex)
        {
            WorkflowInstance? workflowUpdated = null;
            WorkFlowStep? nextStep = null;
            List<WorkflowStepFork> stepForks = await _workflowStepForkRepository.GetStepForkByWorkflowAndStep(workflow.WorkflowTemplateId, workflow.CurrentStepId);

            if (stepForks.Count > 0)
            {
                WorkflowStepFork? stepForkWithoutCondition = stepForks.FirstOrDefault(step => step.DecisionAttributeId == null);

                if (stepForkWithoutCondition != null)
                {
                    nextStep = workFlowSteps.FirstOrDefault(step => step.Id == stepForkWithoutCondition.NextStepId);
                }
                else
                {
                    List<WorkflowInstanceValue> workflowValues = await _workflowInstanceValueRepository.GetWorkflowInstanceValues(workflow.Id);

                    foreach (WorkflowStepFork stepFork in stepForks)
                    {
                        WorkflowInstanceValue? decisionValue = workflowValues.FirstOrDefault(value =>
                        value.ItemAttributeId == stepFork.DecisionAttributeId &&
                        (value.ItemAttributeValueNumber.ToString() == stepFork.DecisionAttributeValue ||
                        value.ItemAttributeValueString == stepFork.DecisionAttributeValue));

                        if (decisionValue != null)
                        {
                            nextStep = workFlowSteps.FirstOrDefault(step => step.Id == stepFork.NextStepId);
                        }
                    }

                }
            }

            if (nextStep is null)
            {
                nextStep = workFlowSteps[currentStepIndex + 1];
            }

            WorkFlowStep previousStep = workFlowSteps[currentStepIndex];

            workflowUpdated = await _workflowInstanceRepository.UpdateWorkflowStep(workflow.Id, nextStep.Id, previousStep.Id);

            if (workflowUpdated is null)
            {
                return Result<WorkflowInstanceResponseDto?>.Failure(new Error(409, "Ocorreu um erro ao finalizar a etapa do workflow"));
            }

            return Result<WorkflowInstanceResponseDto?>.Success(_mapper.Map<WorkflowInstanceResponseDto>(workflowUpdated));
        }

        private async Task<Result<WorkflowInstanceResponseDto?>> FinalizeWorkflowLastStep(WorkflowInstance workflow)
        {
            WorkflowInstance? workflowUpdated = await _workflowInstanceRepository.UpdateWorkflowStep(
                workflow.Id, null, null);

            WorkflowInstance? workflowFinished = await _workflowInstanceRepository.SetWorkflowInstanceStatus(
                workflow.Id, WorkflowStatus.released);

            if (workflowUpdated is null || workflowFinished is null)
            {
                return Result<WorkflowInstanceResponseDto?>.Failure(new Error(409, "Ocorreu um erro ao finalizar a etapa do workflow"));
            }

            return Result<WorkflowInstanceResponseDto?>.Success(_mapper.Map<WorkflowInstanceResponseDto>(workflowFinished));

        }

        private async Task<bool> UpdateItem(int itemId, int userId)
        {
            Item? itemUpdated = await _itemRepository.SetItemStatus(itemId, ItemStatus.released);
            bool itemUncheckedOut = await _itemRepository.ToggleItemCheckout(itemId, userId, false);

            if (itemUpdated is null || itemUncheckedOut == false)
            {
                return false;
            }

            return true;
        }

    }
}