
using Application.Features.Items;
using Application.Features.Items.Commands.CreateItemReservation;
using Application.Features.Items.Commands.DeleteItemReservation;
using Application.Features.Items.Queries;
using Application.Features.Users.Commands.DeleteUserWorkspaceFiles;
using Domain.Entities;
using Domain.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints
{
    public static class ItemEndpoints
    {
        public static void AddItemEndpoints(this WebApplication app)
        {
            var baseUrl = app.MapGroup("items");

            baseUrl.MapGet("{id}", async (ISender sender, int id) =>
            {
                ItemResponseDto? item = await sender.Send(new GetItemByIdQuery { Id = id });

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

            baseUrl.MapDelete("reservations", async (ISender sender, [FromQuery] int itemId, [FromQuery] int userId) =>
            {
                var result = await sender.Send(new DeleteItemReservationCommand { ItemId = itemId, UserId = userId });

                if (!result.IsSuccess)
                {
                    return Results.BadRequest(result.Error!.Message);
                }

                return Results.Ok(result.Value!);
            });
        }
    }
}