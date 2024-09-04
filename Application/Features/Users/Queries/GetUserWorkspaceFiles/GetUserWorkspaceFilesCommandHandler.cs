using Application.Abstractions.FileSystem;
using Application.Abstractions.Repositories;
using Application.Features.Users.Queries.GetUserWorkspaceFiles;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Queries.GetUserFiles
{
    public class GetUserFilesCommandHandler : IRequestHandler<GetUserWorkspaceFilesCommand, List<UserWorkspaceFileResponse>?>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IUserWorkspaceFIlesService _userFIlesService;

        public GetUserFilesCommandHandler(IMapper mapper, IUserRepository userRepository, IUserWorkspaceFIlesService userFIlesService)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _userFIlesService = userFIlesService;
        }

        public async Task<List<UserWorkspaceFileResponse>?> Handle(GetUserWorkspaceFilesCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetUserById(request.UserId);

            if (user is null)
            {
                return null;
            }

            List<UserWorkspaceFileResponse> userFiles = _mapper.Map<List<UserWorkspaceFileResponse>>(
                 _userFIlesService.GetUserUserWorkspaceFiles(user));

            return userFiles;
        }


    }
}