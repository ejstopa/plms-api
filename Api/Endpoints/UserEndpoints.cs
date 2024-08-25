
using Application.Features.Users;
using Application.Features.Users.Commands.AuthenticateUser;
using Application.Features.Users.Commands.DeleteUserWorkspaceFiles;
using Application.Features.Users.Queries.GetUserById;
using Application.Features.Users.Queries.GetUserWorkspaceFiles;
using MediatR;

namespace Api.Endpoints
{
    public static class UserEndpoints
    {
        public static void AddUserEndpoints(this WebApplication app)
        {
            var baseUrl = app.MapGroup("users");

            baseUrl.MapGet("{id}", async (ISender sender, int id) =>
            {
                GetUserByIdQuery getUserByIdQuery = new() { Id = id };
                UserResponseDto user = await sender.Send(getUserByIdQuery);

                if (user is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(user);
            }).WithName("GetUserById");

            baseUrl.MapPost("login", async (ISender sender, AuthenticateUserCommand loginData) =>
            {
                UserResponseDto user = await sender.Send(loginData);

                if (user is null)
                {
                    return Results.Unauthorized();
                }

                return Results.Ok(user);
            }).WithName("Login");

            baseUrl.MapGet("{userId}/workspace-files", async (ISender sender, int userId) =>
            {
                List<UserWorkspaceFileResponse>? userFileResponse = await sender.Send(new GetUserWorkspaceFilesCommand() { UserId = userId });

                if (userFileResponse is null)
                {
                    return Results.NotFound("Usuário não encontrado");
                }

                return Results.Ok(userFileResponse);
            }).WithName("GetUserWorkspaceFiles");

            baseUrl.MapDelete("{userId}/workspace-files/{filePath}", async (ISender sender, int userId, string filePath) =>{
                bool exclusionSucceded = await sender.Send(new DeleteUserWorkspaceFilesCommand() {FilePath = filePath});

                return exclusionSucceded?
                Results.Ok() :
                Results.Conflict(filePath);

            }).WithName("DeleteUserWorkspaceFile");
        }
    }
}