using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Results;
using MediatR;

namespace Application.Features.Users.Commands.DeleteUserWorkspaceFiles
{
    public class DeleteUserWorkspaceFilesCommand : IRequest<Result<bool>>
    {
        public string FilePath { get; set; } = string.Empty;
    }
}