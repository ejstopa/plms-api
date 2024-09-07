using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Features.ItemReservations.Commands
{
    public class ItemNameReservationResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string ItemFamily { get; set; } = string.Empty;
    }
}