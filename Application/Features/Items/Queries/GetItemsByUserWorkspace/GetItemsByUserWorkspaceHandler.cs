using Application.Abstractions.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Errors;
using Domain.Results;
using MediatR;

namespace Application.Features.Items.Queries.GetItemsByUserWorkspace
{
    public class GetItemsByUserWorkspaceHandler : IRequestHandler<GetItemsByUserWorkspaceCommand, Result<List<ItemResponseDto>>>
    {
         private readonly IMapper _mapper;
        private readonly IItemRepository _itemRepository;
        private readonly IUserRepository _userRepository;
       
        public GetItemsByUserWorkspaceHandler(IMapper mapper, IItemRepository itemRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _itemRepository = itemRepository;
            _userRepository = userRepository;
        }

        public async Task<Result<List<ItemResponseDto>>> Handle(GetItemsByUserWorkspaceCommand request, CancellationToken cancellationToken)
        {
            User? user = await  _userRepository.GetUserById(request.UserId);

            if (user is null){
                return Result<List<ItemResponseDto>>.Failure(new Error(400, "Usuário não encontrado"));
            }
            
            
            List<Item> items = await _itemRepository.GetItemsByUserWorkspace(user);

            return Result<List<ItemResponseDto>>.Success(_mapper.Map<List<ItemResponseDto>>(items));
        }
    }
}