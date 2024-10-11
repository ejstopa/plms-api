using System.Data.SqlTypes;
using Application.Abstractions.FileSystem;
using Application.Abstractions.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Results;
using Domain.Services;
using MediatR;

namespace Application.Features.WorkflowInstances.Commands.CreateWorkflowInstance
{
    public class CreateWorkflowInstanceHandler : IRequestHandler<CreateWorkflowInstanceCommand, Result<WorkflowInstanceResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly ICreateWorkflowInstanceValidator _validator;
        private readonly IUserRepository _userRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IWorkflowInstanceRepository _workflowInstanceRepository;
        private readonly IModelRevisionService _modelRevisionService;
        private readonly IUserWorkspaceFIlesService _userWorkspaceFIlesService;
        private readonly IUserWorkflowFilesService _userWorkflowFilesService;
        private readonly IItemFamilyRepository _itemFamilyRepository;
        private readonly IWorkflowTemplateRepository _workflowTemplateRepository;

        public CreateWorkflowInstanceHandler(
            IMapper mapper,
            ICreateWorkflowInstanceValidator validator,
            IUserRepository userRepository,
            IItemRepository itemRepository,
            IWorkflowInstanceRepository workflowInstanceRepository,
            IModelRevisionService modelRevisionService,
            IUserWorkspaceFIlesService userWorkspaceFIlesService,
            IUserWorkflowFilesService userWorkflowFilesService, 
            IItemFamilyRepository itemFamilyRepository, 
            IWorkflowTemplateRepository workflowTemplateRepository)
        {
            _mapper = mapper;
            _validator = validator;
            _userRepository = userRepository;
            _itemRepository = itemRepository;
            _workflowInstanceRepository = workflowInstanceRepository;
            _modelRevisionService = modelRevisionService;
            _userWorkspaceFIlesService = userWorkspaceFIlesService;
            _userWorkflowFilesService = userWorkflowFilesService;
            _itemFamilyRepository = itemFamilyRepository;
            _workflowTemplateRepository = workflowTemplateRepository;
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
            
            List<WorkFlowStep> workFlowSteps = await _workflowTemplateRepository.GetWorkflowTemplateSteps(workflowTemplateId);

            Item? item = await _itemRepository.GetLatestItemByName(request.ItemName);

            if (item != null && item.Id != null)
            {
                Item? itemUpdated = await _itemRepository.SetItemStatus((int)item.Id, ItemStatus.inWorkflow);
            }

            ItemFamily? itemFamily = await _itemFamilyRepository.GetItemFamilyByName(request.ItemName[..4]);

            WorkflowInstance workflowInstance = new()
            {
                Id = 0,
                WorkflowTemplateId = workflowTemplateId,
                ItemId = item != null ? item.Id : null,
                ItemName = request.ItemName,
                ItemRevision = item != null ? _modelRevisionService.IncrementRevision(item.Revision) : "-",
                UserId = request.UserId,
                CurrentStepId = workFlowSteps[0].Id,
                PreviousStepId = null,
                Status = WorkflowStatus.InWork.ToString(),
                Message = "",
                ItemFamilyId = itemFamily!.Id
            };

            User? user = await _userRepository.GetUserById(request.UserId);

            if (user is null)
            {
                return Result<WorkflowInstanceResponseDto>.Failure(new Error(404, "Usário não encontrado"));
            }

            List<UserFile> workspaceFiles = _userWorkspaceFIlesService.GetUserUserWorkspaceFiles(user, [".prt", ".asm", ".drw", ".JPG"]);

            List<UserFile> itemFiles = workspaceFiles.Where(file => file.Name.StartsWith(request.ItemName)).ToList();
            itemFiles.ForEach(file => _userWorkflowFilesService.MoveFileToWorkflowsDirectory(file.FullPath, user));

            WorkflowInstance workflowCreated = await _workflowInstanceRepository.CreateWorkflowInstance(workflowInstance);
            WorkflowInstanceResponseDto workflowInstanceResponseDto = _mapper.Map<WorkflowInstanceResponseDto>(workflowCreated);

            return await Task.FromResult(Result<WorkflowInstanceResponseDto>.Success(workflowInstanceResponseDto));
        }


    }
}