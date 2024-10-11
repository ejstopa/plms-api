using System.ComponentModel.DataAnnotations;
using Application.Abstractions.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Items.Queries.GetItemsByDynamicParams
{
    public class GetItemsByDynamicParamsHandler : IRequestHandler<GetItemsByDynamicParamsQuery, List<ItemResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IItemRepository _itemRepository;

        public GetItemsByDynamicParamsHandler(IMapper mapper, IItemRepository itemRepository)
        {
            _mapper = mapper;
            _itemRepository = itemRepository;
        }
        
        public async Task<List<ItemResponseDto>> Handle(GetItemsByDynamicParamsQuery request, CancellationToken cancellationToken)
        {
            List<Item> items = await _itemRepository.GetItemsByDynamicParams(request.ItemFamilyId, request.SearchParams);

            return _mapper.Map<List<ItemResponseDto>>(items);
        }

        
    }
}