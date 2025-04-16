using Microsoft.EntityFrameworkCore;
using SoulmateDAL.Entities;

namespace SoulmateDAL.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
    }
}
