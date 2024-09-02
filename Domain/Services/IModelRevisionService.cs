using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IModelRevisionService
    {
        public string IncrementRevision(string currentRevision);
      
    }
}