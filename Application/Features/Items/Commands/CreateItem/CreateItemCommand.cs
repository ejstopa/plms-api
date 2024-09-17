using Domain.Results;
using MediatR;

namespace Application.Features.Items.Commands.CreateItem
{
    public class CreateItemCommand : IRequest<Result<ItemResponseDto?>>
    {
        public int WorkflowInstanceId {get; set;}
    }
}