using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class LibraryDirectory
    {
        public string Name {get;set;} = string.Empty;
        public string FullPath {get; set;} = string.Empty;
    }
}