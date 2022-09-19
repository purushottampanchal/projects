using bookWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace bookWeb.Data
{
    public class ApplicationDbContext:DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

    }
}
