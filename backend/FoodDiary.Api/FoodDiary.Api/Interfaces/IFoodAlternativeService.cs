using FoodDiary.Api.Dtos.Request;
using FoodDiary.Api.Dtos.Response;
using FoodDiary.Api.Models;

namespace FoodDiary.Api.Interfaces
{
    public interface IFoodAlternativeService
    {
        Task<List<FoodAlternativeResponseDto>> GetAllAsync();
        Task<FoodAlternativeResponseDto?> GetByIdAsync(int id);
        Task<FoodAlternativeResponseDto> CreateAsync(CreateFoodAlternativeRequest request);
        Task<FoodAlternativeResponseDto?> UpdateAsync(int id, UpdateFoodAlternativeRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
