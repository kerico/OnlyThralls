using Microsoft.EntityFrameworkCore;
using OnlyThrals.Model;

namespace OnlyThrals.Data
{
    public class OnlyThrallsContext : DbContext
    {
        public OnlyThrallsContext(DbContextOptions<OnlyThrallsContext> options) : base(options)
        {

        }
        public DbSet<Thrall>? Thralls { get; set; }
    }


}
