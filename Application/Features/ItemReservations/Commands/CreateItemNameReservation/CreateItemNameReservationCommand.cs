using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Results;
using MediatR;

namespace Application.Features.ItemReservations.Commands.CreateItemNameReservation
{
    public class CreateItemNameReservationCommand : IRequest<Result<ItemNameReservationResponseDto>>
    {
        public int UserId {get; set;}
        public string ItemFamily {get; set;} = string.Empty;

    }
}