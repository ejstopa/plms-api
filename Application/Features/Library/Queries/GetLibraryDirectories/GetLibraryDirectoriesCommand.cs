using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using MediatR;

namespace Application.Features.Library.Queries.GetLibraryDirectories
{
    public class GetLibraryDirectoriesCommand : IRequest<List<LibraryDirectoryResponse>>
    {
        
    }
}