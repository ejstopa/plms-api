using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WorkflowStepFork
    {
        public int Id { get; set; }
        public int WorkflowTemplateId {get; set;}
        public int WorkflowStepId { get; set; }
        public string DecisionAttributeValue  { get; set; } = string.Empty;
        public int? DecisionAttributeId { get; set; }
        public int NextStepId { get; set; }
    }
}