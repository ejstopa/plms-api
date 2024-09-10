using Domain.Entities;

namespace Application.Features.Users
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string WindowsUser { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public UserRole? UserRole { get; set; }

    }
}