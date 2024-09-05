using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Endpoints
{
    public static class ItemNameReservationEndpoints
    {
        public static void AddItemNameReservationEndpoints(this WebApplication app)
        {
            var baseUrl = app.MapGroup("item-name-reservations");

            baseUrl.MapPost("", () =>
            {
                
            }).WithName("CreateItemNameReservation");
        }
    }
}