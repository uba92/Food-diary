using FoodDiary.Api.Data;
using FoodDiary.Api.Interfaces;
using FoodDiary.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDiary.Api.Repositories
{
    public class WeeklyPlanRepository : IWeeklyPlanRepository
    {
        private readonly FoodDiaryDbContext _context;
        public WeeklyPlanRepository(FoodDiaryDbContext context)
        {
            _context = context;
        }
        public async Task<WeeklyPlan> AddWeeklyPlanAsync(WeeklyPlan weeklyPlan)
        {
            await _context.WeeklyPlans.AddAsync(weeklyPlan);
            return weeklyPlan;
        }

        public void DeleteWeeklyPlan(WeeklyPlan weeklyPlan)
        {
            _context.WeeklyPlans.Remove(weeklyPlan);
        }
        
        public async Task<List<WeeklyPlan>> GetAllWeeklyPlansAsync()
        {
            return await _context.WeeklyPlans.Include(w => w.PlannedMeals).ThenInclude(p => p.FoodAlternative).ToListAsync();
        }

        public async Task<WeeklyPlan?> GetWeeklyPlanByIdAsync(int id)
        {
            return await _context.WeeklyPlans.Include(w => w.PlannedMeals).ThenInclude(p => p.FoodAlternative).FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
