using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Items.GetItemByIdQuery.Queries
{
    public class GetItemByIdQuery : IRequest<ItemResponseDto>
    {
        public int Id {get; set;}
    }
}