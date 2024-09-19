
using Application.Features.Users;
using Application.Features.Users.Commands.AuthenticateUser;
using Application.Features.Users.Commands.DeleteUserWorkspaceFiles;
using Domain.Results;
using MediatR;

namespace Api.Endpoints
{
    public static class UserEndpoints
    {
        public static void AddUserEndpoints(this WebApplication app)
        {
            var baseUrl = app.MapGroup("users");

            baseUrl.MapPost("login", async (ISender sender, AuthenticateUserCommand loginData) =>
            {
                Result<UserResponseDto?> result = await sender.Send(loginData);

                if (!result.IsSuccess)
                {
                    return Results.Problem(result.Error!.Message, null, result.Error.StatusCode);
                }

                return Results.Ok(result.Value);
            }).WithName("Login");

            baseUrl.MapDelete("{userId}/workspace-files/{filePath}", async (ISender sender, int userId, string filePath) =>
            {
                Result<bool> result = await sender.Send(new DeleteUserWorkspaceFilesCommand() { FilePath = filePath });

                if (!result.IsSuccess)
                {
                    return Results.Problem(result.Error!.Message, null, result.Error.StatusCode);
                }

                return Results.Ok(result.Value);
            }).WithName("DeleteUserWorkspaceFile");
        }
    }
}