using FoodDiary.Api.Data;
using FoodDiary.Api.Interfaces;
using FoodDiary.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDiary.Api.Repositories
{
    public class PlannedMealRepository : IPlannedMealRepository
    {
        private readonly FoodDiaryDbContext _context;
        public PlannedMealRepository(FoodDiaryDbContext context)
        {
            _context = context;
        }
        public async Task<PlannedMeal> AddPlannedMealAsync(PlannedMeal plannedMeal)
        {
            await _context.PlannedMeals.AddAsync(plannedMeal);
            return plannedMeal;
        }

        public void DeletePlannedMeal(PlannedMeal plannedMeal)
        {
            _context.PlannedMeals.Remove(plannedMeal);
        }

        public async Task<List<PlannedMeal>> GetAllPlannedMealsAsync()
        {
            return await _context.PlannedMeals.Include(p => p.FoodAlternative).ToListAsync();
        }

        public async Task<PlannedMeal?> GetPlannedMealByIdAsync(int id)
        {
            return await _context.PlannedMeals.Include(p => p.FoodAlternative).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
