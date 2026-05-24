using FoodDiary.Api.Models;

namespace FoodDiary.Api.Interfaces
{
    public interface IPlannedMealRepository
    {
        Task<List<PlannedMeal>> GetAllPlannedMealsAsync();
        Task<PlannedMeal?> GetPlannedMealByIdAsync(int id);
        Task<PlannedMeal> AddPlannedMealAsync(PlannedMeal plannedMeal);
        Task SaveChangesAsync();
        void DeletePlannedMeal(PlannedMeal plannedMeal);
    }
}
