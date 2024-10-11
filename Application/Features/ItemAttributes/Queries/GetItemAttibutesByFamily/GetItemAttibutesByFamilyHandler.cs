using Application.Abstractions.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.ItemAttributes.Queries.GetItemAttibutesByFamily
{
    public class GetItemAttibutesByFamilyHandler : IRequestHandler<GetItemAttibutesByFamilyQuery, List<ItemAttributeResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IItemAtributeRepository _itemAtributeRepository;

        public GetItemAttibutesByFamilyHandler(IMapper mapper, IItemAtributeRepository itemAtributeRepository)
        {
            _mapper = mapper;
            _itemAtributeRepository = itemAtributeRepository;
        }

        public async Task<List<ItemAttributeResponseDto>> Handle(GetItemAttibutesByFamilyQuery request, CancellationToken cancellationToken)
        {
            List<ItemAttribute> itemAttributes = await _itemAtributeRepository.GetItemsAttributesByFamily(request.ItemFamilyId);

            return _mapper.Map<List<ItemAttributeResponseDto>>(itemAttributes);
        }
    }
}