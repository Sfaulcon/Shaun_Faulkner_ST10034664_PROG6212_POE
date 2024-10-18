using Microsoft.EntityFrameworkCore;

namespace Shaun_Faulkner_ST10034664_PROG6212_POE.Models
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        }

        public DbSet<Claim> Claims { get; set; }

    }
}
