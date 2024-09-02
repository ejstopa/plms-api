

namespace Application.Features.Users
{
    public class UserWorkspaceFileResponse
    {
        public string Name { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public string FullPath { get; set; } = string.Empty;
        public DateTime LastModifiedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Revision { get; set; } = string.Empty;
        public int ItemId { get; set; }
    }
}