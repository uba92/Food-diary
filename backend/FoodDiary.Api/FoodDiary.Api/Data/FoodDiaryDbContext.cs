using FoodDiary.Api.Models;
using Microsoft.EntityFrameworkCore;
namespace FoodDiary.Api.Data
{
    public class FoodDiaryDbContext : DbContext
    {
        public FoodDiaryDbContext(DbContextOptions<FoodDiaryDbContext> options) : base(options)
        {
        }
        public DbSet<FoodAlternative> FoodAlternatives { get; set; }
    }
}
