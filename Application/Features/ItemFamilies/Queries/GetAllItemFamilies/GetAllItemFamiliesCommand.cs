using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.ItemFamilies.Queries.GetAllItemFamilies
{
    public class GetAllItemFamiliesCommand : IRequest<List<ItemFamilyResponseDto>>
    {
        
    }
}