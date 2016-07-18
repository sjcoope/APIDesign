using Microsoft.EntityFrameworkCore;
using SJCNet.APIDesign.Model;

namespace SJCNet.APIDesign.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {}

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./data.db");
        }
        
        public DbSet<Product> Products { get; set; }
    }
}