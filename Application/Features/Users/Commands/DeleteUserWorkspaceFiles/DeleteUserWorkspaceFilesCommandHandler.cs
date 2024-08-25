using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.FileSystem;
using MediatR;

namespace Application.Features.Users.Commands.DeleteUserWorkspaceFiles
{
    public class DeleteUserWorkspaceFilesCommandHandler : IRequestHandler<DeleteUserWorkspaceFilesCommand, bool>
    {
        private readonly IUserWorkspaceFIlesService _userWorkspaceFIlesService;
        public DeleteUserWorkspaceFilesCommandHandler(IUserWorkspaceFIlesService userWorkspaceFIlesService)
        {
            _userWorkspaceFIlesService = userWorkspaceFIlesService;
        }

        public async Task<bool> Handle(DeleteUserWorkspaceFilesCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_userWorkspaceFIlesService.DeleteUserWorkspaceFile(request.FilePath));
        }
    }
}