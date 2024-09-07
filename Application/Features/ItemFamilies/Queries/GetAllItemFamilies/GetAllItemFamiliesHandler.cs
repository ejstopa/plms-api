using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.ItemFamilies.Queries.GetAllItemFamilies
{
    public class GetAllItemFamiliesHandler : IRequestHandler<GetAllItemFamiliesCommand, List<ItemFamilyResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IItemFamilyRepository _itemFamilyRepository;

        public GetAllItemFamiliesHandler(IMapper mapper, IItemFamilyRepository itemFamilyRepository)
        {
            _mapper = mapper;
            _itemFamilyRepository = itemFamilyRepository;
        }
        public async Task<List<ItemFamilyResponseDto>> Handle(GetAllItemFamiliesCommand request, CancellationToken cancellationToken)
        {
            List<ItemFamily> itemFamilies = await _itemFamilyRepository.GetAllItemFamilies();

            return _mapper.Map<List<ItemFamilyResponseDto>>(itemFamilies);
        }
    }
}