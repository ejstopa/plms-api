using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.models;
using Application.Features.models.Commands;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Endpoints
{
    public static class ModelEndpoints
    {
        public static void AddModelEndpoints(this WebApplication app)
        {
            var baseUrl = app.MapGroup("models");

            baseUrl.MapPost("", async (ISender sender, CreateModelCommand modelData) =>
            {
                ModelResponseDto model = await sender.Send(modelData);

                return Results.Ok(model);
            });
        }

    }
}