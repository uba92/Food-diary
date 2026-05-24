using FoodDiary.Api.Dtos.Request;
using FoodDiary.Api.Dtos.Response;

namespace FoodDiary.Api.Interfaces
{
    public interface IPlannedMealService
    {
        Task<List<PlannedMealResponseDto>> GetAllPlannedMealsAsync();
        Task<PlannedMealResponseDto?> GetPlannedMealByIdAsync(int id);
        Task<PlannedMealResponseDto?> CreatePlannedMealAsync(CreatePlannedMealRequest request);
        Task<bool> DeletePlannedMealAsync(int id);
        Task<bool> UpdatePlannedMealAsync(int id, UpdatePlannedMealRequest request);
    }
}
