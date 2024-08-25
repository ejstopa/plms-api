using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Entities
{
    public class Model
    {
        public int Id { get; private set; }
        public string Name { get; set; } = string.Empty;
        public string CommonName { get; set; } = string.Empty;
        public string Revision { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? LibraryFolder { get; set; } = string.Empty;
        public int CreatedBy { get; set; }
        public int? CheckedOutBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastModifiedAt { get; set; }
        public DateTime? CheckedOutAt { get; set; }
    }
}