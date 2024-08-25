using MediatR;

namespace Application.Features.models.Commands
{
    public class CreateModelCommand : IRequest<ModelResponseDto>
    {
        public string FileName { get; set; } = string.Empty;
        public string CommonName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int CreatedBy { get; set; }
    }
}