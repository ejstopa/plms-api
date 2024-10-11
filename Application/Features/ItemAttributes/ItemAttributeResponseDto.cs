using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Features.ItemAttributes
{
    public class ItemAttributeResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Unit {get; set;} = string.Empty;
        public List<ItemAttributeOption> Options {get; set;} = [];
    }
}