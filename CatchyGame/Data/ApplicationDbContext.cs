using Library.Model;
using Microsoft.EntityFrameworkCore;

namespace CatchyGame.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }
        public DbSet<Score> Score { set; get; }
    }
}
