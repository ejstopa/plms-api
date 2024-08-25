using MediatR;


namespace Application.Features.Users.Queries.GetUserWorkspaceFiles
{
    public class GetUserWorkspaceFilesCommand : IRequest<List<UserWorkspaceFileResponse>?>
    {
        public int UserId { get; set; }
    }
}