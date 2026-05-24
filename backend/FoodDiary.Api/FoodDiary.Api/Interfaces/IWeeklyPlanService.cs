using FoodDiary.Api.Dtos.Request;
using FoodDiary.Api.Dtos.Response;
using FoodDiary.Api.Models;

namespace FoodDiary.Api.Interfaces
{
    public interface IWeeklyPlanService
    {
        Task<List<WeeklyPlanResponseDto>> GetAllWeeklyPlansAsync();
        Task<WeeklyPlanResponseDto?> GetWeeklyPlanByIdAsync(int id);
        Task<WeeklyPlanResponseDto> CreateWeeklyPlanAsync(CreateWeeklyPlanRequest request);
        Task<bool> UpdateWeeklyPlanAsync(int id, UpdateWeeklyPlanRequest request);
        Task<bool> DeleteWeeklyPlanAsync(int id);
    }
}
