using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Items.Queries.GetItemsByFamily
{
    public class GetItemsByFamilyCommand : IRequest<List<ItemResponseDto>>
    {
        public string Family {get; set;} = string.Empty;
    }
}