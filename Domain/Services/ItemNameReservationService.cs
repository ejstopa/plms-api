using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Domain.Services
{
    public class ItemNameReservationService : IItemNameReservationService
    {
        private readonly ILogger _logger;
        public ItemNameReservationService(ILogger<ItemNameReservationService> logger)
        {
            _logger = logger;
            
        }
        public string? IncrementItemName(string name)
        {
            string itemFamily = name[..4];
            string itemSequence = name[4..];
            string? newSequence = IncrementItemSequence(itemSequence);

            if (newSequence is null)
            {
                return null;
            }

            string newName = itemFamily + newSequence;

            return newName;
        }

        private string? IncrementItemSequence(string itemSequence)
        {
            bool isNumber = int.TryParse(itemSequence, out int result);

            if (!isNumber || result == 9999)
            {
                return null;
            }

            int newSequence = result + 1;
            int sequenceCharsNumber = newSequence.ToString().ToCharArray().Length;
            string sequenceString = newSequence.ToString();
            
            for (int i = 0; i < 4 - sequenceCharsNumber; i++)
            {
                sequenceString = $"0{sequenceString}";
            }

            return sequenceString;
        }
    }
}