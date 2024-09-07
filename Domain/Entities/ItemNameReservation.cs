using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ItemNameReservation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string ItemFamily {get; set;} = string.Empty;
    }
}