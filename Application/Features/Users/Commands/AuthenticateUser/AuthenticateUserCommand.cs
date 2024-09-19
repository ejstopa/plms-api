using Domain.Results;
using MediatR;

namespace Application.Features.Users.Commands.AuthenticateUser
{
    public class AuthenticateUserCommand : IRequest<Result<UserResponseDto?>>
    {       
        public string Name {get; set;} = string.Empty;
        public string Password {get; set;} = string.Empty;
    }
}