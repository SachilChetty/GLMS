using Microsoft.EntityFrameworkCore;
using GLMS.API.Models;

namespace GLMS.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Contract> Contracts { get; set; }
    }
}
