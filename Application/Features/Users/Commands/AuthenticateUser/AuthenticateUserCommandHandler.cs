using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Errors;
using Domain.Results;
using MediatR;

namespace Application.Features.Users.Commands.AuthenticateUser
{
    public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, Result<UserResponseDto?>>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public AuthenticateUserCommandHandler(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<Result<UserResponseDto?>> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {        
            User? user = await _userRepository.GetUserByName(request.Name);

            if (user is null || request.Password != user.Password)
            {
                return Result<UserResponseDto?>.Failure(new Error(401, "Usuário ou senha inválidos"));
            }
            
            return Result<UserResponseDto?>.Success(_mapper.Map<UserResponseDto>(user));
        }
    }
}