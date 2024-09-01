using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Library;
using Application.Features.Library.Queries.GetLibraryDirectories;
using MediatR;

namespace Api.Endpoints
{
    public static class LibraryEndpoints
    {
        public static void AddLibraryEndpoints(this WebApplication app)
        {
            var baseUrl = app.MapGroup("library");

            baseUrl.MapGet("directories", async (ISender sender) =>
            {
                List<LibraryDirectoryResponse> libraryDirectories = await sender.Send(new GetLibraryDirectoriesCommand());

                return Results.Ok(libraryDirectories);
            }).WithName("GetLiibraryDirectories");
        }


    }
}