
using Application.Features.models.Queries.DownloadModel;
using Domain.Services;
using MediatR;


namespace Api.Endpoints
{
    public static class ModelEndpoints
    {
        public static void AddModelEndpoints(this WebApplication app)
        {
            var baseUrl = app.MapGroup("models");

            baseUrl.MapGet("download/{filepath}", async (ISender sender, IFileNameService fileNameService, string filePath) =>
            {
                byte[] file = await sender.Send(new DownloadModelCommand { FilePath = filePath });
                if (file.Length == 0) return Results.Conflict("File" + filePath);

                string? fileName = fileNameService.GetFileName(filePath);
                if (fileName is null) return Results.Conflict("fileName");

                string? fileExtension = fileNameService.GetFileExtension(filePath);
                if (fileExtension is null) return  Results.Conflict("fileExtension");

                string? mimeType = fileNameService.GetFileMimiType(fileExtension!);
                if (mimeType is null) return Results.Conflict("mimeType" + fileExtension);


                return Results.File(file, mimeType, $"{fileName}{fileExtension}");
            });



        }


    }
}