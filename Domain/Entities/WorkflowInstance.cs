

namespace Domain.Entities
{
    public class WorkflowInstance
    {
        public int Id { get; set; }
        public int WorkflowTemplateId { get; set; }
        public int? ItemId { get; set; }
        public string ItemName{ get; set; } = string.Empty;
        public string ItemRevision { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int CurrentStepId { get; set; }
        public int? PreviousStepId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int ItemFamilyId {get; set;}
        public int? ReturnedBy { get; set; }
        public Item? Item {get; set;}
        public List<WorkFlowStep> WorkFlowSteps {get; set;} = [];
    }
}