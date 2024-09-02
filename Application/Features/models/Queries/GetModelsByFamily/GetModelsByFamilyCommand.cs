using MediatR;

namespace Application.Features.models.Queries.GetModelsByFamily
{
    public class GetModelsByFamilyCommand : IRequest<List<ModelResponseDto>>
    {
        public string Family {get; set;} = string.Empty;
    }
}