using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Items.Queries.GetItemsByUserWorkflows
{
    public class GetItemsByUserWorkflowsQuery : IRequest<List<ItemResponseDto>>
    {
        public string UserId {get; set;} =string.Empty;
    }
}