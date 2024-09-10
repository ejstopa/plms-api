using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Commands.AuthenticateUser
{
    public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, UserResponseDto?>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public AuthenticateUserCommandHandler(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<UserResponseDto?> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {        
            User? user = await _userRepository.GetUserByName(request.Name);

            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return null;
            }
            
            return await Task.FromResult(_mapper.Map<UserResponseDto>(user));
        }
    }
}