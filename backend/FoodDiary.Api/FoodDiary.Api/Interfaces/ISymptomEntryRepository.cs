using FoodDiary.Api.Models;

namespace FoodDiary.Api.Interfaces
{
    public interface ISymptomEntryRepository
    {
        Task<List<SymptomEntry>> GetAllSymptomEntriesAsync();
        Task<List<SymptomEntry>> GetByRangeAsync(DateTime from, DateTime to);
        Task<SymptomEntry?> GetSymptomEntryByIdAsync(int id);
        Task<SymptomEntry> AddSymptomEntryAsync(SymptomEntry symptomEntry);
        void DeleteSymptomEntry(SymptomEntry symptomEntry);
        Task SaveChangesAsync();
    }
}
