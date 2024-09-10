using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ItemAttributeOption
    {
       public int Id {get; set;}
       public int ItemAttributeId {get; set;}
       public string Name {get; set;} = string.Empty;

    }
}