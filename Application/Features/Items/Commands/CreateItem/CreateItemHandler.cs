using Application.Abstractions.FileSystem;
using Application.Abstractions.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Results;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.Features.Items.Commands.CreateItem
{
    public class CreateItemHandler : IRequestHandler<CreateItemCommand, Result<ItemResponseDto?>>
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IItemRepository _itemRepository;
        private readonly IWorkflowInstanceRepository _workflowInstanceRepository;
        private readonly IItemFamilyRepository _itemFamilyRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IWorkflowInstanceValueRepository _workflowInstanceValueRepository;
        private readonly IItemAtributeValueRepository _itemAtributeValueRepository;
        private readonly IFileManagementService _fileManagementService;

        public CreateItemHandler(
            IMapper mapper,
            IConfiguration configuration,
            IItemRepository itemRepository,
            IWorkflowInstanceRepository workflowInstanceRepository,
            IItemFamilyRepository itemFamilyRepository,
            IModelRepository modelRepository,
            IWorkflowInstanceValueRepository workflowInstanceValueRepository,
            IItemAtributeValueRepository itemAtributeValueRepository,
            IFileManagementService fileManagementService
)
        {
            _mapper = mapper;
            _configuration = configuration;
            _itemRepository = itemRepository;
            _workflowInstanceRepository = workflowInstanceRepository;
            _itemFamilyRepository = itemFamilyRepository;
            _modelRepository = modelRepository;
            _workflowInstanceValueRepository = workflowInstanceValueRepository;
            _itemAtributeValueRepository = itemAtributeValueRepository;
            _fileManagementService = fileManagementService;
        }
        public async Task<Result<ItemResponseDto?>> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            string libraryBaseDir = _configuration.GetSection("BaseDirectories")["LibraryBaseDir"]!;
            Item? latestItem = null;

            WorkflowInstance? workflowInstance = await _workflowInstanceRepository.GetWorkflowInstanceById(request.WorkflowInstanceId, true);

            if (workflowInstance is null)
            {
                return Result<ItemResponseDto?>.Failure(new Error(400, "Workflow não encontrado"));
            }

            ItemFamily? itemFamily = await _itemFamilyRepository.GetItemFamilyById(workflowInstance.ItemFamilyId);

            if (itemFamily is null)
            {
                return Result<ItemResponseDto?>.Failure(new Error(400, "Familia não encontrado"));
            }

            if (workflowInstance.ItemRevision != "-")
            {
                latestItem = await _itemRepository.GetLatestItemByName(workflowInstance.ItemName, true);
            }

            Item newItemData = new()
            {
                Id = null,
                Name = workflowInstance.ItemName,
                Revision = workflowInstance.ItemRevision,
                Version = latestItem != null ? latestItem.Version + 1 : 1,
                Description = itemFamily.Description,
                Family = itemFamily.Name,
                Status = ItemStatus.released.ToString(),
                CreatedBy = workflowInstance.UserId,
                CreatedAt = DateTime.UtcNow,
                LastModifiedBy = workflowInstance.UserId,
                LastModifiedAt = DateTime.UtcNow,
                CheckedOutBy = 0,
                FamilyId = itemFamily.Id,
            };

            Item? newItem = await _itemRepository.CreateItem(newItemData);

            if (newItem is null)
            {
                return Result<ItemResponseDto?>.Failure(new Error(409, "Ocorreu um erro na criação do novo item."));
            }

            List<WorkflowInstanceValue> workflowInstanceValues = await _workflowInstanceValueRepository.GetWorkflowInstanceValues(workflowInstance.Id);

            foreach (WorkflowInstanceValue value in workflowInstanceValues)
            {
                 ItemAttributeValue? valueCreated = null;

                if (value.ItemAttributeValueString != null && value.ItemAttributeValueString != "")
                {
                    valueCreated = await _itemAtributeValueRepository.CreateItemAttributeValue(
                        (int)newItem.Id!, value.ItemAttributeId, value.ItemAttributeValueString);
                }
                else
                {
                    valueCreated = await _itemAtributeValueRepository.CreateItemAttributeValue(
                        (int)newItem.Id!, value.ItemAttributeId, value.ItemAttributeValueNumber);
                }

                if (valueCreated is null)
                {
                    return Result<ItemResponseDto?>.Failure(new Error(409, "Ocorreu um erro na criação de um atributo para o item."));
                }

            }

            foreach (Model previousModel in workflowInstance.Item!.Models)
            {
                string modelFilePath = $"{libraryBaseDir}/{newItemData.Family}\\{previousModel.Name}{previousModel.Type}";

                Model modelToCreate = new()
                {
                    Name = newItem.Name,
                    Type = previousModel.Type,
                    Version = newItemData.Version,
                    Revision = newItemData.Revision,
                    FilePath = modelFilePath,
                    ItemId = newItem.Id,
                    CreatedBy = newItemData.CreatedBy,
                    CreatedAt = newItemData.CreatedAt,
                    LastModifiedBy = newItemData.LastModifiedBy,
                    LastModifiedAt = newItemData.LastModifiedAt
    
                };

                Model? modelCreated = await _modelRepository.CreateModel(modelToCreate);

                if (modelCreated is null)
                {
                    return Result<ItemResponseDto?>.Failure(new Error(409, $"Ocorreu um erro na criação do modelo {previousModel?.Name}{previousModel?.Type}"));
                }

                _fileManagementService.MoveFile(previousModel.FilePath, $"{ modelCreated.FilePath}.{modelCreated.Version}");
            }

            return Result<ItemResponseDto?>.Success(_mapper.Map<ItemResponseDto>(newItem));

        }
    }
}