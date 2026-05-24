using FoodDiary.Api.Models;

namespace FoodDiary.Api.Interfaces
{
    public interface IWeeklyPlanRepository
    {
        Task<List<WeeklyPlan>> GetAllWeeklyPlansAsync();
        Task<WeeklyPlan?> GetWeeklyPlanByIdAsync(int id);
        Task<WeeklyPlan> AddWeeklyPlanAsync(WeeklyPlan weeklyPlan);
        Task SaveChangesAsync();
        void DeleteWeeklyPlan(WeeklyPlan weeklyPlan);
    }
}
