using Application.Features.models;
using Domain.Results;
using MediatR;

namespace Application.Features.Items.Commands.CreateItemReservation
{
    public class CreateItemReservationCommand : IRequest<Result<List<ModelResponseDto>>>
    {
        public int ItemId { get; set; }
        public int UserId { get; set; }
    }
}