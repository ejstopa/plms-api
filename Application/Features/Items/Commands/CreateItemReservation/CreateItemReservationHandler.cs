using Application.Abstractions.FileSystem;
using Application.Abstractions.Repositories;
using Application.Features.models;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Results;
using MediatR;

namespace Application.Features.Items.Commands.CreateItemReservation
{
    public class CreateItemReservationHandler : IRequestHandler<CreateItemReservationCommand, Result<List<ModelResponseDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IFileRevisionService _fileRevisionService;
        
        public CreateItemReservationHandler(IUserRepository userRepository, IItemRepository itemRepository, IModelRepository modelRepository, IFileRevisionService fileRevisionService, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _itemRepository = itemRepository;
            _modelRepository = modelRepository;
            _fileRevisionService = fileRevisionService;
        }

        public async Task<Result<List<ModelResponseDto>>> Handle(CreateItemReservationCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetUserById(request.UserId);
            if (user is null) return Result<List<ModelResponseDto>>.Failure(new Error(400, "O usuário não foi encontrado"));
         
            Item? item = await _itemRepository.GetItemById(request.ItemId);
            if (item is null) return Result<List<ModelResponseDto>>.Failure(new Error(400, "O item não foi encontrado"));
            
            if (item.CheckedOutBy != 0 && item.CheckedOutBy != null)
            {
                User? userReserved = await _userRepository.GetUserById((int)item.CheckedOutBy);

                return userReserved != null ?
                 Result<List<ModelResponseDto>>.Failure(new Error(401, $"O item já está reservado para o usuário {userReserved.Name}")) :
                 Result<List<ModelResponseDto>>.Failure(new Error(401, "O item já está reservado"));
            }
            
            if (item.Status == ItemStatus.inWorkflow.ToString()) return Result<List<ModelResponseDto>>.Failure(new Error(401, "O item está em fluxo de liberação e não pode ser reservado"));

            List<Model> models = await _modelRepository.GetLatestModelsByItem(request.ItemId);
            if (models.Count == 0) return Result<List<ModelResponseDto>>.Failure(new Error(409, "Não existem arquivos relacionados a esse item"));

            bool itemCheckedOut = await _itemRepository.ToggleItemCheckout(request.ItemId, request.UserId, true);
            if (!itemCheckedOut) return Result<List<ModelResponseDto>>.Failure(new Error(409, "Ocorreu um erro ao tentar reservar o item"));

            try
            {
                foreach (Model model in models)
                {
                    _fileRevisionService.CreateFileRevision($"{model.FilePath}.{model.Version}", user);
                }
            }
            catch
            {
                return Result<List<ModelResponseDto>>.Failure(new Error(409, "Ocorreu um erro ao tentar copiar os arquivos para a workspace"));
            }

            List<ModelResponseDto> modelResponseDtos = _mapper.Map<List<ModelResponseDto>>(models);

            return Result<List<ModelResponseDto>>.Success(modelResponseDtos);
        }
    }
}