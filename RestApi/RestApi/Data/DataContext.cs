using Microsoft.EntityFrameworkCore;
using RestApi.Entities;

namespace RestApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}
