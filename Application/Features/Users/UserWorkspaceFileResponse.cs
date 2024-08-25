

namespace Application.Features.Users
{
    public class UserWorkspaceFileResponse
    {
        public string Name { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public string FullPath { get; set; } = string.Empty;
        public DateTime LastModifiedAt { get; set; }
        public bool IsRevision { get; set; }
    }
}