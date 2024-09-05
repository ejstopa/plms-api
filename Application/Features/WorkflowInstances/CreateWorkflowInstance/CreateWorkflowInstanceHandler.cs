using System.Data.SqlTypes;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Errors;
using Domain.Results;
using MediatR;

namespace Application.Features.WorkflowInstances.CreateWorkflowInstance
{
    public class CreateWorkflowInstanceHandler : IRequestHandler<CreateWorkflowInstanceCommand, Result<WorkflowInstanceResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICreateWorkflowInstanceValidator _validator;

        public CreateWorkflowInstanceHandler(IUserRepository userRepository, ICreateWorkflowInstanceValidator validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }
        public async Task<Result<WorkflowInstanceResponseDto>> Handle(CreateWorkflowInstanceCommand request, CancellationToken cancellationToken)
        {
            Result<bool> validationResult = await _validator.Validate(request);

            if (!validationResult.IsSuccess)
            {
                return await Task.FromResult(Result<WorkflowInstanceResponseDto>.Failure(new Error(
                    validationResult.Error!.StatusCode, validationResult.Error!.Message)));
            }

            return await Task.FromResult(Result<WorkflowInstanceResponseDto>.Failure(new Error(200, "workflow iniciado com successo")));
        }


    }
}