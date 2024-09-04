using Application.Abstractions.FileSystem;
using Application.Abstractions.Repositories;
using Application.Features.models;
using AutoMapper;
using Domain.Entities;
using Domain.Errors;
using Domain.Results;
using MediatR;

namespace Application.Features.Items.Commands.DeleteItemReservation
{
    public class DeleteItemReservationHandler : IRequestHandler<DeleteItemReservationCommand, Result<List<ModelResponseDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IFileRevisionService _fileRevisionService;
        
        public DeleteItemReservationHandler(IUserRepository userRepository, IItemRepository itemRepository, IModelRepository modelRepository, IFileRevisionService fileRevisionService, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _itemRepository = itemRepository;
            _modelRepository = modelRepository;
            _fileRevisionService = fileRevisionService;
        }

        public async Task<Result<List<ModelResponseDto>>> Handle(DeleteItemReservationCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetUserById(request.UserId);
            if (user is null) return Result<List<ModelResponseDto>>.Failure(new Error(400, "O usuário não foi encontrado"));

            Item? item = await _itemRepository.GetItemById(request.ItemId);
            if (item is null) return Result<List<ModelResponseDto>>.Failure(new Error(400, "O item não foi encontrado"));
            if (item.CheckedOutBy == 0) return Result<List<ModelResponseDto>>.Failure(new Error(400, "O item não está reservado"));
            if (item.CheckedOutBy != request.UserId) return Result<List<ModelResponseDto>>.Failure(new Error(401, "O item não está reservado para este usuário"));

            List<Model> models = await _modelRepository.GetLatestModelsByItem(request.ItemId);
            if (models.Count == 0) return Result<List<ModelResponseDto>>.Failure(new Error(409, "Não existem arquivos relacionados a esse item"));

            bool itemCheckedOut = await _itemRepository.ToggleItemCheckout(request.ItemId, request.UserId, false);
            if (!itemCheckedOut) return Result<List<ModelResponseDto>>.Failure(new Error(409, "Ocorreu um erro ao tentar excluir a reserva do item o item"));

            try
            {
                foreach (Model model in models)
                {
                    _fileRevisionService.DeleteFileRevision($"{model.Name}{model.Type}", user);
                }
            }
            catch
            {
                return Result<List<ModelResponseDto>>.Failure(new Error(409, "Ocorreu um erro ao tentar excluir os arquivos  da workspace"));
            }

            List<ModelResponseDto> modelResponseDtos = _mapper.Map<List<ModelResponseDto>>(models);

            return Result<List<ModelResponseDto>>.Success(modelResponseDtos);
        }
    }
}