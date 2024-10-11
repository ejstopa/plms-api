using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.ItemAttributes.Queries
{
    public class GetItemAttibutesByFamilyQuery : IRequest<List<ItemAttributeResponseDto>>
    {
        public int ItemFamilyId {get; set;}
    }
}