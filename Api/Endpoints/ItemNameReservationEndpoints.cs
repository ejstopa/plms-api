
using Application.Features.ItemReservations.Commands;
using Application.Features.ItemReservations.Commands.CreateItemNameReservation;
using Domain.Results;
using MediatR;

namespace Api.Endpoints
{
    public static class ItemNameReservationEndpoints
    {
        public static void AddItemNameReservationEndpoints(this WebApplication app)
        {
            var baseUrl = app.MapGroup("item-names");

            baseUrl.MapPost("", async (ISender sender, CreateItemNameReservationCommand itemNameReservationData) =>
            {
                Result<ItemNameReservationResponseDto> result = await sender.Send(itemNameReservationData);

                if (!result.IsSuccess)
                {
                    return Results.Problem(result.Error!.Message, null, result.Error.StatusCode);
                }

                return Results.Ok(result.Value);
            }).WithName("CreateItemNameReservation");
        }
    }
}