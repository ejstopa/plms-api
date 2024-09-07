using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IItemNameReservationService
    {
        public string? IncrementItemName(string name);

    }
}