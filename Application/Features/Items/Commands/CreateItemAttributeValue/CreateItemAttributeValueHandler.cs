using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Results;
using MediatR;

namespace Application.Features.Items.Commands.CreateItemAttributeValue
{
    public class CreateItemAttributeValueHandler : IRequestHandler<CreateItemAttributeValueCommand, Result<ItemAttributeValue?>>
    {
        private readonly IItemAtributeRepository _itemAtributeRepository;
        private readonly IItemAtributeValueRepository _itemAttibuteValueRepository;

        public CreateItemAttributeValueHandler(IItemAtributeRepository itemAtributeRepository, IItemAtributeValueRepository itemAttibuteValueRepository)
        {
            _itemAtributeRepository = itemAtributeRepository;
            _itemAttibuteValueRepository = itemAttibuteValueRepository;
        }

        public async Task<Result<ItemAttributeValue?>> Handle(CreateItemAttributeValueCommand request, CancellationToken cancellationToken)
        {
            ItemAttribute? atribute = await _itemAtributeRepository.GetItemAtribute(request.AttributeId);

            if (atribute is null)
            {
                return Result<ItemAttributeValue?>.Failure(new Error(400, "Attributo não encontrado"));
            }

            ItemAttributeValue? attributeValue = await _itemAttibuteValueRepository.GetItemAttributeValue(request.ItemId, request.AttributeId);

            if (attributeValue is null)
            {
                return await CreateAttributeValue(request, atribute);
            }
            else
            {
                return await UpdateAttributeValue(request, atribute);
            }
        }

        private async Task<Result<ItemAttributeValue?>> CreateAttributeValue(CreateItemAttributeValueCommand request, ItemAttribute atribute)
        {
            ItemAttributeValue? newAttributeValue;

            if (atribute.Type == ItemAtributeTypes.typeNumber.ToString())
            {
                bool isDouble = double.TryParse(request.Value, out double result);

                if (!isDouble)
                {
                    return Result<ItemAttributeValue?>.Failure(new Error(400, "Valor do atributo com formato incorreto"));
                }

                newAttributeValue = await _itemAttibuteValueRepository.CreateItemAttributeValue(request.ItemId, request.AttributeId, result);
            }
            else
            {
                newAttributeValue = await _itemAttibuteValueRepository.CreateItemAttributeValue(request.ItemId, request.AttributeId, request.Value);
            }

            if (newAttributeValue is null)
            {
                return Result<ItemAttributeValue?>.Failure(new Error(409, "Ocorreu um problema na criação do valor do atributo"));
            }

            return Result<ItemAttributeValue?>.Success(newAttributeValue);
        }

        private async Task<Result<ItemAttributeValue?>> UpdateAttributeValue(CreateItemAttributeValueCommand request, ItemAttribute atribute)
        {
            ItemAttributeValue? newAttributeValue;

            if (atribute.Type == ItemAtributeTypes.typeNumber.ToString())
            {
                bool isDouble = double.TryParse(request.Value, out double result);

                if (!isDouble)
                {
                    return Result<ItemAttributeValue?>.Failure(new Error(400, "Valor do atributo com formato incorreto"));
                }

                newAttributeValue = await _itemAttibuteValueRepository.UpdateItemAttributeValue(request.ItemId, request.AttributeId, result);
            }
            else
            {
                newAttributeValue = await _itemAttibuteValueRepository.UpdateItemAttributeValue(request.ItemId, request.AttributeId, request.Value);
            }

            if (newAttributeValue is null)
            {
                return Result<ItemAttributeValue?>.Failure(new Error(409, "Ocorreu um problema na atualizção valor do atributo"));
            }

            return Result<ItemAttributeValue?>.Success(newAttributeValue);
        }
    }
}