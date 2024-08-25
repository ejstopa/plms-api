using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserFile
    {
        public string Name { get; set; } = string.Empty;
        public string Extension {get; set;} = string.Empty;
        public string FullPath { get; set; } = string.Empty;
        public DateTime LastModifiedAt { get; set; }
        public bool IsRevision { get; set; }
    }
}