using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Items.Queries.GetItemsByDynamicParams
{
    public class GetItemsByDynamicParamsQuery : IRequest<List<ItemResponseDto>>
    {
        public int ItemFamilyId { get; set; }
        public List<DynamicSearchParam> SearchParams {get; set;} = [];
    }
}