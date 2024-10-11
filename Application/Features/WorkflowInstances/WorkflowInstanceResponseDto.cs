using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Items;

namespace Application.Features.WorkflowInstances
{
    public class WorkflowInstanceResponseDto
    {
        public int Id { get; set; }
        public int WorkflowTemplateId { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string ItemRevision { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int CurrentStepId { get; set; }
        public int? PreviousStepId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int? ItemFamilyId { get; set; }
        public int? ReturnedBy { get; set; }
        public ItemResponseDto? Item { get; set; }
    }
}