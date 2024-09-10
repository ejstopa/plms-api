using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WorkFlowStep
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public bool IsUserExclusive { get; set; }
        public int? StepOrder {get; set;}
        public List<ItemAttribute> ItemAttributes {get; set;} = [];
    }
}