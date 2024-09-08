using System.Data.SqlTypes;
using Application.Abstractions.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Results;
using Domain.Services;
using MediatR;

namespace Application.Features.WorkflowInstances.CreateWorkflowInstance
{
    public class CreateWorkflowInstanceHandler : IRequestHandler<CreateWorkflowInstanceCommand, Result<WorkflowInstanceResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly ICreateWorkflowInstanceValidator _validator;
        private readonly IUserRepository _userRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IWorkflowInstanceRepository _workflowInstanceRepository;
        private readonly IModelRevisionService _modelRevisionService;

        public CreateWorkflowInstanceHandler(
            IMapper mapper,
            ICreateWorkflowInstanceValidator validator, 
            IUserRepository userRepository, 
            IItemRepository itemRepository, 
            IWorkflowInstanceRepository workflowInstanceRepository, 
            IModelRevisionService modelRevisionService)
        {
            _mapper = mapper;
            _validator = validator;
            _userRepository = userRepository;
            _itemRepository = itemRepository;
            _workflowInstanceRepository = workflowInstanceRepository;
            _modelRevisionService = modelRevisionService;

        }
        public async Task<Result<WorkflowInstanceResponseDto>> Handle(CreateWorkflowInstanceCommand request, CancellationToken cancellationToken)
        {
            Result<bool> validationResult = await _validator.Validate(request);

            if (!validationResult.IsSuccess)
            {
                return await Task.FromResult(Result<WorkflowInstanceResponseDto>.Failure(new Error(
                    validationResult.Error!.StatusCode, validationResult.Error!.Message)));
            }

            int workflowTemplateId = 2;

            Item? item = await _itemRepository.GetItemByName(request.ItemName);

            WorkflowInstance workflowInstance = new()
            {
                Id = 0,
                WorkflowTemplateId = workflowTemplateId,
                ItemId = item != null ? item.Id : null,
                ItemName = request.ItemName,
                ItemRevision = item != null? _modelRevisionService.IncrementRevision(item.LastRevision) : "-",
                UserId = request.UserId,
                CurrentStepId = 1,
                PreviousStepId = null,
                Status = WorkflowStatus.InWork.ToString(),
                Message = ""
            };

            WorkflowInstance workflowCreated = await _workflowInstanceRepository.CreateWorkflowInstance(workflowInstance);
            WorkflowInstanceResponseDto workflowInstanceResponseDto = _mapper.Map<WorkflowInstanceResponseDto>(workflowCreated);

            return await Task.FromResult(Result<WorkflowInstanceResponseDto>.Success(workflowInstanceResponseDto));
        }


    }
}