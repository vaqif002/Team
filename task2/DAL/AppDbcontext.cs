using Microsoft.EntityFrameworkCore;
using task2.Models;

namespace task2.DAL
{
    public class AppDbcontext:DbContext
    {

        public AppDbcontext(DbContextOptions<AppDbcontext>options):base(options) { }
        

        public DbSet<Team> Teams { get; set; }
    }
}
