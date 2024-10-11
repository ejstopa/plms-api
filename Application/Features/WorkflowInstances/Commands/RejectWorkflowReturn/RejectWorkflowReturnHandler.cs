using Application.Abstractions.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Errors;
using Domain.Results;
using MediatR;

namespace Application.Features.WorkflowInstances.Commands.RejectWorkflowReturn
{
    public class RejectWorkflowReturnHandler : IRequestHandler<RejectWorkflowReturnCommand, Result<WorkflowInstanceResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IWorkflowInstanceRepository _workflowInstanceRepository;

        public RejectWorkflowReturnHandler(IMapper mapper, IWorkflowInstanceRepository workflowInstanceRepository)
        {
            _mapper = mapper;
            _workflowInstanceRepository = workflowInstanceRepository;
        }

        public async Task<Result<WorkflowInstanceResponseDto>> Handle(RejectWorkflowReturnCommand request, CancellationToken cancellationToken)
        {
            WorkflowInstance? workflow = await _workflowInstanceRepository.GetWorkflowInstanceById(request.WorkflowInstanceId);

            if (workflow is null)
            {
                return Result<WorkflowInstanceResponseDto>.Failure(new Error(400, "Workflow não encontrado"));
            }

            WorkflowInstance? workflowUpdated = await _workflowInstanceRepository.UpdateWorkflowStep(
                workflow.Id, workflow.PreviousStepId, workflow.CurrentStepId, request.Message, null
            );

            if (workflowUpdated is null)
            {
                return Result<WorkflowInstanceResponseDto>.Failure(new Error(409, "Ocorreu um erro ao tentar atualizar o workflow"));
            }

            return Result<WorkflowInstanceResponseDto>.Success(_mapper.Map<WorkflowInstanceResponseDto>(workflowUpdated));
        }
    }
}