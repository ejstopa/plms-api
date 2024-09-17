using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.models;
using Domain.Entities;

namespace Application.Features.Items
{
    public class ItemResponseDto
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Revision { get; set; } = "-";
        public int Version { get; set; }
        public string? Description { get; set; } = string.Empty;
        public string? Family { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public int? CheckedOutBy { get; set; }
        public int FamilyId { get; set; }
        public DateTime? CheckedOutAt { get; set; }
        public List<ModelResponseDto> Models { get; set; } = [];
    }
}