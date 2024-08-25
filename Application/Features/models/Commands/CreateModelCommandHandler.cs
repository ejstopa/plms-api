using Application.Abstractions.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.models.Commands
{
    public class CreateModelCommandHandler : IRequestHandler<CreateModelCommand, ModelResponseDto>
    {
        private readonly IModelRepository _modelRepository;
        private readonly IMapper _mapper;

        public CreateModelCommandHandler(IMapper mapper, IModelRepository modelRepository)
        {
            _modelRepository = modelRepository;
            _mapper = mapper;
        }

        public async Task<ModelResponseDto> Handle(CreateModelCommand request, CancellationToken cancellationToken)
        {
            Model modelToCreate = _mapper.Map<Model>(request);

            Model modelCreated = await _modelRepository.CreateModel(modelToCreate);

            return _mapper.Map<ModelResponseDto>(modelCreated);
        }
    }
}