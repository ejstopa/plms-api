using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Features.models
{
    public class ModelResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CommonName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Version { get; set; } = 1;
        public string Revision { get; set; } = "-";
        public string FilePath { get; set; } = string.Empty;
        public int ItemId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CheckedOutBy { get; set; }
    }
}