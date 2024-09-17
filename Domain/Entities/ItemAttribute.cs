using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ItemAttribute
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Unit {get; set;} = string.Empty;
        public List<ItemAttributeOption> options {get; set;} = [];

    }
}