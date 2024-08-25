

using MediatR;

namespace Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<UserResponseDto>
    {
        public required int Id {get; set;}
    }
}