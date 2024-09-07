using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.ItemFamilies;
using Application.Features.ItemFamilies.Queries.GetAllItemFamilies;
using Domain.Results;
using MediatR;

namespace Api.Endpoints
{
    public static class ItemFamilyEndpoints
    {
        public static void AddItemFamilyEndpoints(this WebApplication app)
        {
            var baseUrl = app.MapGroup("item-families");

            baseUrl.MapGet("", async (ISender sender) =>
            {
                List<ItemFamilyResponseDto> itemFamilies = await sender.Send(new GetAllItemFamiliesCommand());

                return Results.Ok(itemFamilies);
            }).WithName("GetAllItemFamilies");
        }
    }
}