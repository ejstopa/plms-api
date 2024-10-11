using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.FileSystem;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Results;
using MediatR;

namespace Application.Features.WorkflowInstances.Commands.DeleteWorkflowInstance
{
    public class DeleteWorkflowInstanceHandler : IRequestHandler<DeleteWorkflowInstanceCommand, Result<bool>>
    {
        private readonly IWorkflowInstanceRepository _workflowInstanceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IUserWorkspaceFIlesService _userWorkspaceFIlesService;
        private readonly IUserWorkflowFilesService _userWorkflowFilesService;

        public DeleteWorkflowInstanceHandler(
            IWorkflowInstanceRepository workflowInstanceRepository,
            IUserRepository userRepository,
            IItemRepository itemRepository,
            IUserWorkspaceFIlesService userWorkspaceFIlesService,
            IUserWorkflowFilesService userWorkflowFilesService)
        {
            _workflowInstanceRepository = workflowInstanceRepository;
            _userRepository = userRepository;
            _itemRepository = itemRepository;
            _userWorkspaceFIlesService = userWorkspaceFIlesService;
            _userWorkflowFilesService = userWorkflowFilesService;
        }

        public async Task<Result<bool>> Handle(DeleteWorkflowInstanceCommand request, CancellationToken cancellationToken)
        {
            WorkflowInstance? workflow = await _workflowInstanceRepository.GetWorkflowInstanceById(request.WorkflowInstanceId);

            if (workflow is null)
            {
                return Result<bool>.Failure(new Error(404, "Workflow não encontrado"));
            }

            bool exclusionSucceed = await _workflowInstanceRepository.DeleteWorkflowInstance(request.WorkflowInstanceId);

            if (!exclusionSucceed)
            {
                return Result<bool>.Failure(new Error(409, "Ocorreu um erro ao tentar excluir o workflow"));
            }

            if (workflow.ItemId != null)
            {
                Item? item = await _itemRepository.GetItemById((int)workflow.ItemId!);

                if (item is null)
                {
                    return Result<bool>.Failure(new Error(409, "Ocorreu um erro ao tentar encontrar o item do workflow"));
                }

                Item? itemUpdated = await _itemRepository.SetItemStatus((int)item.Id!, ItemStatus.checkedOut);

                if (itemUpdated is null)
                {
                    return Result<bool>.Failure(new Error(409, "Ocorreu um erro ao tentar atualizar o status do item do workflow"));
                }
            }

            User? user = await _userRepository.GetUserById(workflow.UserId);

            if (user is null)
            {
                return Result<bool>.Failure(new Error(409, "Ocorreu um erro ao encontar o usuário responsável pelo workflow"));
            }

            string workspaceDirectory = _userWorkspaceFIlesService.GetUserWorkspaceDirectory(user);

            _userWorkflowFilesService.MoveFilesBackToWorkspace(workflow.ItemName, workspaceDirectory, user);

            return Result<bool>.Success(true);
        }
    }
}