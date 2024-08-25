using Domain.Entities;

namespace Application.Features.Users
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string WindowsUser { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public UserResponseDto()
        {

        }

        public UserResponseDto(User user)
        {
            Id = user.Id;
            Name = user.Name;
            WindowsUser = user.WindowsUser;
            Role = user.Role;
        }

    }
}