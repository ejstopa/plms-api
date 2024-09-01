using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Items;
using Application.Features.Items.Commands.CreateItemReservation;
using Application.Features.Items.Queries;
using Domain.Entities;
using Domain.Results;
using MediatR;

namespace Api.Endpoints
{
    public static class ItemEndpoints
    {
        public static void AddItemEndpoints(this WebApplication app)
        {
            var baseUrl = app.MapGroup("items");

            baseUrl.MapGet("{id}", async (ISender sender,  int id) =>
            {
                ItemResponseDto? item = await sender.Send(new GetItemByIdQuery{Id = id});

                if (item is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(item);
            }).WithName("GetItemById");

            baseUrl.MapPost("reservations", async (ISender sender, CreateItemReservationCommand createItemReservationCommand) =>
            {
                var result = await sender.Send(createItemReservationCommand);

                if (!result.IsSuccess)
                {
                    return Results.BadRequest(result.Error!.Message);
                }

                return Results.Ok(result.Value!);
            });
        }
    }
}