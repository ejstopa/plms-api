using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Items.Queries.GetItemsByFamily
{
    public class GetItemsByFamilyHandler : IRequestHandler<GetItemsByFamilyCommand, List<ItemResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IItemRepository _itemRepository;

        public GetItemsByFamilyHandler(IMapper mapper, IItemRepository itemRepository)
        {
            _mapper = mapper;
            _itemRepository = itemRepository;
        }
        public async Task<List<ItemResponseDto>> Handle(GetItemsByFamilyCommand request, CancellationToken cancellationToken)
        {
            List<Item> items = await _itemRepository.GetItemsByFamily(request.Family);

            return _mapper.Map<List<ItemResponseDto>>(items);
        }
    }
}