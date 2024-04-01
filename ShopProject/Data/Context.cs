using Microsoft.EntityFrameworkCore;
using ShopProject.Models;

namespace ShopProject.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
        public DbSet<Product> products { get; set; }
    }
}
