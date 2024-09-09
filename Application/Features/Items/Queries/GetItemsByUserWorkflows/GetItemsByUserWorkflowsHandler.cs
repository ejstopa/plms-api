using MediatR;

namespace Application.Features.Items.Queries.GetItemsByUserWorkflows
{
    public class GetItemsByUserWorkflowsHandler : IRequestHandler<GetItemsByUserWorkflowsQuery, List<ItemResponseDto>>
    {
        public Task<List<ItemResponseDto>> Handle(GetItemsByUserWorkflowsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}