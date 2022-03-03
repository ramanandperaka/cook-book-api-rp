using CookbookApi.SqlExpress.Model;
using Microsoft.EntityFrameworkCore;

namespace CookbookApi.SqlExpress.Data
{
    public class DataContext :DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
    }
}
