using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Results;
using MediatR;

namespace Application.Features.Items.Queries.GetItemsByUserWorkspace
{
    public class GetItemsByUserWorkspaceCommand : IRequest<Result<List<ItemResponseDto>>>
    {
        public int UserId{get; set;}
    }
}