using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.FileSystem;
using AutoMapper;
using MediatR;

namespace Application.Features.Library.Queries.GetLibraryDirectories
{
    public class GetLibraryDirectoriesHandler : IRequestHandler<GetLibraryDirectoriesCommand, List<LibraryDirectoryResponse>>
    {
        private readonly ILibraryFilesService _libraryFilesService;
        private readonly IMapper _mapper;

        public GetLibraryDirectoriesHandler(ILibraryFilesService libraryFilesService, IMapper mapper)
        {
            _libraryFilesService = libraryFilesService;
            _mapper = mapper;
        }

        public async Task<List<LibraryDirectoryResponse>> Handle(GetLibraryDirectoriesCommand request, CancellationToken cancellationToken)
        {
           List<LibraryDirectoryResponse> libraryDirectories = _mapper.Map<List<LibraryDirectoryResponse>>(await _libraryFilesService.GetLibraryDirectories());

           return libraryDirectories;
        }
    }
}