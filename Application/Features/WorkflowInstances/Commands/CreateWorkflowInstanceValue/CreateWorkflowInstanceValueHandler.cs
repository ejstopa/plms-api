using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Results;
using MediatR;


namespace Application.Features.WorkflowInstances.Commands.CreateWorkflowInstanceValue
{
    public class CreateWorkflowInstanceValueHandler : IRequestHandler<CreateWorkflowInstanceValueCommand, Result<WorkflowInstanceValue?>>
    {
        private readonly IWorkflowInstanceValueRepository _workflowInstanceValueRepository;
        private readonly IItemAtributeRepository _itemAtributeRepository;

        public CreateWorkflowInstanceValueHandler(IWorkflowInstanceValueRepository workflowInstanceValueRepository, IItemAtributeRepository itemAtributeRepository)
        {
            _itemAtributeRepository = itemAtributeRepository;
            _workflowInstanceValueRepository = workflowInstanceValueRepository;
        }

        public async Task<Result<WorkflowInstanceValue?>> Handle(CreateWorkflowInstanceValueCommand request, CancellationToken cancellationToken)
        {
            ItemAtribute? atribute = await _itemAtributeRepository.GetItemAtribute(request.ItemAttributeId);

            if (atribute is null)
            {
                return Result<WorkflowInstanceValue?>.Failure(new Error(400, "Attributo não encontrado"));
            }

            WorkflowInstanceValue? attributeValue = await _workflowInstanceValueRepository.GetWorkflowInstanceValue(request.WorkflowInstanceId, request.ItemAttributeId);

            if (attributeValue is null)
            {
                return await CreateAttributeValue(request, atribute);
            }
            else
            {
                return await UpdateAttributeValue(request, atribute);
            }
        }

        private async Task<Result<WorkflowInstanceValue?>> CreateAttributeValue(CreateWorkflowInstanceValueCommand request, ItemAtribute atribute)
        {
            WorkflowInstanceValue? newAttributeValue;

            if (atribute.Type == ItemAtributeTypes.typeNumber.ToString())
            {
                bool isDouble = double.TryParse(request.Value, out double result);

                if (!isDouble)
                {
                    return Result<WorkflowInstanceValue?>.Failure(new Error(400, "Valor do atributo com formato incorreto"));
                }

                newAttributeValue = await _workflowInstanceValueRepository.CreateWorkflowInstanceValue(request.WorkflowInstanceId, request.ItemAttributeId, result);
            }
            else
            {
                newAttributeValue = await _workflowInstanceValueRepository.CreateWorkflowInstanceValue(request.WorkflowInstanceId, request.ItemAttributeId, request.Value);
            }

            if (newAttributeValue is null)
            {
                return Result<WorkflowInstanceValue?>.Failure(new Error(409, "Ocorreu um problema na criação do valor do atributo"));
            }

            return Result<WorkflowInstanceValue?>.Success(newAttributeValue);
        }

        private async Task<Result<WorkflowInstanceValue?>> UpdateAttributeValue(CreateWorkflowInstanceValueCommand request, ItemAtribute atribute)
        {
            WorkflowInstanceValue? newWorkflowInstanceValue;

            if (atribute.Type == ItemAtributeTypes.typeNumber.ToString())
            {
                bool isDouble = double.TryParse(request.Value, out double result);

                if (!isDouble)
                {
                    return Result<WorkflowInstanceValue?>.Failure(new Error(400, "Valor do atributo com formato incorreto"));
                }

                newWorkflowInstanceValue = await _workflowInstanceValueRepository.UpdateWorkflowInstanceValue(request.WorkflowInstanceId, request.ItemAttributeId, result);
            }
            else
            {
                newWorkflowInstanceValue = await _workflowInstanceValueRepository.UpdateWorkflowInstanceValue(request.WorkflowInstanceId, request.ItemAttributeId, request.Value);
            }

            if (newWorkflowInstanceValue is null)
            {
                return Result<WorkflowInstanceValue?>.Failure(new Error(409, "Ocorreu um problema na atualizção valor do atributo"));
            }

            return Result<WorkflowInstanceValue?>.Success(newWorkflowInstanceValue);
        }
    }
}