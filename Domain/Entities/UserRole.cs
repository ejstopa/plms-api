

namespace Domain.Entities
{
    public class UserRole
    {
        public int Id {get; set;}
        public string Name {get; set;} = string.Empty;
        public List<Department> Departments {get; set;} = [];
    }
}