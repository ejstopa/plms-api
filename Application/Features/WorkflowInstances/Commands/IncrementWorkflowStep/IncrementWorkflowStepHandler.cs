using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Errors;
using Domain.Results;
using MediatR;

namespace Application.Features.WorkflowInstances.Commands.IncrementWorkflowStep
{
    public class IncrementWorkflowStepHandler : IRequestHandler<IncrementWorkflowStepCommand, Result<WorkflowInstanceResponseDto?>>
    {
        private readonly IMapper _mapper;
        private readonly IWorkflowInstanceRepository _workflowInstanceRepository;

        public IncrementWorkflowStepHandler(IMapper mapper, IWorkflowInstanceRepository workflowInstanceRepository)
        {
            _mapper = mapper;
            _workflowInstanceRepository = workflowInstanceRepository;
        }
        public async Task<Result<WorkflowInstanceResponseDto?>> Handle(IncrementWorkflowStepCommand request, CancellationToken cancellationToken)
        {
            WorkflowInstance? workflow = await _workflowInstanceRepository.GetWorkflowInstanceById(request.WorkflowInstanceId);

            if (workflow is null)
            {
                return Result<WorkflowInstanceResponseDto?>.Failure(new Error(404, "Workflow não encontrado"));
            }

            List<WorkFlowStep>? workFlowSteps = await _workflowInstanceRepository.GetWorkflowInstancSteps(request.WorkflowInstanceId);

            if (workFlowSteps is null || workFlowSteps.Count == 0)
            {
                return Result<WorkflowInstanceResponseDto?>.Failure(new Error(409, "Ocorreu um erro encontrar as etapas do workflow"));
            }

            List<WorkFlowStep>? workFlowStepsOrdered = workFlowSteps.OrderBy(step => step.StepOrder).ToList();

            int stepIndex = 0;

            try
            {
                stepIndex = workFlowStepsOrdered.IndexOf(workFlowStepsOrdered.FirstOrDefault(step => step.Id == workflow.CurrentStepId)!);
            }
            catch
            {
                return Result<WorkflowInstanceResponseDto?>.Failure(new Error(409, "Ocorreu um erro ao finalizar a etapa do workflow"));
            }

            WorkFlowStep nextStep = stepIndex < workFlowStepsOrdered.Count -1 ?
                workFlowStepsOrdered[stepIndex + 1] :
                 workFlowStepsOrdered[stepIndex];

            WorkFlowStep previousStep = workFlowStepsOrdered[stepIndex];


            WorkflowInstance? workflowUpdated = await _workflowInstanceRepository.UpdateWorkflowStep(request.WorkflowInstanceId, nextStep.Id, previousStep.Id);

            if (workflowUpdated is null)
            {
                return Result<WorkflowInstanceResponseDto?>.Failure(new Error(409, "Ocorreu um erro ao finalizar a etapa do workflow"));
            }

            return Result<WorkflowInstanceResponseDto?>.Success(_mapper.Map<WorkflowInstanceResponseDto>(workflowUpdated));
        }
    }
}