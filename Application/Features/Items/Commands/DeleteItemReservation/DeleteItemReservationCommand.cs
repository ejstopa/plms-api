using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.models;
using Domain.Results;
using MediatR;

namespace Application.Features.Items.Commands.DeleteItemReservation
{
    public class DeleteItemReservationCommand : IRequest<Result<List<ModelResponseDto>>>
    {
        public int ItemId { get; set; }
        public int UserId { get; set; }
    }
}