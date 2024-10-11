using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public class DynamicSearchParam
    {
        public int ItemAttributeId { get; set; }
        public char Operator { get; set; }
        public string? ValueString { get; set; } = string.Empty;
        public double? ValueNumber { get; set; }
    }
}