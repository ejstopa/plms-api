using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Features.WorkflowInstances
{
    public class WorkflowStepResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public bool IsUserExclusive { get; set; }
        public int? StepOrder {get; set;}
         public List<ItemAttribute> ItemAttributes {get; set;} = [];
    }
}