using Microsoft.EntityFrameworkCore;

namespace ProjectManagementApp.Model
{
    public class Project : DbContext{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
