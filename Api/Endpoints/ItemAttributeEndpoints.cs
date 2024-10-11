using Application.Features.ItemAttributes;
using Application.Features.ItemAttributes.Queries;
using MediatR;

namespace Api.Endpoints
{
    public static class ItemAttributeEndpoints
    {
        public static void AddItemAttributeEndpoints(this WebApplication app)
        {
            app.MapGet("item-families/{id}/attributes", async (ISender sender, int id) =>
            {
                List<ItemAttributeResponseDto> itemAttibutes = await sender.Send(new GetItemAttibutesByFamilyQuery { ItemFamilyId = id });
           
                return Results.Ok(itemAttibutes);
            }).WithName("GetItemAttibutesByFamily");
        }
    }
}