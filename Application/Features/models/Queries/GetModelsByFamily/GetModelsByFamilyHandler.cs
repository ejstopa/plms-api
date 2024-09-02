using Application.Abstractions.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.models.Queries.GetModelsByFamily
{
    public class GetModelsByFamilyHandler : IRequestHandler<GetModelsByFamilyCommand, List<ModelResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IModelRepository _modelRepository;

        public GetModelsByFamilyHandler(IMapper mapper, IModelRepository modelRepository)
        {
            _mapper = mapper;
            _modelRepository = modelRepository;
        }

        public async Task<List<ModelResponseDto>> Handle(GetModelsByFamilyCommand request, CancellationToken cancellationToken)
        {
            List<Model> models = await _modelRepository.GetModelsByFamily(request.Family);
            List<ModelResponseDto> modelResponseDtos = _mapper.Map<List<ModelResponseDto>>(models);

            return modelResponseDtos;
        }
    }
}