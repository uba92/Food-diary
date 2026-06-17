using FoodDiary.Api.Dtos.Request;
using FoodDiary.Api.Dtos.Response;

namespace FoodDiary.Api.Interfaces
{
    public interface ISymptomEntryService
    {
        Task<List<SymptomEntryResponseDto>> GetAllAsync(DateTime? from, DateTime? to);
        Task<SymptomEntryResponseDto?> GetByIdAsync(int id);
        Task<SymptomEntryResponseDto> CreateAsync(CreateSymptomEntryRequest request);
        Task<SymptomEntryResponseDto?> UpdateAsync(int id, UpdateSymptomEntryRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
