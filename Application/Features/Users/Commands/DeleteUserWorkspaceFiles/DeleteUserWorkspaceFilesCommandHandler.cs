using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.FileSystem;
using Domain.Errors;
using Domain.Results;
using MediatR;

namespace Application.Features.Users.Commands.DeleteUserWorkspaceFiles
{
    public class DeleteUserWorkspaceFilesCommandHandler : IRequestHandler<DeleteUserWorkspaceFilesCommand, Result<bool>>
    {
        private readonly IFileManagementService _fileManagementService;

        public DeleteUserWorkspaceFilesCommandHandler(IFileManagementService fileManagementService)
        {
            _fileManagementService = fileManagementService;
        }

        public  Task<Result<bool>> Handle(DeleteUserWorkspaceFilesCommand request, CancellationToken cancellationToken)
        {
            bool fileDeleteSucceded = _fileManagementService.DeleteFile(request.FilePath);

            if (!fileDeleteSucceded)
            {
                Result<bool>.Failure(new Error(409, "Ocorreu um erro ao tentar deletar o arquivo"));
            }

            return Task.FromResult(Result<bool>.Success(true));
        }
    }
}