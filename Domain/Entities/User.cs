using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public int Id {get; set;}
        public string Name {get; set;} = string.Empty;
        public string Password {get; set;} = string.Empty;
        public string WindowsUser {get; set;} = string.Empty;
        public string Role{get; set;} = string.Empty;
        public bool IsActive{get; set;}

    }
}