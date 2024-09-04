using Application.Abstractions.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Items.GetItemByIdQuery.Queries
{
    public class GetItemByIdHandler : IRequestHandler<GetItemByIdQuery, ItemResponseDto>
    {
        private readonly IMapper _mapper;
        private readonly IItemRepository _itemRepository;

        public GetItemByIdHandler(IMapper mapper, IItemRepository itemRepository)
        {
            _mapper = mapper;
            _itemRepository = itemRepository;
        }

        public async Task<ItemResponseDto> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
        {
            Item? item = await _itemRepository.GetItemById(request.Id);

            return _mapper.Map<ItemResponseDto>(item);
        }
    }
}