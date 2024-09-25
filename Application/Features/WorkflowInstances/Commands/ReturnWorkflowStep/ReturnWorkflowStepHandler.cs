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

namespace Application.Features.WorkflowInstances.Commands.ReturnWorkflowStep
{
    public class ReturnWorkflowStepHandler : IRequestHandler<ReturnWorkflowStepCommand, Result<bool>>
    {
        private readonly IMapper _mapper;
        private readonly IWorkflowInstanceRepository _workflowInstanceRepository;

        public ReturnWorkflowStepHandler(IMapper mapper, IWorkflowInstanceRepository workflowInstanceRepository)
        {
            _mapper = mapper;
            _workflowInstanceRepository = workflowInstanceRepository;
        }

        public async Task<Result<bool>> Handle(ReturnWorkflowStepCommand request, CancellationToken cancellationToken)
        {
            WorkflowInstance? workflow = await _workflowInstanceRepository.GetWorkflowInstanceById(request.WorkflowInstanceId);

            if (workflow is null)
            {
                return Result<bool>.Failure(new Error(404, "Workflow não encontrado"));
            }

            WorkflowInstance? workflowUpdated = await _workflowInstanceRepository.UpdateWorkflowStep(
                request.WorkflowInstanceId, request.NewStepId, workflow.CurrentStepId, request.Message );
            
            if (workflowUpdated is null)
            {
                return Result<bool>.Failure(new Error(409, "Ocorreu um erro ao tentar atualizar o workflow"));
            }

            return Result<bool>.Success(true);
        }
    }
}