using Application.Abstractions.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResponseDto?>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponseDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetUserById(request.Id);

            if (user is null)
            {
                return null;
            }

            UserResponseDto userResponseDto = new()
            {
                Id = user.Id,
                Name = user.Name,
                WindowsUser = user.WindowsUser,
                RoleId = user.RoleId
            };

            return await Task.FromResult(userResponseDto);
        }
    }
}