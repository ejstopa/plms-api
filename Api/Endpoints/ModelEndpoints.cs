using Application.Features.models;
using Application.Features.models.Queries.GetModelsByFamily;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints
{
    public static class ModelEndpoints
    {
        public static void AddModelEndpoints(this WebApplication app)
        {
            var baseUrl = app.MapGroup("models");

            baseUrl.MapGet("", async (ISender sender, [FromQuery] string family) =>
            {
                List<ModelResponseDto> models = await sender.Send(new GetModelsByFamilyCommand { Family = family });

                if (models.Count == 0)
                {
                    return Results.NotFound("Nenhum model foi encontrado");
                }

                return Results.Ok(models);

            }).WithName("GetModelsByFamily");
            
        }


    }
}