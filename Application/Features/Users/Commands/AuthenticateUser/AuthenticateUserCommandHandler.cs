using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Commands.AuthenticateUser
{
    public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, UserResponseDto?>
    {
        private readonly IUserRepository _userRepository;

        public AuthenticateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponseDto?> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetUserByName(request.Name);

            if (user is null || user.Password != request.Password)
            {
                return null;
            }
            
            return await Task.FromResult(new UserResponseDto(user));
        }
    }
}