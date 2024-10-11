using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.WorkflowInstances.Queries.GetAllWorkflowInstances
{
    public class GetAllWorkflowInstancesHandler : IRequestHandler<GetAllWorkflowInstancesQuery, List<WorkflowInstanceResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IWorkflowInstanceRepository _workflowInstanceRepository;

        public GetAllWorkflowInstancesHandler(IMapper mapper, IWorkflowInstanceRepository workflowInstanceRepository)
        {
            _mapper = mapper;
            _workflowInstanceRepository = workflowInstanceRepository;
        }

        public async Task<List<WorkflowInstanceResponseDto>> Handle(GetAllWorkflowInstancesQuery request, CancellationToken cancellationToken)
        {
            List<WorkflowInstance> workflows = await _workflowInstanceRepository.GetAllWorkflowInstances();

            return _mapper.Map<List<WorkflowInstanceResponseDto>>(workflows);

        }
    }
}