using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.FileSystem;
using Application.Abstractions.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Results;
using MediatR;

namespace Application.Features.WorkflowInstances.Queries.GetWorkflowsByUser
{
    public class GetWorkflowsByUserHandler : IRequestHandler<GetWorkflowsByUserQuery, Result<List<WorkflowInstanceResponseDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IWorkflowInstanceRepository _workflowInstanceRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IUserWorkflowFilesService _userWorkflowFilesService;
        private readonly IUserRepository _userRepository;

        public GetWorkflowsByUserHandler(
        IMapper mapper,
        IWorkflowInstanceRepository workflowInstanceRepository, 
        IItemRepository itemRepository, 
        IUserWorkflowFilesService userWorkflowFilesService, 
        IUserRepository userRepository)
        {
            _mapper = mapper;
            _workflowInstanceRepository = workflowInstanceRepository;
            _itemRepository = itemRepository;
            _userWorkflowFilesService = userWorkflowFilesService;
            _userRepository = userRepository;
        }
        public async Task<Result<List<WorkflowInstanceResponseDto>>> Handle(GetWorkflowsByUserQuery request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetUserById(request.UserId);

            if (user is null){
                return Result<List<WorkflowInstanceResponseDto>>.Failure(new Error(400, "Usuário não encontrado"));
            }

            List<WorkflowInstance> workflows = await _workflowInstanceRepository.GetWorkflowsByUserId(user);

            List<WorkflowInstanceResponseDto> workflowInstanceResponseDtos = _mapper.Map<List<WorkflowInstanceResponseDto>>(workflows);

            return Result<List<WorkflowInstanceResponseDto>>.Success(workflowInstanceResponseDtos);
        }
    }
}